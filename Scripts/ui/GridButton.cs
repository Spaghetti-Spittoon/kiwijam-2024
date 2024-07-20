using Godot;
using System;

public class GridButton : Control
{
    [Export]
    public int GridIndex;

    EventBus bus;
    TextureButton button;

    public override void _Ready()
    {
        button = GetNode<TextureButton>("TextureButton");
        button.Connect("button_up", this, nameof(OnButton_Click));
        bus = GetNode<EventBus>("/root/EventBus");
    }

    void OnButton_Click() {
        bus.EmitSignal(nameof(EventBus.GridButtonReleased), GridIndex);
    }
}
