using Godot;
using System;

public class LevelsMenu : CanvasLayer
{
	SceneTree tree;
	EventBus bus;
	GenericButton backButton;
	GridContainer grid;
	Progress progressState;

	public override void _Ready()
	{
		tree = GetTree();
		bus = GetNode<EventBus>("/root/EventBus");
		backButton = GetNode<GenericButton>("BackButton");
		grid = GetNode<GridContainer>("GridContainer");
		progressState = GetNode<Progress>("/root/Progress");

		backButton.TexturePath = "res://Assets/back button.png";
		backButton.PressedTexturePath = "res://Assets/back button dark.png";
		backButton.Connect(nameof(GenericButton.ButtonReleased), this, nameof(OnBackButton_Click));
		AddLevelButtons();
	}

	void OnBackButton_Click() {
		//hide buttons to prevent duplicate scenes
		backButton.Visible = false;

		var mainMenu = (PackedScene)ResourceLoader.Load("res://Scenes/Ui/MainMenu.tscn");
		var levelsInstance = mainMenu.Instance();
		tree.Root.AddChild(levelsInstance);

		bus.EmitSignal(nameof(EventBus.MainMenuAppeared));
		QueueFree();
	}

	void AddLevelButtons() {
		var buttonScene = (PackedScene)ResourceLoader.Load("res://Scenes/Ui/GridButton.tscn");

		for(int i = 1; i <= progressState.LevelAchieved; i++) {
			GridButton buttonInstance = buttonScene.Instance<GridButton>();
			buttonInstance.GridIndex = i;
			buttonInstance.TexturePath = $"res://Assets/level{i}.png";
			buttonInstance.PressedTexturePath = $"res://Assets/level{i} dark.png";
			buttonInstance.Connect(nameof(GridButton.GridButtonReleased), this, nameof(OnLevel_Click));
			grid.AddChild(buttonInstance);
			buttonInstance.ScaleButton(new Vector2{x = 0.3f, y = 0.3f});
		}
	}

	void OnLevel_Click(int gridIndex) {
		string scenePath;

		switch(gridIndex) {
			case 1:
				scenePath = "res://Scenes/Levels/Level1.tscn";
				break;

			default:
				throw new Exception("ERROR: level 0 not valid");
		}
		//hide buttons to prevent duplicate scenes
		backButton.Visible = false;
		var gridButtons = grid.GetChildren();
		
		for(int i = 0; i < gridButtons.Count; i++) {
			var currentButton = (GridButton)gridButtons[i];
			currentButton.Visible = false;
		}
		Node level = ((PackedScene)ResourceLoader.Load(scenePath)).Instance();
		GotoLevel(level);
	}

	void GotoLevel(Node level) {
		tree.Root.AddChild(level);
		GetParent().QueueFree();
	}
}
