using Godot;

public class Hud : Node {
    [Export]
    public int TotalPieces { get; private set; }

    int piecesCollected;

    EventBus bus;

    public override void _Ready()
    {
        bus = GetNode<EventBus>("/root/EventBus");
        bus.Connect(nameof(EventBus.PieceCollected), this, nameof(OnPieceCollected));
    }

    void OnPieceCollected() {
        if(piecesCollected < TotalPieces) {
            piecesCollected++;
        }

        if(piecesCollected >= TotalPieces) {
            bus.EmitSignal(nameof(EventBus.PortalOpen));
        }
    }
}
