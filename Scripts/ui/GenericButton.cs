using Godot;
using System;

public class GenericButton : Node
{
    [Export]
    public string TexturePath;

    [Export]
    public string PressedTexturePath;

    [Signal]
    public delegate void ButtonReleased();

    TextureButton button;

    public override void _Ready()
    {
        button = GetNode<TextureButton>("CenterContainer/TextureButton");
        var normalTexture = (Texture)ResourceLoader.Load(TexturePath);
        var pressedTexture = (Texture)ResourceLoader.Load(PressedTexturePath);
        button.TextureNormal = normalTexture;
        button.TexturePressed = pressedTexture;
        button.Connect("button_up", this, nameof(OnButton_Click));
    }

    void OnButton_Click() {
        EmitSignal(nameof(ButtonReleased));
    }
}
