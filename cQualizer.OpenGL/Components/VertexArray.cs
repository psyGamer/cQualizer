using OpenTK.Graphics.OpenGL4;

namespace cQualizer.OpenGL.Components {

	public class VertexArray : IBindable {

		public int ArrayID { get; private set; } = -1;
		public VertexBuffer VertexBuffer { get; private set; }
		public IndexBuffer IndexBuffer { get; private set; }

		public VertexArray(VertexBuffer vertexBuffer, IndexBuffer indexBuffer = null) {
			ArrayID = GL.GenVertexArray();

			Enable();
			vertexBuffer.Enable();
			indexBuffer?.Enable();

			VertexBuffer = vertexBuffer;
			IndexBuffer = indexBuffer;
		}

		~ VertexArray() {
			GL.DeleteVertexArray(ArrayID);
		}

		public void Enable() {
			GL.BindVertexArray(ArrayID);
		}

		public void Disable() {
			GL.BindVertexArray(0);
		}
	}
}
