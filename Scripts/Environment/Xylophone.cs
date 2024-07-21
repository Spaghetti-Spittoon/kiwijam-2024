using Godot;

public class Xylophone : StaticBody
{
    private AudioStreamPlayer _note;
    public override void _Ready()
    {
        _note = GetNode<AudioStreamPlayer>("MusicNote");
    }
    private void PlayNote(Node body)
    {
        if (body.GetType() == typeof(Player))
        {
            _note.Play(0);
        }
    }
}
