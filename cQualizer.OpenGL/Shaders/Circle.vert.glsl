#version 330

layout(location = 0) in float aAngle;

uniform float uRadius;

void main() {
	float rad = radians(aAngle);
	float xPos = cos(rad) * uRadius;
	float yPos = sin(rad) * uRadius;

	gl_Position = vec4(xPos, yPos, 0.0, 1.0);
}