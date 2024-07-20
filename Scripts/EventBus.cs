using Godot;

public class EventBus : Node {

    public override void _Ready()
    {
        GD.Print("");    
    }

    [Signal]
    public delegate void LevelOneSet();
}
