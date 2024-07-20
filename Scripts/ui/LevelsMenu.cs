using Godot;
using System;

public class LevelsMenu : CanvasLayer
{
    SceneTree tree;
    EventBus bus;
    TextureButton backButton;
    GridContainer grid;
    Progress progressState;

    public override void _Ready()
    {
        tree = GetTree();
        bus = GetNode<EventBus>("/root/EventBus");
        backButton = GetNode<TextureButton>("BackButton/TextureButton");
        grid = GetNode<GridContainer>("GridContainer");
        progressState = GetNode<Progress>("/root/Progress");
        backButton.Connect("button_up", this, nameof(OnBackButton_Click));
        bus.Connect(nameof(EventBus.GridButtonReleased), this, nameof(OnLevel_Click));
        AddLevelButtons();
    }

    void OnBackButton_Click() {
        var mainMenu = (PackedScene)ResourceLoader.Load("res://scenes/MainMenu.tscn");
        var levelsInstance = mainMenu.Instance();
	    tree.Root.AddChild(levelsInstance);
        bus.EmitSignal(nameof(EventBus.MainMenuAppeared));
        QueueFree();
    }

    void AddLevelButtons() {
        var buttonScene = (PackedScene)ResourceLoader.Load("res://scenes/ui/GenericButton.tscn");

        for(int i = 1; i <= progressState.LevelAchieved; i++) {
            GridButton buttonInstance = buttonScene.Instance<GridButton>();
            buttonInstance.GridIndex = i;
            grid.AddChild(buttonInstance);
        }
    }

    void OnLevel_Click(int gridIndex) {
        string scenePath;

        switch(gridIndex) {
            case 1:
                scenePath = "res://scenes/levels/Level1.tscn";
                break;

            default:
                throw new Exception("ERROR: level 0 not valid");
        }
        Node level = ((PackedScene)ResourceLoader.Load(scenePath)).Instance();
        GotoLevel(level);
    }

    void GotoLevel(Node level) {
        tree.Root.AddChild(level);
        QueueFree();
    }
}
