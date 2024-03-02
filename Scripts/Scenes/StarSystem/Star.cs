using System.Collections.Generic;
using GloryOfRitiria.Scripts.Utils;

public class Star: CelestialBody
{
    /// <summary> Name of the star </summary>
    //public string Name;
    public string StarType; // Better to make enum
    //public string ImagePath; // Better to make enum

    /// <summary> Celestial bodies in orbits of the star </summary>
    public List<CelestialBody> Bodies;

    public CelestialBody InnerSpace;

    public StarSystemInfo StarSystem;

    /// <summary> The distance from the star to the edge of the system </summary>
    public double OuterSpaceDistance;

    public Star(string name, StarSystemInfo starSystem, double distance, string imagePath)
    {
        Star = this;
        Name = name;
        StarSystem = starSystem;
        ImagePath = imagePath;
        Bodies = new List<CelestialBody>();
        InnerSpace = new CelestialBody("Inner Space",  this, distance,"res://Assets/Icons/cross.png");
        OuterSpaceDistance = distance;
    }
    
    // Populate the bodies list with celestial bodies of this system
    // probably using its ID. I need to think how to populate them...
    public void LoadCelestialBodies(){

    }

    // Returns the path to the star gfx based on the star type
    public string GetStarImage()
    {
        return "";
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
