using Godot;
using System;

public class Rotation : StaticBody
{
	// Declare member variables here. Examples:
	// private int a = 2;
	private float time_total = 0;
	public bool cubic_rotate = false;
	[Export]
	public bool cubic_linear_cyclic = true; 
	[Export]
	public float cubic_linear_magnitude_x = 1;
	[Export]
	public float cubic_linear_magnitude_y = 2;
	[Export]
	public float cubic_linear_magnitude_z = 0;
	[Export]
	public float in_between_delay_time_seconds = 0;
	[Export]
	public float half_cycle_time_loop_seconds = 2;
	// private string b = "text";
	private bool flip_flop = false;

	private Transform original_transform;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		original_transform = this.GlobalTransform;
		
		
		
		

	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{

		time_total += delta;

		if (flip_flop)
		{
			this.GlobalTransform = new Transform(this.GlobalTransform.basis
										 ,new Vector3(
										  (cubic_linear_magnitude_x *((float)easeInCubic((time_total % half_cycle_time_loop_seconds) / half_cycle_time_loop_seconds)) + original_transform.origin.x)
										 ,(cubic_linear_magnitude_y *((float)easeInCubic((time_total % half_cycle_time_loop_seconds) / half_cycle_time_loop_seconds)) + original_transform.origin.y)
										 ,(cubic_linear_magnitude_z *((float)easeInCubic((time_total % half_cycle_time_loop_seconds) / half_cycle_time_loop_seconds)) + original_transform.origin.z)
										 )
										 );
			// this.Transform = new Transform(original_basis
			// 							 ,new Vector3(
			// 							  cubic_linear_magnitude_x *((float)easeInCubic((time_total % half_cycle_time_loop_seconds) / half_cycle_time_loop_seconds))
			// 							 ,cubic_linear_magnitude_y *((float)easeInCubic((time_total % half_cycle_time_loop_seconds) / half_cycle_time_loop_seconds))
			// 							 ,cubic_linear_magnitude_z *((float)easeInCubic((time_total % half_cycle_time_loop_seconds) / half_cycle_time_loop_seconds))
			// 							 )
			// 							 );
		}
		else
		{
			this.GlobalTransform = new Transform(this.GlobalTransform.basis
										 ,new Vector3(
										  (cubic_linear_magnitude_x *((float)easeOutCubic((time_total % half_cycle_time_loop_seconds) / half_cycle_time_loop_seconds)) + original_transform.origin.x)
										 ,(cubic_linear_magnitude_y *((float)easeOutCubic((time_total % half_cycle_time_loop_seconds) / half_cycle_time_loop_seconds)) + original_transform.origin.y)
										 ,(cubic_linear_magnitude_z *((float)easeOutCubic((time_total % half_cycle_time_loop_seconds) / half_cycle_time_loop_seconds)) + original_transform.origin.z)
										 )
										 );
		}
		// this.Transform. = Vector3(0,((float)easeInOutCubic((time_total % 2) / 2)),0);

	}



	// 0 - 1 Easing in
	public double easeOutCubic(double y)
	{
		double x = 1 - y;
		double return_value = x < 0.5 ? 4 * x * x * x : 1 - Math.Pow(-2 * x + 2, 3) / 2;
		if (y >= .95) flip_flop = true;
		return return_value;
	}
	public double easeInCubic(double x)
	{

		double return_value = x < 0.5 ? 4 * x * x * x : 1 - Math.Pow(-2 * x + 2, 3) / 2;
		if (x >= .95) flip_flop = false;
		return return_value;
	}
}
