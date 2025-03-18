using System.Collections.Generic;
using GloryOfRitiria.Scripts.StarSystem;

public class Star: CelestialBody
{
    /// <summary> Name of the star </summary>
    //public string Name;
    public StarClass StarClass;
    //public string ImagePath; // Better to make enum

    /// <summary> Celestial bodies in orbits of the star </summary>
    public List<CelestialBody> Bodies;

    public CelestialBody InnerSpace;

    public StarSystemInfo StarSystem;

    /// <summary> The distance from the star to the edge of the system </summary>
    public double OuterSpaceDistance;

    public Star(string name, StarSystemInfo starSystem, double distance, StarClass starClass = StarClass.RedDwarf)
    {
        Star = this;
        Name = name;
        // If a star within a star system is not discovered, it shouldn't be selectable. Still not sure if it's needed...
        DiscoveryStatus = DiscoveryStatus.Explored; 
        StarSystem = starSystem;
        Bodies = [];
        InnerSpace = new CelestialBody("Inner Space",  this, distance,"res://Assets/Icons/cross.png");
        OuterSpaceDistance = distance;
        StarClass = starClass;
        SizeFactor = 1.5f;
    }
    
    /**Incomplete constructor for parsing. Other properties should be added during parsing*/
    public Star(StarSystemInfo starSystem, double distance)
    {
        Star = this;
        StarSystem = starSystem;
        Bodies = [];
        InnerSpace = new CelestialBody("Inner Space",  this, distance,"res://Assets/Icons/cross.png");
        OuterSpaceDistance = distance;
    }


    public override void SetImagePath()
    {
        switch (StarClass)
        {
            case StarClass.BrownDwarf:
                SizeFactor = 1.2f; 
                ImagePath = "res://Assets/Img/tmp/CelestialBodies/BrownDwarf.png"; 
                break;
            case StarClass.RedDwarf: 
                SizeFactor = 1.5f;
                ImagePath = "res://Assets/Img/tmp/CelestialBodies/Stars/RedStar.png"; 
                break;
            case StarClass.OrangeDwarf: 
                SizeFactor = 1.8f;
                ImagePath = "res://Assets/Img/tmp/CelestialBodies/Stars/OrangeStar.png"; 
                break;
            case StarClass.YellowDwarf: 
                SizeFactor = 2.0f;
                ImagePath = "res://Assets/Img/tmp/CelestialBodies/Stars/YellowStar.png"; 
                break;
            case StarClass.HotDwarf: 
                ImagePath = "res://Assets/Img/tmp/CelestialBodies/HotDwarf.png"; break;
            case StarClass.WhiteDwarf: 
                SizeFactor = 1.2f;
                ImagePath = "res://Assets/Img/tmp/CelestialBodies/WhiteDwarf.png"; break;
            default: ImagePath = "res://Assets/Img/tmp/CelestialBodies/OrangeDwarf.png"; break;
        }
    }

    public override string GetImage()
    {
        // For stars it doesn't really makes sense not to know what they look like... 
        // if (DiscoveryStatus == DiscoveryStatus.Explored) 
        return ImagePath;
        // return "res://Assets/Img/tmp/CelestialBodies/Stars/UknownStar.png";
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