using Godot;
using System;

public class GenericButton : Control
{
    [Export]
    public string TexturePath;

    [Export]
    public string PressedTexturePath;

    [Signal]
    public delegate void ButtonReleased();

    protected TextureButton Button {get; private set;}

    public override void _Ready()
    {
        Button = GetNode<TextureButton>("TextureButton");
        Button.Connect("button_up", this, nameof(OnButton_Click));
        RedrawTextures();
    }

    public void RedrawTextures() {
        if(string.IsNullOrEmpty(TexturePath) == false) {
            var normalTexture = (Texture)ResourceLoader.Load(TexturePath);
            Button.TextureNormal = normalTexture;
        }

        if(string.IsNullOrEmpty(TexturePath) == false) {
            var pressedTexture = (Texture)ResourceLoader.Load(PressedTexturePath);
            Button.TexturePressed = pressedTexture; 
        }
    }

    protected virtual void OnButton_Click() {
        EmitSignal(nameof(ButtonReleased));
    }
}
