using Godot;

public class MovingObject : KinematicBody
{

    public override void _Ready()
    {
        var originalPosition = Position;

        var tween = GetNode<Tween>("Tween");
        tween.InterpolateProperty(this, "position", originalPosition, Position + new Vector3(10, 0, 0), 3f, easeType: Tween.EaseType.InOut);
        tween.InterpolateProperty(this, "position", Position, originalPosition, 3f, easeType: Tween.EaseType.InOut);
        tween.Start();
    }

    public override void _PhysicsProcess(float delta)
    {

    }
}
