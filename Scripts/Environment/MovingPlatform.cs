using Godot;
using System;

public class MovingPlatform : KinematicBody
{
    private Vector3 endPoint;
    private Vector3 startPoint;

    private Vector3 _velocity;
    [Export]
    public float _speed = 1f;

    private bool flip;
    private bool moving;
    [Export]
    public float k_acceleration = 0.05f;
    [Export]
    public float acceleration_window = 4;
    private float acceleration_window_div;
    private float time_total = 0;



    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        acceleration_window_div = acceleration_window / 2;
        startPoint = GlobalPosition;
        _velocity = new Vector3(0, 0, 0);
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

        time_total += delta;
        updateSpeed(time_total % acceleration_window);

        var collision = MoveAndCollide(_velocity * delta, infiniteInertia: true);
        if (collision != null)
        {

            var player = collision.Collider as Player;
            player.MoveAndCollide(collision.Remainder);
            player.ForceUpdateTransform();
            this.MoveAndCollide(collision.Remainder);
        }

    }



    public void updateSpeed(float time_window)
    {

        float x = (time_window % acceleration_window_div) / acceleration_window_div;

        if (time_window <= acceleration_window_div)
        {
            if (x < 0.5)
            {
                _velocity.x = _speed * 12 * x * x;
            }
            else
            {
                _velocity.x = _speed * 3 * (float)Math.Pow(2 * x - 2, 2);
            }
        }
        else
        {
            if (x < 0.5)
            {
                _velocity.x = _speed * -12 * x * x;
            }
            else
            {
                _velocity.x = _speed * -3 * (float)Math.Pow(2 * x - 2, 2);
            }
        }

    }
}
