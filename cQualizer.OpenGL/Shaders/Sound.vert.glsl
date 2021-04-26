#version 330

layout(location = 0) in vec2 aPosition;
layout(location = 1) in float aRadius;

void main() {
	float posX = aPosition.x * aRadius;
	float posY = aPosition.y * aRadius;

	gl_Position = vec4(posX, posY, 0.0, 1.0);
}