using System.Collections.Generic;
using GloryOfRitiria.Scripts.Utils;
using GloryOfRitiria.Scripts.Utils.Events;

public struct GameEvent
{
    public string Id;
    public string Name;
    public string Description;
    public string ImagePath;
    public IEventCondition Condition;

    public GameEvent()
    {
        Id = "Empty_Event";
        Name = "Empty Event";
        Description = "Lorem Ipsum";
        ImagePath = "res://icon.svg"; // default godot icon
        Condition = null;
    }

    public void SetSingleCondition(string varName, string condType, string value)
    {
        Condition = new SingleCondition(varName, condType, value);
    }
    

    public bool IsSatisfied()
    {
        return Condition.IsSatisfied();
    }
}