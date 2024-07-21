using Godot;
using System;
//final
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

	private AudioStreamPlayer _walkSound;
	private AudioStreamPlayer _jumpSound;

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

		_walkSound = GetNode<AudioStreamPlayer>("FootstepPlayer");
		_walkSound.Connect("finished", this, nameof(OnWalkingSoundFinished));

		_jumpSound = GetNode<AudioStreamPlayer>("JumpPlayer");

		//RCUpper1 = _model.GetNode<RayCast>(nameof(RCUpper1));
		//RCUpper2 = _model.GetNode<RayCast>(nameof(RCUpper2));
		//RCBody = _model.GetNode<RayCast>(nameof(RCBody));
	}

	public void OnWalkingAnimFinished(string animationName) => _running.Play();
	public void OnIdleAnimFinished(string animationName) => _idle.Play();
	public void OnJumpingAnimFinished(string animationName) => _jumping.Play();

	public void OnWalkingSoundFinished()
	{
		_walkSound.Stop();

		if (ModelWalking.Visible)
		{
			_walkSound.Play(0);
		}
	}

	public override void _Process(float delta)
	{
		_cameraArm.Translation = Translation;

		if (!ModelWalking.Visible && _walkSound.Playing) _walkSound.Stop();
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

			_jumpSound.Play(0);
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

				if (!_walkSound.Playing)
				{
					_walkSound.Play(0);
				}
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

		AngleArm(delta);
	}

	private float GetConstantVelocityPosition(float val, int direction, float angle_max)
	{
		return val * direction * angle_max;
	}

	bool toggle_move_linear = false;
	float toggle_move_linear_amount = 0;
	float counter_x = 0;
	float counter_x_inertia = 0;

	int direction = 0;
	public void AngleArm(float time)
	{
		const float k_camera_turn_angle = 25;
		const float k_linear_turn_speed = 3;
		const int k_direction_left = -1;
		const int k_direction_middle = 0;
		const int k_direction_right = 1;

		counter_x_inertia = Mathf.Clamp(counter_x_inertia, -0.5f, 0);

		float rotationX = 0;
		// rotationX = Mathf.Clamp(rotationX, -90, 30);

		// var rotationY = RotationDegrees.y - (inputEvent.Relative.x * _mouseSensitivity);
		// rotationY = Mathf.Wrap(rotationY, 0, 360);


		// if (velocity.x >= k_direction_right)
		if (velocity.x <= k_direction_left)
		{
			if (toggle_move_linear)
			{
				if (direction == k_direction_left)
				{
					if (counter_x > 0)
					{

						counter_x -= time * k_linear_turn_speed;
					}
					else
					{
						direction = k_direction_right;
					}
				}
				else if (direction == k_direction_right)
				{
					if (counter_x < 1)
					{
						counter_x += time * k_linear_turn_speed;
					}
					else
					{
						toggle_move_linear = false;
					}
				}
				rotationX = GetConstantVelocityPosition(counter_x, direction, k_camera_turn_angle);
			}
			else
			{
				if (counter_x > 0.2 && direction == k_direction_left)
				{
					toggle_move_linear = true;

					//set map out the counter_x linear degree To the current cubic ???
					counter_x = easeIn(counter_x);

					rotationX = easeIn(counter_x) * k_camera_turn_angle * direction;

					toggle_move_linear_amount = counter_x;
				}
				else
				{
					direction = k_direction_right;
					if (counter_x_inertia >= 0)
					{
						counter_x += time;
					}
					else
					{
						counter_x_inertia += time;
					}
					rotationX = easeIn(counter_x) * k_camera_turn_angle * direction;
				}

			}

		}
		// else if (velocity.x <= k_direction_left)
		else if (velocity.x >= k_direction_right)
		{
			if (toggle_move_linear)
			{
				if (direction == k_direction_right)
				{
					if (counter_x > 0)
					{
						counter_x -= time * k_linear_turn_speed;
					}
					else
					{
						direction = k_direction_left;
					}
				}
				else if (direction == k_direction_left)
				{
					if (counter_x < 1)
					{
						counter_x += time * k_linear_turn_speed;
					}
					else
					{
						toggle_move_linear = false;
					}
				}
				rotationX = GetConstantVelocityPosition(counter_x, direction, k_camera_turn_angle);
			}
			else
			{
				if (counter_x > 0.2 && direction == k_direction_right)
				{
					toggle_move_linear = true;
					counter_x = easeIn(counter_x);
					toggle_move_linear_amount = counter_x;
					rotationX = easeIn(counter_x) * k_camera_turn_angle * direction;
				}
				else
				{
					direction = k_direction_left;
					if (counter_x_inertia >= 0)
					{
						counter_x += time;
					}
					else
					{
						counter_x_inertia += time;
					}
					rotationX = easeIn(counter_x) * k_camera_turn_angle * direction;
				}

			}
		}
		else
		{
			if (counter_x > 0)
			{
				if (counter_x > 1)
				{
					counter_x = 0.7f;
				}
				counter_x -= time;
				counter_x_inertia = -2;
				rotationX = easeIn(counter_x) * k_camera_turn_angle * direction;
			}
			else
			{
				counter_x = 0;
				counter_x_inertia -= time * 2;
				toggle_move_linear = false;
				direction = k_direction_middle;
			}
		}

		if (Input.IsMouseButtonPressed(1))
		{
			// GD.Print(GetViewport().Size.x);
			// GD.Print(GetViewport().GetMousePosition().x);
			// rotationX = (GetViewport().GetMousePosition().x / GetViewport().Size.x);

			float half = GetViewport().Size.x / 2;
			rotationX = ((GetViewport().GetMousePosition().x - half) / half) * -60;
		}


		_cameraArm.RotationDegrees = new Vector3(_cameraArm.RotationDegrees.x, rotationX, _cameraArm.RotationDegrees.z);
	}




	public float easeIn(float val)
	{

		float x = Mathf.Clamp(val * 3, 0, 1); // 1 second
											  //Cubic
											  // return x < 0.5 ? 4 * x * x * x : 1 - (float)Math.Pow(-2 * x + 2, 3) / 2;

		//Sine
		return -(float)(Math.Cos(Math.PI * x) - 1) / 2;
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
