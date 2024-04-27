using System;

namespace GloryOfRitiria.Scripts.Utils;

public class ShipDesign
{
    public String Name;
    public int Cost;
    public ShipFrameSize Size;

    public ShipDesign()
    {
        Name = "Test";
        Cost = 10;
        Size = ShipFrameSize.Medium;
    }
}

public enum ShipFrameSize
{
    Small, Medium, Large
}