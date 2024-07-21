using System;
using Godot;

public class PhotoPiece : Area {
    [Export]
    public uint Quadrant { get; private set; }

    EventBus bus;

    public override void _Ready()
    {
        bus = GetNode<EventBus>("/root/EventBus");
        Connect("body_entered", this, nameof(OnBodyEntered));
    }

    void OnBodyEntered(Node body) {
        var enumLength = Enum.GetValues(typeof(PieceQuadrant)).Length;

        if(Quadrant >= enumLength) {
            throw new Exception($"Quadrant value not valid: {Quadrant}");
        }
        bus.EmitSignal(nameof(EventBus.PieceCollected), (PieceQuadrant)Quadrant);
        GD.Print("OnBodyEntered");
        QueueFree();
    }

    public override void _PhysicsProcess(float delta)
    {
        var newTransform = Transform;
        newTransform.basis = newTransform.basis.Rotated(Vector3.Up, 0.01f);
        Transform = newTransform;
    }
}

public enum PieceQuadrant {
    TopLeft,
    TopRight,
    BotLeft,
    BotRight
}