using Godot;
using System;

public class GenericButton : Node
{
    [Export]
    public string ButtonText;

    public override void _Ready()
    {
        Label buttonsLabel = GetNode<Label>("CenterContainer/Label");
        buttonsLabel.Text = ButtonText;
    }
}
