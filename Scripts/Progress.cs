using Godot;

public class Progress : Node {
    public int LevelAchieved { get; private set; } = 1;

    EventBus bus;

    public override void _Ready()
    {
        bus = GetNode<EventBus>("/root/EventBus");
        bus.Connect(nameof(EventBus.LevelOneAchieved), this, nameof(OnLevelOneAchieved));
    }

    void OnLevelOneAchieved() {
        if(LevelAchieved < 1) {
            LevelAchieved++;
        }
    }
}
