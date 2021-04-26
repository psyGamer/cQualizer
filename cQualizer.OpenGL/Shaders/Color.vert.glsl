#version 330

layout(location = 0) in vec4 aPosition;

uniform mat4 uProjection;

void main() {
	gl_Position = uProjection * aPosition;
}