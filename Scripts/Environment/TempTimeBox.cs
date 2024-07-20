using Godot;

public class TempTimeBox : StaticBody
{
    private Spatial _model;

    private bool isPhasing;
    private float flickerTimer;
    private float flickerShortening;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _model = GetNode<Spatial>("Model");
    }

    public override void _Process(float delta)
    {
        flickerTimer += delta;

        if (flickerTimer > 10)
        {
            _model.Visible = !_model.Visible;
            flickerTimer = 0;
        }
    }

    private void OnAreaCollision(Node body)
    {
        if (!isPhasing)
        {
            isPhasing = true;

            var timer = GetTree().CreateTimer(1);
            timer.Connect("timeout", this, nameof(DoPhase));
        }
    }

    private void DoPhase()
    {

    }
}
