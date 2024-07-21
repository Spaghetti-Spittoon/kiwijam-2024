using Godot;
using System;

public class MainMenu : CanvasLayer
{
	SceneTree tree;
	EventBus bus;
	GenericButton levelsButton;
	GenericButton continueButton;
	Progress progressState;

	public override void _Ready()
	{
		tree = GetTree();
		bus = GetNode<EventBus>("/root/EventBus");
		levelsButton = GetNode<GenericButton>("Container/LevelsButton");
		continueButton = GetNode<GenericButton>("Container/ContinueButton");
		progressState = GetNode<Progress>("/root/Progress");

		levelsButton.TexturePath = "res://Assets/levels button.png";
		levelsButton.PressedTexturePath = "res://Assets/levels button dark.png";
		continueButton.TexturePath = "res://Assets/continue button.png";
		continueButton.PressedTexturePath = "res://Assets/continue button dark.png";

		levelsButton.Connect(nameof(GenericButton.ButtonReleased), this, nameof(OnLevelsButton_Click));
		continueButton.Connect(nameof(GenericButton.ButtonReleased), this, nameof(OnContinueButton_Click));
	}

	void OnLevelsButton_Click() {
		//hide buttons to prevent duplicate scenes
		levelsButton.Visible = false;
		continueButton.Visible = false;

		var levelsMenu = (PackedScene)ResourceLoader.Load("res://Scenes/Ui/LevelsMenu.tscn");
		var levelsInstance = levelsMenu.Instance();

		//add levels scene
		AddChild(levelsInstance);
		bus.EmitSignal(nameof(EventBus.LevelsMenuAppeared));

		//remove the main menu scene
		QueueFree();
	}

	void OnContinueButton_Click() {
		int achievedLevel = progressState.LevelAchieved;
		var scenePath = $"res://Scenes/Levels/Level{achievedLevel}.tscn";
		Node level = ((PackedScene)ResourceLoader.Load(scenePath)).Instance();
		tree.Root.AddChild(level);
		GetParent().QueueFree();
	}
}
