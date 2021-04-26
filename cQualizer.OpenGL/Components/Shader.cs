using System;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace cQualizer.OpenGL.Components {

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
				Console.WriteLine("Warning ! Program could not be linked: " + GL.GetShaderInfoLog(Program));
		}

		#region Vector1
		public void SetUniformInt(string uniformName, int i1) {
			Enable();
			GL.Uniform1(GL.GetUniformLocation(Program, uniformName), i1);
		}
		public void SetUniformFloat(string uniformName, float f1) {
			Enable();
			GL.Uniform1(GL.GetUniformLocation(Program, uniformName), f1);
		}
		public void SetUniformDouble(string uniformName, float f1) {
			Enable();
			GL.Uniform1(GL.GetUniformLocation(Program, uniformName), f1);
		}
		#endregion

		#region Vector2
		public void SetUniformVector2(string uniformName, Vector2 vector) {
			Enable();
			GL.Uniform2(GL.GetUniformLocation(Program, uniformName), vector);
		}
		public void SetUniformVector2i(string uniformName, int v1, int v2) {
			Enable();
			GL.Uniform2(GL.GetUniformLocation(Program, uniformName), v1, v2);
		}
		public void SetUniformVector2f(string uniformName, float v1, float v2) {
			Enable();
			GL.Uniform2(GL.GetUniformLocation(Program, uniformName), v1, v2);
		}
		public void SetUniformVector2d(string uniformName, double v1, double v2) {
			Enable();
			GL.Uniform2(GL.GetUniformLocation(Program, uniformName), v1, v2);
		}
		#endregion

		#region Vector3
		public void SetUniformVector3(string uniformName, Vector3 vector) {
			Enable();
			GL.Uniform3(GL.GetUniformLocation(Program, uniformName), vector);
		}
		public void SetUniformVector3i(string uniformName, int v1, int v2, int v3) {
			Enable();
			GL.Uniform3(GL.GetUniformLocation(Program, uniformName), v1, v2, v3);
		}
		public void SetUniformVector3f(string uniformName, float v1, float v2, float v3) {
			Enable();
			GL.Uniform3(GL.GetUniformLocation(Program, uniformName), v1, v2, v3);
		}
		public void SetUniformVector3d(string uniformName, double v1, double v2, double v3) {
			Enable();
			GL.Uniform3(GL.GetUniformLocation(Program, uniformName), v1, v2, v3);
		}
		#endregion

		#region Vector4
		public void SetUniformColor(string uniformName, Color4 color) {
			Enable();
			GL.Uniform4(GL.GetUniformLocation(Program, uniformName), color);
		}
		public void SetUniformQuaternion(string uniformName, Quaternion quaterníon) {
			Enable();
			GL.Uniform4(GL.GetUniformLocation(Program, uniformName), quaterníon);
		}
		public void SetUniformVector4(string uniformName, Vector4 vector) {
			Enable();
			GL.Uniform4(GL.GetUniformLocation(Program, uniformName), vector);
		}

		public void SetUniformVector4i(string uniformName, int v1, int v2, int v3, int v4) {
			Enable();
			GL.Uniform4(GL.GetUniformLocation(Program, uniformName), v1, v2, v3, v4);
		}
		public void SetUniformVector4f(string uniformName, float v1, float v2, float v3, float v4) {
			Enable();
			GL.Uniform4(GL.GetUniformLocation(Program, uniformName), v1, v2, v3, v4);
		}
		public void SetUniformVector4d(string uniformName, double v1, double v2, double v3, double v4) {
			Enable();
			GL.Uniform4(GL.GetUniformLocation(Program, uniformName), v1, v2, v3, v4);
		}
		#endregion

		#region Matrix
		public void SetUniformMatrix2(string uniformName, Matrix2 matrix) {
			Enable();
			GL.UniformMatrix2(GL.GetUniformLocation(Program, uniformName), false, ref matrix);
		}
		public void SetUniformMatrix3(string uniformName, Matrix3 matrix) {
			Enable();
			GL.UniformMatrix3(GL.GetUniformLocation(Program, uniformName), false, ref matrix);
		}
		public void SetUniformMatrix4(string uniformName, Matrix4 matrix) {
			Enable();
			GL.UniformMatrix4(GL.GetUniformLocation(Program, uniformName), false, ref matrix);
		}
		#endregion

		public void Enable() {
			GL.UseProgram(Program);
		}

		public void Disable() {
			GL.UseProgram(0);
		}
	}
}
