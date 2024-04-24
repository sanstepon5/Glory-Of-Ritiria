using System;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.Utils;
using Godot;

namespace GloryOfRitiria.Scripts;

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

public class ShipEffect : Effect
{
    // I'm not a fan of this approach but I don't see a different direct enough approach to this
    // Basically it makes life easier for effects created during the game (ie mission effects)
    // We can know the target (a body for example) instance immediately, without the need to loop over everything
    // For events it will still have to use the string id value as no instances exist on event parsing
    public CelestialBody BodyParam;

    public ShipEffect(string methodName, string desc = "idunnolol") : base(methodName, desc)
    {
        
    }

    public void SetBodyParam(CelestialBody body)
    {
        BodyParam = body;
    }
    
    public override void ApplyEffect()
    {
        if (BodyParam == null)
        {
            GD.PrintErr("Trying to apply effect with no target : " + MethodName);
            return;
        }
        
        switch (MethodName)
        {
            case "ExplorePlanet":
                BodyParam.ExplorePlanet();
                break;
            case "DiscoverSystem":
                var star = BodyParam as Star;
                game_state.DiscoverSystem(star);
                break;
            default:
                GD.PrintErr("Unsupported effect for ship mission");
                break;
        }
    }
}