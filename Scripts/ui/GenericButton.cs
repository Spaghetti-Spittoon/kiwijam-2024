using Godot;
using System;

public class GenericButton : Node
{
    [Export]
    public string ButtonText;

    [Signal]
    public delegate void ButtonReleased();

    TextureButton button;

    public override void _Ready()
    {
        Label buttonsLabel = GetNode<Label>("CenterContainer/Label");
        button = GetNode<TextureButton>("CenterContainer/TextureButton");
        buttonsLabel.Text = ButtonText;
        button.Connect("button_up", this, nameof(OnButton_Click));
    }

    void OnButton_Click() {
        EmitSignal(nameof(ButtonReleased));
    }
}
