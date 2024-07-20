
using Godot;

public class Hud : CanvasLayer {
    [Export]
    public int TotalPieces { get; private set; }

    int piecesCollected;

    bool topLeftCollected;
    bool topRightCollected;
    bool botLeftCollected;
    bool botRightCollected;

    TextureRect topLeftPiece;
    TextureRect topRightPiece;
    TextureRect botLeftPiece;
    TextureRect botRightPiece;

    EventBus bus;
    Label collectionCounter;

    public override void _Ready()
    {
        bus = GetNode<EventBus>("/root/EventBus");
        topLeftPiece = GetNode<TextureRect>("PiecesContainer/TextureRect");
        topRightPiece = GetNode<TextureRect>("PiecesContainer/TextureRect2");
        botLeftPiece = GetNode<TextureRect>("PiecesContainer/TextureRect3");
        botRightPiece = GetNode<TextureRect>("PiecesContainer/TextureRect4");
        collectionCounter = GetNode<Label>("Label");

        bus.Connect(nameof(EventBus.PieceCollected), this, nameof(OnPieceCollected));
        topLeftPiece.Visible = false;
        topRightPiece.Visible = false;
        botLeftPiece.Visible = false;
        botRightPiece.Visible = false;
    }

    void OnPieceCollected(PieceQuadrant quadrant) {
        GD.Print("OnPieceCollected");

        switch(quadrant) {
            case PieceQuadrant.TopLeft:
                topLeftPiece.Visible = true;
                IncrementCollection(ref topLeftCollected);
                break;

            case PieceQuadrant.TopRight:
                topRightPiece.Visible = true;
                IncrementCollection(ref topRightCollected);
                break;

            case PieceQuadrant.BotLeft:
                botLeftPiece.Visible = true;
                IncrementCollection(ref botLeftCollected);
                break;

            case PieceQuadrant.BotRight:
                botRightPiece.Visible = true;
                IncrementCollection(ref botRightCollected);
                break;
        }

        if(piecesCollected >= TotalPieces) {
            bus.EmitSignal(nameof(EventBus.PortalOpen));
            GD.Print("Level Complete!");
        }
    }

    void IncrementCollection(ref bool state) {
        if(state) {
            return;
        }
        piecesCollected++;
        state = true;
        collectionCounter.Text = $"{piecesCollected}/4";
    }
}
