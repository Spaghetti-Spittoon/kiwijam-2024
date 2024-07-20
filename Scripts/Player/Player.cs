using Godot;

public class Player : KinematicBody
{
    [Export]
    private float _speed = 7f;

    [Export]
    private float _jump_strength = 20f;

    [Export]
    private float _gravity = 50f;

    private Vector3 velocity;
    private Vector3 snapVector;

    [Export]
    private SpringArm _cameraArm;

    private Spatial ModelWalking;
    private Spatial ModelIdle;
    private Spatial ModelJumping;

    private RayCast RCUpper1;
    private RayCast RCUpper2;
    private RayCast RCBody;

    private bool _isOnLedge;
    private bool _isClimbingLedge;

    private float directionX;

    private AnimationPlayer _running;
    private AnimationPlayer _idle;
    private AnimationPlayer _jumping;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _cameraArm = GetNode<SpringArm>("SpringArm");
        ModelWalking = GetNode<Spatial>(nameof(ModelWalking));
        ModelIdle = GetNode<Spatial>(nameof(ModelIdle));
        ModelJumping = GetNode<Spatial>(nameof(ModelJumping));

        _running = ModelWalking.GetNode<AnimationPlayer>("AnimationPlayer");
        _running.CurrentAnimation = "Armature|walking|BaseLayer";
        _running.Play();
        _running.Connect("animation_finished", this, nameof(OnWalkingAnimFinished));

        _idle = ModelIdle.GetNode<AnimationPlayer>("AnimationPlayer");
        _idle.CurrentAnimation = "Armature|Idling|BaseLayer";
        _idle.Play();
        _idle.Connect("animation_finished", this, nameof(OnIdleAnimFinished));

        _jumping = ModelJumping.GetNode<AnimationPlayer>("AnimationPlayer");
        _jumping.CurrentAnimation = "Armature|jump|BaseLayer";
        _jumping.Play();
        _jumping.Connect("animation_finished", this, nameof(OnJumpingAnimFinished));

        //RCUpper1 = _model.GetNode<RayCast>(nameof(RCUpper1));
        //RCUpper2 = _model.GetNode<RayCast>(nameof(RCUpper2));
        //RCBody = _model.GetNode<RayCast>(nameof(RCBody));
    }

    public void OnWalkingAnimFinished(string animationName) => _running.Play();
    public void OnIdleAnimFinished(string animationName) => _idle.Play();
    public void OnJumpingAnimFinished(string animationName) => _jumping.Play();

    public override void _Process(float delta)
    {
        _cameraArm.Translation = Translation;
    }

    public override void _PhysicsProcess(float delta)
    {
        ModelWalking.Visible = false;
        ModelJumping.Visible = false;
        ModelIdle.Visible = false;

        var canMove = true;

        var moveDirection = Vector3.Zero;

        moveDirection.x = Input.GetActionStrength("right") - Input.GetActionStrength("left");
        //moveDirection = moveDirection.Rotated(Vector3.Up, _cameraArm.Rotation.y).Normalized();

        //RCUpper1.CastTo = new Vector3(0, 0, moveDirection.x);
        //RCUpper2.CastTo = new Vector3(0, 0, moveDirection.x);
        //RCBody.CastTo = new Vector3(0, 0, moveDirection.x);

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
            ModelJumping.Visible = true;
        }
        else if (isJumpingInAir)
        {
            DetectLedge();
            ModelJumping.Visible = true;
        }
        else if (justLanded)
        {
            snapVector = Vector3.Down;
        }
        else if (IsOnFloor())
        {
            velocity.y = 0;

            if (velocity.x != 0)
            {
                ModelWalking.Visible = true;
            }
        }

        if (Input.IsActionJustReleased("forward"))
        {
            if (velocity.y > 10f)
            {
                velocity.y = 10f;
            }
        }

        if (_isClimbingLedge)
        {
            return;
        }

        if (!ModelWalking.Visible && !ModelJumping.Visible) ModelIdle.Visible = true;

        MoveAndSlideWithSnap(velocity, snapVector, Vector3.Up, true);



        var lookDirection = new Vector2(velocity.z, velocity.x);

        ModelWalking.Rotation = new Vector3(ModelWalking.Rotation.x, lookDirection.Angle(), ModelWalking.Rotation.z);
        ModelIdle.Rotation = new Vector3(ModelIdle.Rotation.x, lookDirection.Angle(), ModelIdle.Rotation.z);
        ModelJumping.Rotation = new Vector3(ModelJumping.Rotation.x, lookDirection.Angle(), ModelJumping.Rotation.z);

        directionX = moveDirection.x;

        //AngleArm();
    }

    private void AngleArm()
    {
        var ratio = Mathf.Clamp(velocity.x, -10, 10) / 10;

        var rotationX = 30 * ratio;
        rotationX = Mathf.Clamp(rotationX, -90, 30);

        //var rotationY = RotationDegrees.y - (inputEvent.Relative.x * _mouseSensitivity);
        //rotationY = Mathf.Wrap(rotationY, 0, 360);

        _cameraArm.RotationDegrees = new Vector3(_cameraArm.RotationDegrees.x, rotationX, _cameraArm.RotationDegrees.z);
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
        transform.origin.x += directionX;
        velocity = Vector3.Zero;

        GlobalTransform = transform;

        _isOnLedge = false;
        _isClimbingLedge = false;
    }
}
