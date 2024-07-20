using System;
using Godot;

public class PhotoPiece : Area {
    [Export]
    public uint Quadrant { get; private set; }

    EventBus bus;

    public override void _Ready()
    {
        bus = GetNode<EventBus>("/root/EventBus");
    }

    void OnBodyEntered() {
        var enumLength = Enum.GetValues(typeof(PieceQuadrant)).Length;

        if(Quadrant >= enumLength) {
            throw new Exception($"Quadrant value not valid: {Quadrant}");
        }
        bus.EmitSignal(nameof(EventBus.PieceCollected), (PieceQuadrant)Quadrant);
        QueueFree();
    }
}

public enum PieceQuadrant {
    TopLeft,
    TopRight,
    BotLeft,
    BotRight
}