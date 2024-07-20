using Godot;

public class EventBus : Node {

    [Signal]
    public delegate void LevelsMenuAppeared();

    [Signal]
    public delegate void MainMenuAppeared();

    [Signal]
    public delegate void LevelOneAchieved();

    [Signal]
    public delegate void PieceCollected(PieceQuadrant quadrant);

    [Signal]
    public delegate void PortalOpen();
}
