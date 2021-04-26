using cQualizer.OpenGL.Loaders;
using cQualizer.OpenGL.Utils;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Text;

namespace cQualizer.OpenGL.Renderers {

	public class CircleRenderer : IRenderer {

		public Shader Shader { get; private set; }
		public VertexArray VertexArray { get; private set; }

		private uint sampleSize;
		private float radius;

		private float[] vertecieAngles;
		private uint [] indicies;

		public CircleRenderer(uint sampleSize, float radius) {
			this.sampleSize = sampleSize;
			this.radius = radius;
		}

		public void Load() {

			vertecieAngles = new float[sampleSize * 4 + 1];
			indicies	   = new uint [sampleSize * 4 * 3];

			vertecieAngles[0] = -1.0f;

			for (uint i = 0 ; i < sampleSize * 4 ; i++) {
				vertecieAngles[i + 1] = i * 90.0f / sampleSize;

				indicies[i * 3 + 0] = 0;
				indicies[i * 3 + 1] = i + 1;
				indicies[i * 3 + 2] = (uint) ((i + 2) % (sampleSize * 4));
			}

			VertexArray = new VertexArray(new VertexBuffer(vertecieAngles), new IndexBuffer(indicies));
			Shader = ShaderLoader.LoadShader("Circle");

			/*
			float[] vertecies = new float[sampleSize * 4 * 2 + 2];
			uint [] indicies  = new uint [sampleSize * 4 * 3];

			vertecies[sampleSize * 4 * 2 + 0] = 0.0f;
			vertecies[sampleSize * 4 * 2 + 1] = 0.0f;

			for (float a = 0 ; a < 360 ; a += 90.0f / sampleSize) {
				radius = (float) new Random().NextDouble();
				float radiens = (MathF.PI / 180) * a;
				float x = MathF.Cos(radiens) * radius;
				float y = MathF.Sin(radiens) * radius;
				uint i = (uint) (a / (90 / sampleSize));

				vertecies[i * 2 + 0] = x;
				vertecies[i * 2 + 1] = y;

				indicies[i * 3 + 0] = 0;
				indicies[i * 3 + 1] = i + 1;
				indicies[i * 3 + 2] = (uint) ((i + 2) % (sampleSize * 4));
			}

			var vertexBuffer = new VertexBuffer(vertecies);
			var indexBuffer  = new IndexBuffer (indicies);

			vertexArray = new VertexArray (vertexBuffer, indexBuffer);
			*/
		}

		public void Render() {
			Shader.Enable();
			Shader.SetUniformFloat("uRadius", radius);
			VertexArray.Enable();

			GL.VertexAttribPointer(0, 1, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexAttribArray(0);

			GL.DrawElements(PrimitiveType.Triangles, indicies.Length, DrawElementsType.UnsignedInt, 0);

			VertexArray.Disable();
			Shader.Disable();
		}

		public void Update() {
			Console.WriteLine($"Old radius: {radius}");
			radius = (radius + new Random().Next(-1, 1) / 100000) % 0.8f + 0.1f;
			Console.WriteLine($"New radius: {radius}");
		}
	}
}
