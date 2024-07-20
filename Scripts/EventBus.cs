using Godot;

public class EventBus : Node {

    public override void _Ready()
    {
        
    }

    [Signal]
    public delegate void LevelsMenuAppeared();

    [Signal]
    public delegate void MainMenuAppeared();

    [Signal]
    public delegate void LevelOneAchieved();

    [Signal]
    public delegate void PieceCollected();

    [Signal]
    public delegate void PortalOpen();
}
