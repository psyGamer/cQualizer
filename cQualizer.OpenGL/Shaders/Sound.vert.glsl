#version 330

layout(location = 0) in vec2 aPosition;
layout(location = 1) in float aRadius;

uniform mat4 uProjection;

out vec4 vGlPos;
out vec2 vPosition;
out float vRadius;
out float vError;

void main() {
	float posX = aPosition.x * aRadius;
	float posY = aPosition.y * aRadius;

	vPosition = aPosition;
	vRadius = aRadius;

	gl_Position = uProjection * vec4(posX, posY, 0.0, 1.0);
}