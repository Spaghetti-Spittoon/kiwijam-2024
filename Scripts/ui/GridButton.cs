using Godot;
using System;

public class GridButton : GenericButton
{
    [Export]
    public int GridIndex;

    [Signal]
    public delegate void GridButtonReleased(int gridIndex);

    TextureButton button;

    public override void _Ready()
    {
        button = GetNode<TextureButton>("CenterContainer/TextureButton");
        button.Connect("button_up", this, nameof(OnButton_Click));
    }

    void OnButton_Click() {
        EmitSignal(nameof(GridButtonReleased), GridIndex);
    }
}
