using FuckedUpPlatformer.Graphics.Vertices;
using OpenTK.Graphics.OpenGL4;
using System.Collections.ObjectModel;

namespace FuckedUpPlatformer.Graphics.Buffers
{
    public interface IVertexBufferObject
    {
        ReadOnlyCollection<VertexDescription> VertexDescription { get; }
        BufferUsageHint BufferUsageHint { get; }
        int VertexSizeInBytes { get; }
        bool IsDisposed { get; }

        void Bind();
    }
}
