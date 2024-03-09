using System.Collections.Generic;
using GloryOfRitiria.Scripts;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.Utils;
using Ship = GloryOfRitiria.Scripts.ShipRelated.Ship;

public class CelestialBody
{
    /**Also server as an "ID" for now*/
    public string Name;
    // Parent Star
    public Star Star;

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

    public void ElevateDiscoveryStatus()
    {
        switch (DiscoveryStatus)
        {
            case DiscoveryStatus.Undiscovered: DiscoveryStatus = DiscoveryStatus.ExistenceKnown; break;
            case DiscoveryStatus.ExistenceKnown: DiscoveryStatus = DiscoveryStatus.Discovered; break;
        }
    }


    // Returns path to the small version (32x32) of the bodies' image
    public string GetSmallImage()
    {
        return "res://Assets/GUI/Icons/32/liveablePlanet.png";
    }

    public virtual string GetImage()
    {
        if (DiscoveryStatus == DiscoveryStatus.Discovered) return _imagePath;
        return "res://Assets/Img/tmp/CelestialBodies/UndiscoveredPlanet.png";
    }
}

public enum DiscoveryStatus
{
    Discovered,
    ExistenceKnown,
    Undiscovered
}