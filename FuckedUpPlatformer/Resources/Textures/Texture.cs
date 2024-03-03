using SkiaSharp;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using System;

namespace FuckedUpPlatformer.Resources.Textures
{
    internal class Texture
    {
        private int _width;
        private int _height;
        private byte[] _pixels;
        private int _textureId;
        private bool _isDisposed;

        public Texture(string filePath) {
            LoadTexture(filePath);
            InitializeTexture();

            _isDisposed = false;

            Console.WriteLine($"TextureId {_textureId}");
        }

        private void LoadTexture(string filepath) {
            using (SKBitmap bmp = SKBitmap.Decode(File.Open(filepath, FileMode.Open))) {
                _width = bmp.Width;
                _height = bmp.Height;
                _pixels = bmp.Bytes;
            }
        }

        private void InitializeTexture() {
            _textureId = GL.GenTexture();
            if (_textureId <= 0) throw new System.Exception($"Invalid texture ID: {_textureId}");

            GL.BindTexture(TextureTarget.Texture2D, _textureId);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
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