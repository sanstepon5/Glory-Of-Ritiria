using System;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.Utils;
using Godot;

namespace GloryOfRitiria.Scripts;

public class Effect
{
    public string Desc; // to display in tooltips. Maybe a to_string is necessary too.
    public string MethodName; // name of a method of game state (should exist)
    public string Value; // value used for the method

    public Effect(string methodName, string value = "", string desc = "idunnolol")
    {
        Desc = desc;
        MethodName = methodName;
        Value = value;
    }
    
    // TODO: Redo the whole value extraction process. It simply won't work with custom classes.
    // Modifies the state of the game depending on the effect
    public void ApplyEffect()
    {
        //TODO: Need to check for cases where wrong values are used for methods
        switch (MethodName)
        {
            case "AddRes1":
                game_state.Res1 += (double)Convert.ChangeType(Value, typeof(double))!;
                break;
            case "SetRes1":
                game_state.Res1 = (double)Convert.ChangeType(Value, typeof(double))!;
                break;
            case "AddFlag":
                {
                Enum.TryParse(Value, out Flags flag);
                game_state.CurrentFlags.Add(flag);
                GD.Print(game_state.CurrentFlags);
                }
                break;
            case "RemoveFlag":
                {
                Enum.TryParse(Value, out Flags flag);
                game_state.CurrentFlags.Remove(flag);
                }
                break;
            case "DiscoverPlanet":
                {
                    game_state.DiscoverPlanet(Value);
                }
                break;
        }
    }

    
}