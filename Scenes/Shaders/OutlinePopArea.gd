extends Area

var animated_outline: AnimatedOutline


func _ready() -> void:
	animated_outline = AnimatedOutline.new(
		get_parent().get_node("Tween"), get_parent().get_surface_material(0).next_pass, 0.5, 0.2
	)
	animated_outline.pop_in()
