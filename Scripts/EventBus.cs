using Godot;

public class EventBus : Node {

    public override void _Ready()
    {
        GD.Print("");    
    }

    [Signal]
    public delegate void LevelsMenuAppeared();

    [Signal]
    public delegate void MainMenuAppeared();
}