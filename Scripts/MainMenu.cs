using Godot;
using System;

public class MainMenu : CanvasLayer
{
	SceneTree tree;
	EventBus bus;
	TextureButton levelsButton;

	public override void _Ready()
	{
		tree = GetTree();
		var toplevelnodes = tree.Root.GetChildren();
		bus = GetNode<EventBus>("/root/EventBus");

		levelsButton = GetNode<TextureButton>("LevelsButton/TextureButton");
		levelsButton.Connect("button_up", this, nameof(OnLevelsButton_Click));
	}

	void OnLevelsButton_Click() {
		var levelsMenu = (PackedScene)ResourceLoader.Load("res://scenes/LevelsMenu.tscn");
		var levelsInstance = levelsMenu.Instance();

		//add levels scene
		tree.Root.AddChild(levelsInstance);
		bus.EmitSignal(nameof(EventBus.LevelsMenuAppeared));

		//remove the main menu scene
		QueueFree();
	}    
}
