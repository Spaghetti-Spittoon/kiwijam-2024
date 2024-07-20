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

    [Signal]
    public delegate void GridButtonReleased(int gridIndex);

    [Signal]
    public delegate void LevelOneAchieved();
}
