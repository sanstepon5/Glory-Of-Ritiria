using System;
using System.Collections.Generic;
using GloryOfRitiria.Scripts.Global;
using Godot;

namespace GloryOfRitiria.Scripts.Utils.Events.Conditions;

public class FlagCondition : IEventCondition
{
    private readonly List<Flags> _flags;

    public FlagCondition(IEnumerable<Flags> list)
    {
        _flags = new List<Flags>(list);
    }
    
    public FlagCondition()
    {
        _flags = new List<Flags>();
    }

    public void AddFlag(string flag)
    {
        if (Enum.TryParse(flag, out Flags myStatus))
        {
            _flags.Add(myStatus);
        }
        else
        {
            GD.Print("Invalid flag");
        }
    }

    // Returns true if all its flags are in game_state
    public bool IsSatisfied()
    {
        foreach (var flag in _flags) if (!game_state.CurrentFlags.Contains(flag)) return false;
        return true;
    }
}