using System.Collections.Generic;

public class Star
{
    public string Id; 
    public string Name;
    public string StarType; // Better to make enum

    // Celestial bodies in orbits of the star
    public List<CelestialBody> Bodies;

    public Star(string name){
        Name = name;
        Bodies = new List<CelestialBody>();
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

}
