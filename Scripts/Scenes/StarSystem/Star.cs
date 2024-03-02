using System.Collections.Generic;
using GloryOfRitiria.Scripts.Utils;

public class Star: CelestialBody
{
    /// <summary> Name of the star </summary>
    //public string Name;
    public StarType Type; // Better to make enum
    //public string ImagePath; // Better to make enum

    /// <summary> Celestial bodies in orbits of the star </summary>
    public List<CelestialBody> Bodies;

    public CelestialBody InnerSpace;

    public StarSystemInfo StarSystem;

    /// <summary> The distance from the star to the edge of the system </summary>
    public double OuterSpaceDistance;

    public Star(string name, StarSystemInfo starSystem, double distance, StarType type = StarType.RedDwarf)
    {
        Star = this;
        Name = name;
        // If a star within a star system is not discovered, it shouldn't be selectable. Still not sure if it's needed...
        DiscoveryStatus = DiscoveryStatus.Discovered; 
        StarSystem = starSystem;
        Bodies = new List<CelestialBody>();
        InnerSpace = new CelestialBody("Inner Space",  this, distance,"res://Assets/Icons/cross.png");
        OuterSpaceDistance = distance;
        Type = type;
    }
    
    // Populate the bodies list with celestial bodies of this system
    // probably using its ID. I need to think how to populate them...
    public void LoadCelestialBodies(){

    }

    // Returns the path to the star gfx based on the star type
    public override string GetImage()
    {
        switch (Type)
        {
            case StarType.BrownDwarf: return "res://Assets/Img/tmp/CelestialBodies/BrownDwarf.png";
            case StarType.RedDwarf: return "res://Assets/Img/tmp/CelestialBodies/RedDwarf.png";
            case StarType.OrangeDwarf: return "res://Assets/Img/tmp/CelestialBodies/OrangeDwarf.png";
            case StarType.YellowDwarf: return "res://Assets/Img/tmp/CelestialBodies/YellowDwarf.png";
            case StarType.HotDwarf: return "res://Assets/Img/tmp/CelestialBodies/HotDwarf.png";
            case StarType.WhiteDwarf: return "res://Assets/Img/tmp/CelestialBodies/WhiteDwarf.png";
            default: return "res://Assets/Img/tmp/CelestialBodies/OrangeDwarf.png";
        }
    }

    // Gets the description based on the star Id from... Somewhere.
    public string GetStarDescription(){
        return "";
    }

    public void AddCelestialBody(CelestialBody body){
        Bodies.Add(body);
    }

    public void SetInnerSpaceShips(ShipGroup shipGroup)
    {
        InnerSpace.AddShipGroup(shipGroup);
    }
    
}

// There are A LOT of different types so it's better to eventually make it a more developed class