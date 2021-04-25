using OpenTK.Graphics.OpenGL4;

namespace cQualizer.OpenGL.Utils {

	public class VertexArray : IBindable {

		public int ID { get; private set; } = -1;
		public VertexBuffer VertexBuffer { get; private set; }

		public VertexArray() {
			ID = GL.GenVertexArray();

			Enable();

			//VertexBuffer = new VertexBuffer();
		}

		public void Enable() {
			GL.BindVertexArray(ID);
		}

		public void Disable() {
			GL.BindVertexArray(0);
		}
	}
}
