using System;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.ShipRelated.Missions;
using GloryOfRitiria.Scripts.StarSystem;
using Godot;

namespace GloryOfRitiria.Scripts.Effects;

public abstract class Effect
{
    public string Desc; // to display in tooltips. Maybe a to_string is necessary too.
    
    public Effect()
    {
    }

    public Effect(string desc)
    {
        Desc = desc;
    }
    
    public abstract void ApplyEffect();


}

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