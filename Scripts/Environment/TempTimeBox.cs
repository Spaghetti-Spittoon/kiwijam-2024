using Godot;

public class TempTimeBox : StaticBody
{
    [Export]
    private float _flickerTime = 1f;

    [Export]
    private float _ReturnTime = 2f;

    private Spatial _model;
    private CollisionShape _shape;

    private bool isPhasing;
    private float flickerTimer;
    private float flickerCap;
    private bool isHidden;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _model = GetNode<Spatial>("Model");
        _shape = GetNode<CollisionShape>("CollisionShape");
        flickerCap = _flickerTime;
    }

    public override void _Process(float delta)
    {
        if (isPhasing && !isHidden)
        {
            flickerTimer += delta;

            if (flickerTimer > flickerCap)
            {
                _model.Visible = !_model.Visible;
                flickerTimer = 0;
                flickerCap /= 1.3f;

                if (flickerCap < 0.05f)
                {
                    _model.Visible = false;
                    isHidden = true;
                    _shape.Disabled = true;

                    var timer = GetTree().CreateTimer(_ReturnTime);
                    timer.Connect("timeout", this, nameof(ReturnToWorld));
                }
            }
        }
    }

    private void OnAreaCollision(Node body)
    {
        if (body.GetType() == typeof(Player))
        {
            if (!isPhasing)
            {
                isPhasing = true;
            }
        }
    }

    private void ReturnToWorld()
    {
        _model.Visible = true;
        _shape.Disabled = false;
        isHidden = false;
        isPhasing = false;
        flickerCap = _flickerTime;
    }
}
