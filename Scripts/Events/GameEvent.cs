﻿using System.Collections.Generic;
using GloryOfRitiria.Scripts.Events.Conditions;
using GloryOfRitiria.Scripts.Scenes.Parts;

namespace GloryOfRitiria.Scripts.Events;

public struct GameEvent
{
    public string Id;
    public string Name;
    public string Description;
    public string ImagePath;
    public IEventCondition Condition;
    public List<Choice> Options;

    public GameEvent()
    {
        Id = "Empty_Event";
        Name = "Empty Event";
        Description = "Lorem Ipsum";
        ImagePath = "res://icon.svg"; // default godot icon
        Condition = null;
        Options = new List<Choice>();
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