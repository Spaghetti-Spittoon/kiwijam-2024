using Godot;
using System;

public class MainMenu : CanvasLayer
{
    EventBus bus;
    TextureButton levelsButton;

    public override void _Ready()
    {
        var toplevelnodes = GetTree().Root.GetChildren();
        bus = GetNode<EventBus>("/root/EventBus");

        levelsButton = GetNode<TextureButton>("LevelsButton/TextureButton");
        levelsButton.Connect("button_up", this, nameof(OnLevelsButton_Click));
    }

    void OnLevelsButton_Click() {
        bus.EmitSignal(nameof(EventBus.LevelOneSet));
    }    
}
