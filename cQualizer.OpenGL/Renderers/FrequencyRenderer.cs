using cQualizer.OpenGL.Components.Graphics;
using cQualizer.OpenGL.Utils;

using Vector2 = OpenTK.Mathematics.Vector2;

using System.Numerics;
using OpenTK.Graphics.OpenGL4;
using System;

namespace cQualizer.OpenGL.Renderers {

	public class FrequencyRenderer : IRenderer {
		public override Shader Shader { get; protected set; }

		private VertexArray vertexArray;
		private VertexBuffer vertexBuffer;
		private IndexBuffer indexBuffer;

		private float[] vertecies = new float[0];
		private uint[] indicies = new uint[0];

		public SoundSignal signal;

		public FrequencyRenderer() {
			signal = new SoundSignal();
			signal.Start();

			Shader = ShaderUtil.LoadShader("Freqency");
		}

		public void UpdateWave(Complex[] Wave, int max, bool useImaginary = false) {
			if (vertecies.Length != Wave.Length * 2) {
				vertecies = new float[Wave.Length * 2];
				indicies = new uint[Wave.Length * 2 - 2];
			}

			for (uint i = 0 ; i < Math.Min(Wave.Length, max) ; i++) {
				vertecies[i * 2 + 0] = 1 / (Wave.Length - 1.0f) * i * 2 - 1;
				vertecies[i * 2 + 1] = MathF.Abs((float) (useImaginary ? Wave[i].Imaginary : Wave[i].Real) * 2) - 0.9f;

				if (i * 2 >= Wave.Length * 2 - 2)
					break;

				indicies[i * 2 + 0] = i;
				indicies[i * 2 + 1] = i + 1;
			}

			vertexBuffer = new VertexBuffer(vertecies);
			indexBuffer = new IndexBuffer(indicies);
			vertexArray = new VertexArray(vertexBuffer, indexBuffer);

			GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexAttribArray(0);
		}

		public override void Render(ApplicationWindow window, Vector2 aspectRatio) {
			if (!up) return;
			Shader.Enable();
			vertexArray.Enable();

			GL.DrawElements(BeginMode.Lines, indicies.Length, DrawElementsType.UnsignedInt, 0);

			vertexArray.Disable();
			Shader.Disable();
		}

		private bool up = false;

		public override void Update(ApplicationWindow window, Vector2 aspectRatio) {
			if (signal.FFT == null)
				return;

			Console.WriteLine(signal.FFT.Length);
			Console.WriteLine(signal.FFT[0]);
			UpdateWave(signal.FFT, int.MaxValue, false);
			up = true;
		}
	}
}
