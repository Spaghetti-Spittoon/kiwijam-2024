using Godot;
using System;

public class Level1Up : StaticBody
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	private void OnCollision(object body)
	{
		GD.Print("TestCollision");
		if (body is KinematicBody)
		{
			GD.Print("OnCollision_With_KinemeticBody");
			// GetTree().ReloadCurrentScene();
			GetTree().ChangeScene("res://scenes/MainMenu.tscn");
		}
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      
	//  }
}

