using System;

using cQualizer.OpenGL.Util;

using OpenTK.Graphics.OpenGL4;

namespace cQualizer.OpenGL.Graphics {

	public class Shader : IBindable {

		public int Program { get; private set; } = -1;

		public Shader(string vertexCode, string fragmentCode) {

			int status;

			int vertexShader = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(vertexShader, vertexCode);
			GL.CompileShader(vertexShader);
			GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out status);

			if (status == 0)
				Console.WriteLine("Warning ! Vertex shader could not be compiled: " + GL.GetShaderInfoLog(vertexShader));

				int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(fragmentShader, fragmentCode);
			GL.CompileShader(fragmentShader);
			GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out status);

			if (status == 0)
				Console.WriteLine("Warning ! Fragment shader could not be compiled: " + GL.GetShaderInfoLog(fragmentShader));

			Program = GL.CreateProgram();

			GL.AttachShader(Program, vertexShader);
			GL.AttachShader(Program, fragmentShader);
			GL.LinkProgram(Program);
			GL.GetProgram(Program, GetProgramParameterName.LinkStatus, out status);

			if (status == 0)
				Console.WriteLine("Warning ! Program could not be linked: " + GL.GetShaderInfoLog(fragmentShader));
		}

		public void Enable() {
			GL.UseProgram(Program);
		}

		public void Disable() {
			GL.UseProgram(0);
		}
	}
}
