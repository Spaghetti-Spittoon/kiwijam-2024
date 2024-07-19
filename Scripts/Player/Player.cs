using Godot;

public class Player : KinematicBody
{
    private float _speed = 7f;
    private float _jump_strength = 20f;
    private float _gravity = 50f;

    private Vector3 velocity;
    private Vector3 snapVector;

    [Export]
    private SpringArm _cameraArm;

    [Export]
    private Spatial _model;

    private RayCast RCUpper1;
    private RayCast RCUpper2;
    private RayCast RCBody;

    private bool _isOnLedge;
    private bool _isClimbingLedge;

    private RayCast _RayCastWall;
    private RayCast _RayCastLedge;
    private Spatial _RayCastLedgeHolder;
    private MeshInstance _RayCastLedgeMarker;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _cameraArm = GetNode<SpringArm>("SpringArm");
        _model = GetNode<Spatial>("Model");

        RCUpper1 = _model.GetNode<RayCast>(nameof(RCUpper1));
        RCUpper2 = _model.GetNode<RayCast>(nameof(RCUpper2));
        RCBody = _model.GetNode<RayCast>(nameof(RCBody));


        //_RayCastWall = GetNode<RayCast>("RayCastWall");
        //_RayCastLedgeHolder = GetNode<Spatial>("RayCastLedgeHolder");

        //_RayCastLedge = _RayCastLedgeHolder.GetNode<RayCast>("RayCastLedge");
        //_RayCastLedgeMarker = _RayCastLedge.GetNode<MeshInstance>("RayCastLedgeMarker");
    }

    public override void _Process(float delta)
    {
        _cameraArm.Translation = Translation;
    }

    public override void _PhysicsProcess(float delta)
    {
        var canMove = true;

        var moveDirection = Vector3.Zero;

        moveDirection.x = Input.GetActionStrength("right") - Input.GetActionStrength("left");
        moveDirection = moveDirection.Rotated(Vector3.Up, _cameraArm.Rotation.y).Normalized();

        RCUpper1.CastTo = new Vector3(0, 0, moveDirection.x);
        RCUpper2.CastTo = new Vector3(0, 0, moveDirection.x);
        RCBody.CastTo = new Vector3(0, 0, moveDirection.x);

        velocity.x = moveDirection.x * _speed;
        velocity.z = moveDirection.z * _speed;
        velocity.y -= _gravity * delta;

        var justLanded = IsOnFloor() && snapVector == Vector3.Zero;
        var isJumpingInAir = !IsOnFloor() && Input.GetActionStrength("forward") > 0;
        var isJumpingOnFloor = IsOnFloor() && Input.IsActionJustPressed("forward");

        if (isJumpingOnFloor)
        {
            velocity.y = _jump_strength;
            snapVector = Vector3.Zero;
        }
        else if (isJumpingInAir)
        {
            DetectLedge();
        }
        else if (justLanded)
        {
            snapVector = Vector3.Down;
        }
        else if (IsOnFloor())
        {
            velocity.y = 0;
        }

        if (_isClimbingLedge)
        {
            return;
        }

        MoveAndSlideWithSnap(velocity, snapVector, Vector3.Up, true);

        if (velocity.Length() > 0.2f)
        {
            var lookDirection = new Vector2(velocity.z, velocity.x);

            _model.Rotation = new Vector3(_model.Rotation.x, lookDirection.Angle(), _model.Rotation.z);
        }
    }

    private void DetectLedge()
    {
        //TODO: See if we want to have ledge climbing
        return;

        if (RCUpper2.IsColliding() && !RCUpper1.IsColliding())
        {
            if (!_isOnLedge)
            {
                var timer = GetTree().CreateTimer(1);
                timer.Connect("timeout", this, nameof(DoClimb));
            }

            _isOnLedge = true;
            RCBody.Enabled = true;
            //_model.SetAsToplevel(true);
        }

        if (_isOnLedge)
        {
            velocity.y = 0;
            _isClimbingLedge = true;
        }
    }

    public void DoClimb()
    {
        var transform = GlobalTransform;
        transform.origin.y += 1.2f;
        transform.origin.x += 1;

        GlobalTransform = transform;

        _isOnLedge = false;
        _isClimbingLedge = false;
    }
}
