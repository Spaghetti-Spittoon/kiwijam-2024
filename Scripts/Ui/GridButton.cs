using Godot;
using System;

public class GridButton : GenericButton
{
    [Export]
    public int GridIndex;

    [Signal]
    public delegate void GridButtonReleased(int gridIndex);

    public override void _Ready()
    {
        base._Ready();
    }

    public void ScaleButton(Vector2 scale) {
        Button.RectScale = scale;
    }

    protected override void OnButton_Click() {
        EmitSignal(nameof(GridButtonReleased), GridIndex);
        base.OnButton_Click();
    }
}
