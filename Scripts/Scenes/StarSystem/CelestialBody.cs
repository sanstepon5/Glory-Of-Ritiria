using System.Collections.Generic;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.ShipRelated.Missions;
using GloryOfRitiria.Scripts.Utils;
using Godot;
using Ship = GloryOfRitiria.Scripts.ShipRelated.Ship;

public class CelestialBody
{
    public string Id;
    public string Name;
    // Parent Star
    public Star Star;

    public CelestialBody ParentBody = null;

    /// <summary> The distance from the star to this body in light minutes</summary>
    public double Distance;

    public DiscoveryStatus DiscoveryStatus;
    
    public CelestialBodyType BodyType;
    // Determines if body should display satellites
    public bool HasSatellites;
    // Determines if bodies' satellites will be displayed vertically or horizontally
    public bool IsSatellite;
    public List<CelestialBody> Satellites;
    public List<Ship> ShipsInOrbit = new(); //public ShipGroup ShipsInOrbit;
    
    // Random planet, some specific planet, asteroid, station....
    private string _imagePath; 

    public List<Shipyard> Shipyards = new();
    
    
    // Default constructor, for the system's main celestial bodies such as planets
    public CelestialBody(string name, Star star, double distance, string imagePath, 
        DiscoveryStatus discoveryStatus = DiscoveryStatus.Undiscovered, 
        CelestialBodyType type = CelestialBodyType.GenericPlanet){
        Name = name;
        Star = star;
        Distance = distance;
        DiscoveryStatus = discoveryStatus;
        _imagePath = imagePath;
        BodyType = type;
        HasSatellites = true;
        Satellites = new List<CelestialBody>();
        IsSatellite = false;
    }
    
    public CelestialBody(string name, Star star, double distance, string imagePath, bool hasSatellites, 
                         bool isSatellite, DiscoveryStatus discoveryStatus = DiscoveryStatus.Undiscovered, 
                         CelestialBodyType type = CelestialBodyType.GenericPlanet){
        Name = name;
        Star = star;
        Distance = distance;
        Distance = distance;
        DiscoveryStatus = discoveryStatus;
        _imagePath = imagePath;
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
    
    /* Constructor for parsing */
    public CelestialBody(string id)
    {
        Id = id;
        if (Id.Equals("pallyria"))
            BodyType = CelestialBodyType.Pallyria;
        else
            BodyType = CelestialBodyType.GenericPlanet;
        
        HasSatellites = false;
        IsSatellite = false;
    }

    public void SetImagePath(string path)
    {
        _imagePath = game_state.AssetsDir + "Img/tmp/CelestialBodies/" + path;
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
    
    public void AddShipyard(Shipyard shipyard)
    {
        if (Shipyards.Count == 0) game_state.BodiesWithShipyards.Add(this);
        Shipyards.Add(shipyard);
    }
    
    public void AddBusyShipyard(string name, Ship ship, int progress = 0)
    {
        if (Shipyards.Count == 0) game_state.BodiesWithShipyards.Add(this);
        Shipyards.Add(new Shipyard(name, this, ship, progress));
    }

    /**Explore the body giving more information about it on the system map*/
    public void ExplorePlanet()
    {
        if (DiscoveryStatus == DiscoveryStatus.ExistenceKnown)
            DiscoveryStatus = DiscoveryStatus.Explored;
        else
            GD.Print("A ship tried exploring already explored body: " + Name);
    }

    /**Discover the body making it visible on the system map*/
    public void DiscoverBody()
    {
        if (DiscoveryStatus == DiscoveryStatus.Undiscovered)
            DiscoveryStatus = DiscoveryStatus.ExistenceKnown;
        else
            GD.Print("A ship tried discovering already known body: " + Name);
    }

    public bool IsMissionCompatible(Mission mission)
    {
        switch (mission)
        {
            case PlanetExplorationMission:
                return (this is not global::Star) && DiscoveryStatus != DiscoveryStatus.Explored;
            case SystemExplorationMission: // Can still try to explore system even if everything is found
                return (this is Star);
            default:
                return false;
        }
    }


    // Returns path to the small version (32x32) of the bodies' image
    public string GetSmallImage()
    {
        return "res://Assets/GUI/Icons/32/liveablePlanet.png";
    }

    public virtual string GetImage()
    {
        if (DiscoveryStatus == DiscoveryStatus.Explored) return _imagePath;
        return "res://Assets/Img/tmp/CelestialBodies/UndiscoveredPlanet.png";
    }


    public override string ToString()
    {
        var result = "";

        result += GetType().Name + ": " + Name + "\n";
        result += "Exploration status: " + DiscoveryStatus + "\n";
        result += "Image Path: " + _imagePath + "\n";
        
        return result;
    }
}

public enum DiscoveryStatus
{
    Explored,
    ExistenceKnown,
    Undiscovered
}