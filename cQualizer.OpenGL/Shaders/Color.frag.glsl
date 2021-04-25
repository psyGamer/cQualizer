#version 330

layout(location = 0) out vec4 fColor;

uniform vec4 uColor;

void main() {
	fColor = uColor;
}