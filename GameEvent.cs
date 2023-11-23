using System.Collections.Generic;

public struct GameEvent
{
    public string Name;
    public string Description;
    public string ImagePath;
    public List<EventCondition> Conditions;

    public GameEvent()
    {
        Name = "Empty Event";
        Description = "Lorem Ipsum";
        ImagePath = "res://icon.svg"; // default godot icon
        Conditions = new List<EventCondition>();
    }
}