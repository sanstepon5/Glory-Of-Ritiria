/**
 * <param name="VarName">Name of the attribute in gamestate</param>
 * <param name="CondType">What kind of comparison should be made.
 * Should be different for different types. For exemple &quot;&lt;&quot; or "&gt;" for an int
 * and &quot;earlier&quot; for a date</param>
 * <param name="Value">the value for which the condition should be true. For exemple:
 * gamestate has attribute turn = 3. With EventCondition = {VarName="turn", CondType=">", Value="2"}
 * it should be true. Should be read as "game turn should be bigger than 2"</param>
 */
public struct EventCondition
{
    // VarName the 
    public string VarName;
    // CondType   
    public string CondType;
    public string Value;

    public EventCondition(string varName, string condType, string val)
    {
        VarName = varName;
        CondType = condType;
        Value = val;
    }
}