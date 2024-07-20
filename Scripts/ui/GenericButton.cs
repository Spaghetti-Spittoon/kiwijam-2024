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
        button.Connect("button_up", this, nameof(OnButton_Click));
        RedrawTextures();
    }

    public void RedrawTextures() {
        if(string.IsNullOrEmpty(TexturePath) == false) {
            var normalTexture = (Texture)ResourceLoader.Load(TexturePath);
            button.TextureNormal = normalTexture;
        }

        if(string.IsNullOrEmpty(TexturePath) == false) {
            var pressedTexture = (Texture)ResourceLoader.Load(PressedTexturePath);
            button.TexturePressed = pressedTexture; 
        }
    }

    void OnButton_Click() {
        EmitSignal(nameof(ButtonReleased));
    }
}
