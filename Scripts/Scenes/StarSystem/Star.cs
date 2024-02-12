using System;
using System.Collections.Generic;
using GloryOfRitiria.Scripts.Utils;
using Godot;

public class Star
{
    /// <summary> Name of the star </summary>
    public string Name;
    public string StarType; // Better to make enum
    public string ImagePath; // Better to make enum

    // Celestial bodies in orbits of the star
    public List<CelestialBody> Bodies;

    public ShipGroup InnerSpace;

    public StarSystemInfo StarSystem;

    public Star(string name, StarSystemInfo starSystem, string imagePath){
        Name = name;
        StarSystem = starSystem;
        ImagePath = imagePath;
        Bodies = new List<CelestialBody>();
    }

    // For inherited classes
    public Star()
    {
        
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
        InnerSpace = shipGroup;
    }
    
}
