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
        levelsButton = GetNode<GenericButton>("LevelsButton");
        continueButton = GetNode<GenericButton>("ContinueButton");
        progressState = GetNode<Progress>("/root/Progress");
        levelsButton.Connect(nameof(GenericButton.ButtonReleased), this, nameof(OnLevelsButton_Click));
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

    void OnContinueButton_Click() {
        int achievedLevel = progressState.LevelAchieved;
        var scenePath = $"res://scenes/levels/Level{achievedLevel}.tscn";
        Node level = ((PackedScene)ResourceLoader.Load(scenePath)).Instance();
        tree.Root.AddChild(level);
        QueueFree();
    }
}
