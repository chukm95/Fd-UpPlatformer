using OpenTK.Graphics.OpenGL4;
using SkiaSharp;
using System.IO;
using System.Reflection;

namespace FuckedUpPlatformer.Resources.Textures
{
    internal class Texture
    {
        public string FilePath => _filePath;
        public int Width => _width;
        public int Height => _height;
        public bool IsDisposed => _isDisposed;

        private string _filePath;
        private int _width;
        private int _height;
        private byte[] _pixels;
        private int _textureId;
        private bool _isDisposed;

        public Texture(string filePath) {
            _filePath = filePath;
            LoadTexture(filePath);
            InitializeTexture();
            _isDisposed = false;
        }

        private void LoadTexture(string filepath) {
            using (SKBitmap bmp = SKBitmap.Decode(Assembly.GetExecutingAssembly().GetManifestResourceStream(filepath))) {
                _width = bmp.Width;
                _height = bmp.Height;
                _pixels = bmp.Bytes;
            }
        }

        private void InitializeTexture() {
            _textureId = GL.GenTexture();
            if (_textureId <= 0) throw new System.Exception($"Invalid texture ID: {_textureId}");

            GL.BindTexture(TextureTarget.Texture2D, _textureId);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, _width, _height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, _pixels);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void Bind(int textureUnit) {
            if (!_isDisposed)
                GL.BindTextureUnit(textureUnit, _textureId);
        }

        public void Dispose() {
            if (!_isDisposed) {
                _isDisposed = true;
                GL.DeleteTexture(_textureId);
                _textureId = -1;
            };
        }
    }
}