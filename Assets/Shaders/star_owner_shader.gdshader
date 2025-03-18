shader_type canvas_item;

uniform vec4 owner_color : source_color;  // Color for the ownership
uniform float radius : hint_range(0.1, 1.0) = 0.8;  // How far the effect extends
uniform float border_thickness : hint_range(0.01, 0.5) = 0.05;  // The solid border thickness

void fragment() {
    vec2 uv_centered = UV - vec2(0.5);  // Center the UVs
    float dist = length(uv_centered);  // Distance from center

    float alpha = smoothstep(radius, radius - border_thickness, dist);  // Smooth border
    alpha *= smoothstep(0.0, radius, dist);  // Fade the inner part

    COLOR = vec4(owner_color.rgb, alpha * owner_color.a);
}
