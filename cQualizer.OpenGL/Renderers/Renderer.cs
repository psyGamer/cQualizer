using cQualizer.OpenGL.Components.Graphics;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace cQualizer.OpenGL.Renderers {

	public abstract class IRenderer {
		public abstract Shader Shader { get; protected set; }

		public abstract void Render(ApplicationWindow window, Vector2 aspectRatio);
		public abstract void Update(ApplicationWindow window, Vector2 aspectRatio);
	}

	public static class RendererRegistry {

		private static readonly List<IRenderer> renderers = new List<IRenderer>();

		public static void RegisterRenderer(IRenderer renderer) {
			renderers.Add(renderer);
		}

		public static void Update(ApplicationWindow window, Vector2 aspectRatio) {
			renderers.ForEach(renderer => renderer.Update(window, aspectRatio));
		}

		public static void Render(ApplicationWindow window, Vector2 aspectRatio) {
			renderers.ForEach(renderer => {
				renderer.Shader.SetUniformMatrix4("uProjection", Matrix4.CreateOrthographic(aspectRatio.X * 2, aspectRatio.Y * 2, -1.0f, 1.0f));
				renderer.Render(window, aspectRatio);
			});
		}
	}
}
