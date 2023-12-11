using System;
using Godot;

namespace GloryOfRitiria.Scripts.Utils.Events;

public class Effect
{
    public string desc; // to display in tooltips. Maybe a to_string is necessary too.
    public string methodName; // name of a method of game state (should exist)
    public string value; // value used for the method

    public Effect(string methodName, string value, string desc = "idunnolol")
    {
        this.desc = desc;
        this.methodName = methodName;
        this.value = value;
    }

    // Modifies the state of the game depending on the effect
    public void ApplyEffect()
    {
        //TODO: Need to check for cases where wrong values are used for methods
        switch (methodName)
        {
            case "AddRes1":
                game_state.Res1 += (double)Convert.ChangeType(value, typeof(double))!;
                break;
            case "SetRes1":
                game_state.Res1 = (double)Convert.ChangeType(value, typeof(double))!;
                break;
            case "AddFlag":
                {
                Enum.TryParse(value, out Flags flag);
                game_state.CurrentFlags.Add(flag);
                GD.Print(game_state.CurrentFlags);
                }
                break;
            case "RemoveFlag":
                {
                Enum.TryParse(value, out Flags flag);
                game_state.CurrentFlags.Add(flag);
                }
                break;
        }
    }
}