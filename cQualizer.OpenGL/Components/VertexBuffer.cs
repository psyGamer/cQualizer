using OpenTK.Graphics.OpenGL4;
using System;

namespace cQualizer.OpenGL.Components {

	public class VertexBuffer : IBindable {

		public int BufferID { get; private set; } = -1;

		public VertexBuffer() { }

		public VertexBuffer(float[] data, int size = -1, BufferUsageHint usage = BufferUsageHint.StaticDraw) {
			BufferID = GL.GenBuffer();
			SetData(data, size, usage);
		}

		public void SetData(float[] data, int size = -1, BufferUsageHint usage = BufferUsageHint.StaticDraw) {
			Enable();
			GL.BufferData(BufferTarget.ArrayBuffer, (size < 0) ? data.Length * sizeof(float) : size, data, usage);
		}

		public void UpdateData(float[] data, int offset, int size) {
			Enable();
			GL.BufferSubData(BufferTarget.ArrayBuffer, new IntPtr(offset), size, data);
		}

		~ VertexBuffer() {
			//GL.DeleteBuffer(BufferID);
		}

		public void Enable() {
			GL.BindBuffer(BufferTarget.ArrayBuffer, BufferID);
		}

		public void Disable() {
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		}
	}
}
