shader_type canvas_item;

void fragment() {
	COLOR = vec4(0.55, 1, 1, 0.5);
}
void vertex() {
  // Animate Sprite2D moving in big circle around its location
  VERTEX += vec2(-100.0, 100.0);
  VERTEX.y *= 0.2;
  VERTEX.x *= 2.5;
}