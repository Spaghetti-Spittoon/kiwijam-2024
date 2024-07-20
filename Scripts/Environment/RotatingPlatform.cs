using Godot;

public class RotatingPlatform : StaticBody
{
    [Export]
    private float _rotationSpeed = 10f;

    [Export]
    private float _alternatingSpeed = 2f;

    private bool rotateHorizontal = false;
    private bool isRotating = true;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    public override void _PhysicsProcess(float delta)
    {
        if (!isRotating) return;

        if (!rotateHorizontal)
        {
            var currentRotation = RotationDegrees;

            currentRotation.z += _rotationSpeed * delta;

            if (currentRotation.z > 90)
            {
                isRotating = false;
                currentRotation.z = 90;

                var timer = GetTree().CreateTimer(_alternatingSpeed);
                timer.Connect("timeout", this, nameof(SwitchDirection));
            }

            RotationDegrees = currentRotation;
        }
        else
        {
            var currentRotation = RotationDegrees;

            currentRotation.z -= _rotationSpeed * delta;

            GD.Print(currentRotation.z);

            if (currentRotation.z < 0)
            {
                isRotating = false;
                currentRotation.z = 0;
                var timer = GetTree().CreateTimer(_alternatingSpeed);
                timer.Connect("timeout", this, nameof(SwitchDirection));
            }

            RotationDegrees = currentRotation;
        }
    }

    private void SwitchDirection()
    {
        rotateHorizontal = !rotateHorizontal;
        isRotating = true;
    }
}
