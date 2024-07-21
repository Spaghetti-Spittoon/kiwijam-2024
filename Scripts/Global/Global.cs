using Godot;

public class Global : Spatial
{
    private AudioStreamPlayer _themePlayer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _themePlayer = GetNode<AudioStreamPlayer>("ThemePlayer");
    }

    public void Finished()
    {
        _themePlayer.Stop();
        _themePlayer.Play(0);
    }
}
