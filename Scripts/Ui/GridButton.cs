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
        button = GetNode<TextureButton>("TextureButton");
        button.Connect("button_up", this, nameof(OnButton_Click));
        base._Ready();
    }

    public void ScaleButton(Vector2 scale) {
        button.RectScale = scale;
    }

    void OnButton_Click() {
        EmitSignal(nameof(GridButtonReleased), GridIndex);
    }
}