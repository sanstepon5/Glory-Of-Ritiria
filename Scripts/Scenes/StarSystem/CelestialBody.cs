using System.Collections.Generic;
using GloryOfRitiria.Scripts;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.Utils;
using Godot;

public enum CelestialBodyType
{
    Star,
    GenericPlanet,
    MinorBody,
    Pallyria
}

public class CelestialBody
{
    public string Name;
    // Parent Star
    public Star Star;

    /// <summary> The distance from the star to this body in light minutes</summary>
    public double Distance;
    
    public CelestialBodyType BodyType;
    // Determines if body should display satellites
    public bool HasSatellites;
    // Determines if bodies' satellites will be displayed vertically or horizontally
    public bool IsSatellite;
    public List<CelestialBody> Satellites;
    public List<Ship> ShipsInOrbit = new(); //public ShipGroup ShipsInOrbit;
    
    // Random planet, some specific planet, asteroid, station....
    public string ImagePath;

    public List<Shipyard> Shipyards = new();
    
    
    // Default constructor, for the system's main celestial bodies such as planets
    public CelestialBody(string name, Star star, double distance, string imagePath, CelestialBodyType type = CelestialBodyType.GenericPlanet){
        Name = name;
        Star = star;
        Distance = distance;
        ImagePath = imagePath;
        BodyType = type;
        HasSatellites = true;
        Satellites = new List<CelestialBody>();
        IsSatellite = false;
    }
    
    public CelestialBody(string name, Star star, double distance, string imagePath, bool hasSatellites, 
                         bool isSatellite, CelestialBodyType type = CelestialBodyType.GenericPlanet){
        Name = name;
        Star = star;
        //if (isSatellite) Distance = 
        Distance = distance;
        ImagePath = imagePath;
        BodyType = type;
        HasSatellites = hasSatellites;
        IsSatellite = isSatellite;
        if (hasSatellites) Satellites = new List<CelestialBody>();
    }

    // Constructor for Star
    public CelestialBody()
    {
        BodyType = CelestialBodyType.Star;
        Distance = 0;
        HasSatellites = false;
        Satellites = null;
        IsSatellite = false;
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

    public void AddShipyard(string name)
    {
        if (Shipyards.Count == 0) game_state.BodiesWithShipyards.Add(this);
        Shipyards.Add(new Shipyard(name, this));
    }
    public void AddBusyShipyard(string name, Ship ship, int progress = 0)
    {
        if (Shipyards.Count == 0) game_state.BodiesWithShipyards.Add(this);
        Shipyards.Add(new Shipyard(name, this, ship, progress));
    }


    // Returns path to the small version (32x32) of the bodies' image
    public string GetSmallImage()
    {
        return "res://Assets/GUI/Icons/32/liveablePlanet.png";
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