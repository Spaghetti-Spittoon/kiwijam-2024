using Godot;
using Godot.Collections;

public class PathingPlatform : KinematicBody
{
    [Export]
    private Array<Vector3> TargetPoints;

    [Export]
    private PathingPlatformMode Mode;

    [Export]
    private float _Speed = 8f;

    [Export]
    private float _TimeBetweenPoints = 0f;

    private Vector3 endPoint;
    private Vector3 startPoint;

    private Vector3 velocity;

    private bool _flip;
    private bool _moving;

    private int currentPointIndex = 0;

    private Vector3 initialPosition;

    private Array<Vector3> _targetPoints;
    private bool moveToNextPoint = true;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        initialPosition = GlobalPosition;

        _targetPoints = TargetPoints;

        CyclePoints();
    }

    public override void _PhysicsProcess(float delta)
    {
        if (!moveToNextPoint) return;

        var movementDirection = Vector3.Zero;

        var direction = GlobalPosition.DirectionTo(endPoint);

        velocity = direction * _Speed * delta;

        var collision = MoveAndCollide(velocity, infiniteInertia: true);

        if (GlobalPosition.DistanceTo(endPoint) < 0.1)
        {
            CyclePoints();

            if (_TimeBetweenPoints > 0)
            {
                moveToNextPoint = false;

                var timer = GetTree().CreateTimer(_TimeBetweenPoints);
                timer.Connect("timeout", this, nameof(ContinueToNextPoint));
            }
        }

        if (collision != null)
        {
            var collider = collision.Collider as Player;

            collider.MoveAndCollide(collision.Remainder);
            collider.ForceUpdateTransform();

            var additionalVelocity = Vector3.Zero;
            additionalVelocity.y = direction.y * _Speed * delta * 3f;

            if (_Speed < 6f)
            {
                additionalVelocity.y = direction.y * _Speed * delta * 3f;
            }

            MoveAndCollide(collision.Remainder + additionalVelocity, infiniteInertia: true);
            ForceUpdateTransform();
        }
    }

    private void CyclePoints()
    {
        if (currentPointIndex + 1 == TargetPoints.Count)
        {
            if (Mode == PathingPlatformMode.Boomerang)
            {
                var temp = new Array<Vector3>();

                for (int i = _targetPoints.Count - 1; i >= 0; i--)
                {
                    temp.Add(_targetPoints[i]);
                }

                _targetPoints = temp;

                currentPointIndex = 0;

                startPoint = initialPosition + _targetPoints[currentPointIndex];
                endPoint = initialPosition + _targetPoints[currentPointIndex + 1];

                currentPointIndex++;
            }
            else
            {
                startPoint = initialPosition + _targetPoints[currentPointIndex];
                endPoint = initialPosition + _targetPoints[0];

                currentPointIndex = 0;
            }
        }
        else
        {
            startPoint = initialPosition + _targetPoints[currentPointIndex];
            endPoint = initialPosition + _targetPoints[currentPointIndex + 1];

            currentPointIndex++;
        }
    }

    private void ContinueToNextPoint() => moveToNextPoint = true;
}

public enum PathingPlatformMode
{
    Boomerang,
    Continuous,
}