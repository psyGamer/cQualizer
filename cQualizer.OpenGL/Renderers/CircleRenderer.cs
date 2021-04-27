using cQualizer.OpenGL.Utils;
using cQualizer.OpenGL.Components.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;

namespace cQualizer.OpenGL.Renderers {

	public class CircleRenderer : IRenderer {

		public override Shader Shader { get; protected set; }
		public VertexArray VertexArray { get; private set; }

		private uint sampleSize;
		private float radius;

		private float[] vertecieAngles;
		private uint [] indicies;

		public CircleRenderer(uint sampleSize, float radius) {
			this.sampleSize = sampleSize;
			this.radius = radius;

			vertecieAngles = new float[sampleSize * 4 + 1];
			indicies = new uint[sampleSize * 4 * 3];

			vertecieAngles[0] = -1.0f;

			for (uint i = 0 ; i < sampleSize * 4 ; i++) {
				vertecieAngles[i + 1] = i * 90.0f / sampleSize;

				indicies[i * 3 + 0] =      0;
				indicies[i * 3 + 1] =  i + 1;
				indicies[i * 3 + 2] = (i + 2) % (sampleSize * 4);
			}

			VertexArray = new VertexArray(new VertexBuffer(vertecieAngles), new IndexBuffer(indicies));
			Shader = ShaderUtil.LoadShader("Circle");
		}

		public override void Render(ApplicationWindow window, Vector2 aspectRatio) {
			Shader.Enable();
			Shader.SetUniformFloat("uRadius", radius);
			VertexArray.Enable();

			GL.VertexAttribPointer(0, 1, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexAttribArray(0);

			GL.DrawElements(PrimitiveType.Triangles, indicies.Length, DrawElementsType.UnsignedInt, 0);

			VertexArray.Disable();
			Shader.Disable();
		}

		public override void Update(ApplicationWindow window, Vector2 aspectRatio) {
			Console.WriteLine($"Old radius: {radius}");
			radius = (radius + new Random().Next(-1, 1) / 100) % .8f + .1f;
			Console.WriteLine($"New radius: {radius}");
		}
	}
}
