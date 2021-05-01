#version 330

layout(location = 0) in vec2 aPosition;

uniform mat4 uProjection;

void main() {
	gl_Position = uProjection * vec4(aPosition, 0, 1);
}