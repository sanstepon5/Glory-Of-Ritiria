using System;
using GloryOfRitiria.Scripts.Utils.Events;
using Godot;

namespace GloryOfRitiria.Scripts.Utils;

/**
 * <param name="VarName">Name of the attribute in gamestate</param>
 * <param name="CondType">What kind of comparison should be made.
 * Should be different for different types. For exemple &quot;&lt;&quot; or "&gt;" for an int
 * and &quot;earlier&quot; for a date</param>
 * <param name="Value">the value for which the condition should be true. For exemple:
 * gamestate has attribute turn = 3. With EventCondition = {VarName="turn", CondType=">", Value="2"}
 * it should be true. Should be read as "game turn should be bigger than 2"</param>
 */
public class SingleCondition : IEventCondition
{
    public string VarName;
    public string CondType;
    public string Value;

    public SingleCondition(string varName, string condType, string value)
    {
        VarName = varName;
        CondType = condType;
        Value = value;
    }

    public bool IsSatisfied()
    {
        var attributeValues = game_state.GetAttributeValues();
        if (!attributeValues.TryGetValue(VarName, out var attribute)){
            throw new Exception($"Attribute of name {VarName} doesn't exist in game_state");
        }
        var attributeType = attribute.Item1;
        var attributeVal = attribute.Item2;

        switch (attributeType.Name)
        {
            case "Int32":
                // Normalement ne doit pas etre null..
                var comparableIntValue = (int)Convert.ChangeType(Value, attributeType)!;

                return CondType switch
                {
                    "==" => ((int)attributeVal == comparableIntValue),
                    "<" => ((int)attributeVal < comparableIntValue),
                    ">" => ((int)attributeVal > comparableIntValue),
                    _ => throw new Exception($"Invalid condition ({CondType}) for type {attributeType.Name}")
                };

            case "String":
                // Normalement ne doit pas etre null..
                var comparableStringValue = (string)Convert.ChangeType(Value, attributeType)!;

                return CondType switch
                {
                    // There will probably be a lot here: earlier, later, ...
                    "==" => (((string)attributeVal).Equals(comparableStringValue)),
                    _ => throw new Exception($"Invalid condition ({CondType}) for type {attributeType.Name}")
                };
            case "Double":
                // Normalement ne doit pas etre null..
                var comparableDoubleValue = (double)Convert.ChangeType(Value, attributeType)!;

                return CondType switch
                {
                    // To compensate for loss of precision
                    "==" => (Math.Abs((double)attributeVal - comparableDoubleValue) < 0.01), 
                    "<" => ((double)attributeVal < comparableDoubleValue),
                    ">" => ((double)attributeVal > comparableDoubleValue),
                    _ => throw new Exception($"Invalid condition ({CondType}) for type {attributeType.Name}")
                };
        }

        // Should never get here ideally
        return false;
    }
}