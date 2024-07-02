using System;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.ShipRelated.Missions;
using Godot;

namespace GloryOfRitiria.Scripts.Effects;

public abstract class Effect
{
    public string Desc; // to display in tooltips. Maybe a to_string is necessary too.

    protected Effect()
    {
    }

    protected Effect(string desc)
    {
        Desc = desc;
    }
    
    public abstract void ApplyEffect();
}

// TODO: Refactor this class (and the parser) like the ship effects
public class EventEffect : Effect
{
    private string _methodName; // name of a method of game state (should exist)
    private string _basicValue; // value used for the method

    
    public EventEffect(string methodName, string desc = "idunnolol") : base(desc)
    {
        _methodName = methodName;
    }

    public void SetBasicValue(string value)
    {
        _basicValue = value;
    }
    
    public override void ApplyEffect()
    {
        if (_basicValue == null)
        {
            GD.PrintErr("Trying to apply effect with no value : " + _methodName);
            return;
        }
        
        //TODO: Need to check for cases where wrong values are used for methods
        switch (_methodName)
        {
            case "AddRes1":
                game_state.Res1 += (double)Convert.ChangeType(_basicValue, typeof(double))!;
                break;
            case "SetRes1":
                game_state.Res1 = (double)Convert.ChangeType(_basicValue, typeof(double))!;
                break;
            case "AddFlag":
            {
                Enum.TryParse(_basicValue, out Flags flag);
                game_state.CurrentFlags.Add(flag);
                GD.Print(game_state.CurrentFlags);
            }
                break;
            case "RemoveFlag":
            {
                Enum.TryParse(_basicValue, out Flags flag);
                game_state.CurrentFlags.Remove(flag);
            }
                break;

        }
    }
    
    public string GetTooltipText()
    {
        switch (_methodName)
        {
            case "AddRes1":
                var value = (double)Convert.ChangeType(_basicValue, typeof(double))!;
                if (value > 0)
                {
                    return "[b]Gain [color=green]" + value +"[/color] material resources[/b]";
                }
                return "[b]Lose [color=red]" + -value +"[/color] material resources[/b]";
            case "SetRes1": // This one will probably be removed
                value = (double)Convert.ChangeType(_basicValue, typeof(double))!;
                if (value > 0)
                {
                    return "[b]Our amount of material resources will change to : [color=green]" + value +"[/color][/b]";
                }
                return "[b]Our amount of material resources will change to : [color=red]" + value +"[/color][/b]";
            case "AddFlag":
            case "RemoveFlag":
                return "[b]This choice might have consequences, good or bad[/b]";
            default:
                return "";
        }
    }
}

public abstract class MissionEffect : Effect
{
    protected Mission Mission;

    // Body should probably be an argument at construction, effect should know the target from the start?
    protected MissionEffect(Mission mission, string desc) : base(desc)
    {
        Mission = mission;
    }
}