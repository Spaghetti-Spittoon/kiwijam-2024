using Godot;
using System;

public class LevelsMenu : CanvasLayer
{
    SceneTree tree;
    EventBus bus;
    TextureButton backButton;
    GridContainer grid;

    public override void _Ready()
    {
        tree = GetTree();
        bus = GetNode<EventBus>("/root/EventBus");
        backButton = GetNode<TextureButton>("BackButton/TextureButton");
        grid = GetNode<GridContainer>("GridContainer");
        backButton.Connect("button_up", this, nameof(OnBackButton_Click));
        AddLevelButtons();
    }

    void OnBackButton_Click() {
        var mainMenu = (PackedScene)ResourceLoader.Load("res://scenes/MainMenu.tscn");
        var levelsInstance = mainMenu.Instance();
	    tree.Root.AddChild(levelsInstance);
        bus.EmitSignal(nameof(EventBus.MainMenuAppeared));
        QueueFree();
    }

    void AddLevelButtons() {
        grid.AddChild();
    }

    void OnLevel_Click(int gridIndex) {

    }
}
