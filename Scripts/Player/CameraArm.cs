using Godot;

public class CameraArm : SpringArm
{
    [Export]
    private float _mouseSensitivity = 0.05f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SetAsToplevel(true);
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion)
        {
            var inputEvent = @event as InputEventMouseMotion;

            var rotationX = RotationDegrees.x - (inputEvent.Relative.y * _mouseSensitivity);
            rotationX = Mathf.Clamp(rotationX, -90, 30);

            var rotationY = RotationDegrees.y - (inputEvent.Relative.x * _mouseSensitivity);
            rotationY = Mathf.Wrap(rotationY, 0, 360);

            RotationDegrees = new Vector3(rotationX, rotationY, RotationDegrees.z);

        }
    }
}
