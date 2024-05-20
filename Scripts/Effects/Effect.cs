using System;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.ShipRelated.Missions;
using GloryOfRitiria.Scripts.StarSystem;
using Godot;

namespace GloryOfRitiria.Scripts.Effects;

public abstract class Effect
{
    public string Desc; // to display in tooltips. Maybe a to_string is necessary too.
    public string MethodName; // name of a method of game state (should exist)
    public string BasicValue; // value used for the method

    
    public Effect()
    {
    }

    public Effect(string methodName, string desc)
    {
        Desc = desc;
        MethodName = methodName;
    }
    
    public Effect(string desc)
    {
        Desc = desc;
    }
    
    public abstract void ApplyEffect();


}

public class EventEffect : Effect
{
    public EventEffect(string methodName, string desc = "idunnolol") : base(methodName, desc)
    {
        
    }

    public void SetBasicValue(string value)
    {
        BasicValue = value;
    }
    
    public override void ApplyEffect()
    {
        if (BasicValue == null)
        {
            GD.PrintErr("Trying to apply effect with no value : " + MethodName);
            return;
        }
        
        //TODO: Need to check for cases where wrong values are used for methods
        switch (MethodName)
        {
            case "AddRes1":
                game_state.Res1 += (double)Convert.ChangeType(BasicValue, typeof(double))!;
                break;
            case "SetRes1":
                game_state.Res1 = (double)Convert.ChangeType(BasicValue, typeof(double))!;
                break;
            case "AddFlag":
            {
                Enum.TryParse(BasicValue, out Flags flag);
                game_state.CurrentFlags.Add(flag);
                GD.Print(game_state.CurrentFlags);
            }
                break;
            case "RemoveFlag":
            {
                Enum.TryParse(BasicValue, out Flags flag);
                game_state.CurrentFlags.Remove(flag);
            }
                break;

        }
    }
}

public abstract class MissionEffect : Effect
{
    public CelestialBody TargetBody;
    public Mission Mission;

    // Body should probably be an argument at construction, effect should know the target from the start?
    public MissionEffect(Mission mission, string desc) : base(desc)
    {
        Mission = mission;
    }

    public void SetBodyParam(CelestialBody body)
    {
        TargetBody = body;
    }
}