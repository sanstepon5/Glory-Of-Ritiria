using System.Collections.Generic;
using GloryOfRitiria.Scripts;
using Godot;

public enum CelestialBodyType
{
    GenericPlanet,
    Pallyria
}

public class CelestialBody
{
    public string Name;
    // Distance from its parent star, probably in AU
    public float Distance;
    // Random planet, some specific planet, asteroid, station....
    public string ImagePath;
    public CelestialBodyType BodyType;
    // Determines if body should display satellites
    public bool HasSatellites;
    // Determines if bodies' satellites will be displayed vertically or horizontally
    public bool IsSatellite;
    public List<CelestialBody> Satellites;
    public List<Ship> ShipsInOrbit = new(); //public ShipGroup ShipsInOrbit;


    // Constructor for inherited classes
    public CelestialBody()
    {
        
    }

    // Default constructor, for the system's main celestial bodies such as planets
    public CelestialBody(string name, float distance, string imagePath, CelestialBodyType type = CelestialBodyType.GenericPlanet){
        Name = name;
        Distance = distance;
        ImagePath = imagePath;
        BodyType = type;
        HasSatellites = true;
        Satellites = new List<CelestialBody>();
        IsSatellite = false;

        
    }
    
    public CelestialBody(string name, float distance, string imagePath, bool hasSatellites, 
                         bool isSatellite, CelestialBodyType type = CelestialBodyType.GenericPlanet){
        Name = name;
        Distance = distance;
        ImagePath = imagePath;
        BodyType = type;
        HasSatellites = hasSatellites;
        IsSatellite = isSatellite;
        if (hasSatellites) Satellites = new List<CelestialBody>();
    }

    public void AddSatellite(CelestialBody satellite)
    {
        if (HasSatellites)
        {
            Satellites.Add(satellite);
        }
    }

    public void AddShipGroup(ShipGroup shipGroup)
    {
        foreach (var ship in shipGroup.Ships)
        {
            ShipsInOrbit.Add(ship);
        }
        //ShipsInOrbit = shipGroup;
    }

    // Get the position of the body on the map using distance and angle
    // public Vector2 GetGlobalPosition()
    // {
    //     return new Vector2();
    // }


    // Functions that get information about the body based on id or something
    // public string GetBodyImage(){
    //     return "";
    // }
    // public string GetBodyDescription(){
    //     return "";
    // }
}


public class MinorBody : CelestialBody
{
    public MinorBody(string name, float distance, string imagePath, bool isSatellite, CelestialBodyType type = CelestialBodyType.GenericPlanet){
        Name = name;
        Distance = distance;
        ImagePath = imagePath;
        BodyType = type;
        
        HasSatellites = false;
        IsSatellite = isSatellite;
    }
}

public class ShipGroup
{
    public string Name;
    public string ImagePath;
    public List<Ship> Ships;
    
    
    public ShipGroup(string name, string imagePath = "res://Assets/Icons/shipGroup.png", List<Ship> ships=null){
        Name = name;
        ImagePath = imagePath;

        if (ships!=null) Ships = new List<Ship>();
        else Ships = ships;
    }
    
    public ShipGroup(string name){
        Name = name;
        ImagePath = "res://Assets/Icons/shipGroup.png";

        Ships = new List<Ship>();
    }

    public void AddShip(Ship ship)
    {
        Ships.Add(ship);
    }
    
    
}