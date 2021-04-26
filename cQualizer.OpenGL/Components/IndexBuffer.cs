using OpenTK.Graphics.OpenGL4;

namespace cQualizer.OpenGL.Components {

	public class IndexBuffer : IBindable {

		public int BufferID { get; private set; } = -1;
		public int Count { get; private set; } = -1;

		public IndexBuffer(uint[] data, int count = -1, BufferUsageHint usage = BufferUsageHint.StaticDraw) {
			BufferID = GL.GenBuffer();
			Enable();
			GL.BufferData(BufferTarget.ElementArrayBuffer, ((count < 0) ? data.Length : count) * sizeof(uint), data, usage);
		}

		~ IndexBuffer() {
			GL.DeleteBuffer(BufferID);
		}

		public void Enable() {
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, BufferID);
		}

		public void Disable() {
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
		}
	}
}
