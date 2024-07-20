using Godot;
using System;

public class LevelsMenu : CanvasLayer
{
    SceneTree tree;
    EventBus bus;
    TextureButton backButton;

    public override void _Ready()
    {
        tree = GetTree();
        bus = GetNode<EventBus>("/root/EventBus");
        backButton = GetNode<TextureButton>("BackButton/TextureButton");
        backButton.Connect("button_up", this, nameof(OnBackButton_Click));
    }

    void OnBackButton_Click() {
        var mainMenu = (PackedScene)ResourceLoader.Load("res://scenes/MainMenu.tscn");
        var levelsInstance = mainMenu.Instance();
	    tree.Root.AddChild(levelsInstance);
        bus.EmitSignal(nameof(EventBus.MainMenuAppeared));
        QueueFree();
    }
}
