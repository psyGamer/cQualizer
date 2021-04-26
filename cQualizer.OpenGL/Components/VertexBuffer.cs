using OpenTK.Graphics.OpenGL4;

namespace cQualizer.OpenGL.Components {

	public class VertexBuffer : IBindable {

		public int BufferID { get; private set; } = -1;

		public VertexBuffer(float[] data, int size = -1, BufferUsageHint usage = BufferUsageHint.StaticDraw) {
			BufferID = GL.GenBuffer();
			Enable();
			GL.BufferData(BufferTarget.ArrayBuffer, (size < 0) ? data.Length * sizeof(float) : size, data, usage);
		}

		~ VertexBuffer() {
			GL.DeleteBuffer(BufferID);
		}

		public void Enable() {
			GL.BindBuffer(BufferTarget.ArrayBuffer, BufferID);
		}

		public void Disable() {
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		}
	}
}
