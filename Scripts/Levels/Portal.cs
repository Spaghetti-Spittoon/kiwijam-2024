using Godot;
using System;

public class Portal : StaticBody
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	public PackedScene scn;

	private Area top_left;
	
	private Area top_right;
	private Area bot_left;
	private Area bot_right;
	private KinematicBody player; 

	private bool got_top_left, got_top_right, got_bot_left, got_bot_right = false;
	private Vector3 Distance;
	EventBus bus;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print(this.Transform.origin);
		top_left = this.GetNode<Area>("top_left");
		top_right = this.GetNode<Area>("top_right");
		bot_left = this.GetNode<Area>("bot_left");
		bot_right = this.GetNode<Area>("bot_right");
		player = GetNode<KinematicBody>("../Player");

		top_left.Scale = (new Vector3(0, 0, 0));
		top_right.Scale = (new Vector3(0, 0, 0));
		bot_left.Scale = (new Vector3(0, 0, 0));
		bot_right.Scale = (new Vector3(0, 0, 0));

		bus = GetNode<EventBus>("/root/EventBus");
		bus.Connect(nameof(EventBus.PieceCollected), this, nameof(OnPieceCollected));
	}


	public void OnPieceCollected(PieceQuadrant a)
	{
		GD.Print("OnPieceCollected" + (int)a);
		if (a == 0)
		{
			got_top_left = true;
		}
		else if ((int)a == 1)
		{
			got_top_right = true;
		}
		else if ((int)a == 2)
		{
			got_bot_left = true;
		}
		else if ((int)a == 3)
		{
			got_bot_right = true;
		}
	}


 // Called every frame. 'delta' is the elapsed time since the previous frame.
 public override void _Process(float delta)
 {
	Distance = player.Transform.origin - this.Transform.origin;	
	Distance = Distance.Abs();
	if (Distance.Length() < 3)
	{
		if (got_top_left)
		{
			top_left.Scale = (new Vector3(0.1f, 0.1f, 0.1f));
		}
		if (got_top_right)
		{
			top_right.Scale = (new Vector3(0.1f, 0.1f, 0.1f));
		}
		if (got_bot_left)
		{
			bot_left.Scale = (new Vector3(0.1f, 0.1f, 0.1f));
		}
		if (got_bot_right)
		{
			bot_right.Scale = (new Vector3(0.1f, 0.1f, 0.1f));
		}

		if (got_top_left && got_top_right && got_bot_left && got_bot_right)
		{
			GD.Print("All pieces collected");
			Node scn = ResourceLoader.Load<PackedScene>("res://levels/level2.tscn").Instance();
			GetTree().Root.AddChild(scn);
		}
	}
 }
}
