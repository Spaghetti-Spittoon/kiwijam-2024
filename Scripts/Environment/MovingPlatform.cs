using Godot;

public class MovingPlatform : KinematicBody
{
    private Vector3 endPoint;
    private Vector3 startPoint;

    private Vector3 _velocity;
    private float _speed = 1f;

    private bool flip;
    private bool moving;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        startPoint = GlobalPosition;
        endPoint = GlobalPosition + new Vector3(100, 0, 0);
    }

    public override void _PhysicsProcess(float delta)
    {
        var movementDirection = Vector3.Zero;
        moving = false;

        if (!flip)
        {
            if (GlobalPosition < endPoint)
            {
                movementDirection.x = 1;
                moving = true;
            }
            else
            {
                flip = true;
            }
        }
        else
        {
            if (GlobalPosition > startPoint)
            {
                movementDirection.x = -1;
                moving = true;
            }
            else
            {
                flip = false;
            }
        }

        _velocity.x = movementDirection.x * _speed * delta;

        MoveAndCollide(_velocity);


    }
}
