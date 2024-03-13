using OpenTK.Graphics.OpenGL4;

namespace FuckedUpPlatformer.Graphics.Buffers
{
    public interface IElementBufferObject
    {
        BufferUsageHint BufferUsageHint { get; }
        DrawElementsType DrawElementsType { get; }
        int ElementSizeInBytes { get; }
        bool IsDisposed { get; }

        void Bind();
    }
}