shader_type spatial;

// Uniform is like an export field, which can be set from the editor.

// Colour and roughness
uniform vec4 primary_color: source_color;
uniform float roughness = 0.2;
uniform float alpha = 1.0;

// Normal map
uniform sampler2D normalmap_a : hint_normal;
uniform sampler2D normalmap_b : hint_normal;
uniform float normalmap_a_strength = 0.3;
uniform float normalmap_b_strength = 0.4;

// Movement
uniform vec2 movement_direction = vec2(0.1, 0.1);
uniform float movement_strength = 0.5;
uniform float uv_scale = 1.0;

// Calculates the strength of the colour based on the viewing angle.
float fresnel(float amount, vec3 normal, vec3 view)
{
	return pow((1.0 - clamp(dot(normalize(normal), normalize(view)), 0.0, 1.0)), amount);
}

// Also known as Pixer Shader
void fragment() {
	// Called for every pixel the material is visible on.
	
	float basic_fresnel = fresnel(3.0, NORMAL, VIEW);
	
	vec2 uv = UV * uv_scale;
	vec2 uv_movement = movement_direction * TIME * movement_strength;
	
	// Calculate a normal map for reflections.
	vec3 normalmap = texture(normalmap_a, uv + uv_movement).rgb * normalmap_a_strength;
	normalmap += texture(normalmap_b, uv - uv_movement).rgb * normalmap_b_strength;
	
	// The colour of the water. Basically the colour the surface reflects when light hits it.
	ALBEDO = primary_color.rgb + 0.5 * basic_fresnel;
	
	// How rough the surface is, meaning how well light reflects from it. 0 is like a mirror surface
	// and 1 equals to a completely matte surface (light bounces to every direction equally).
	ROUGHNESS = roughness * (1.0 - basic_fresnel);
	
	NORMAL_MAP = normalmap;
	
	ALPHA = alpha;
}

//void light() {
//	// Called for every pixel for every light affecting the material.
//	// Uncomment to replace the default light processing function with this one.
//}
