using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace cQualizer.OpenGL.Graphics {

	public static class ShaderLoader {

		private static readonly Dictionary<string, Shader> shaderCache = new Dictionary<string, Shader>();

		public static Shader LoadShader(string shaderName) {
			if (shaderCache.ContainsKey(shaderName))
				return shaderCache[shaderName];

			string vertexCode = GetShaderCode(shaderName, "vertex", "vert", "v"); 
			string fragmentCode = GetShaderCode(shaderName, "fragment", "frag", "f");

			return new Shader(vertexCode, fragmentCode);
		}

		private static string GetShaderCode(string shaderName, params string[] fileEndings) {
			Assembly assembly = Assembly.GetExecutingAssembly();
			Stream fragmentStream = null;

			foreach (var fileEnding in fileEndings) {
				try { fragmentStream = assembly.GetManifestResourceStream($"cQualizer.OpenGL.Shaders.{shaderName}.{fileEnding}.glsl"); break; } catch (FileNotFoundException) { }
			}

			if (fragmentStream is null) {
				Console.Error.WriteLine($"Could not find the {fileEndings[0]} shader: {shaderName}");

				return "";
			}

			return new StreamReader(fragmentStream).ReadToEnd();
		}
	}
}
