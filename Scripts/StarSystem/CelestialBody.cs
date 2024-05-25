using System;
using System.Collections.Generic;
using System.Linq;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.ShipRelated;
using GloryOfRitiria.Scripts.ShipRelated.Missions;
using Godot;

namespace GloryOfRitiria.Scripts.StarSystem;

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
    
    
    // Determines if body should display satellites
    public bool HasSatellites;
    // Determines if bodies' satellites will be displayed vertically or horizontally
    public bool IsSatellite;
    public List<CelestialBody> Satellites;
    public List<Ship> ShipsInOrbit = new(); //public ShipGroup ShipsInOrbit;
    
    // Random planet, some specific planet, asteroid, station....
    private string _imagePath; 

    public List<Shipyard> Shipyards = new();

    private string _customDescription;


    public CelestialBodyState KnownState;
    public CelestialBodyState ActualState;
    
    
    // Default constructor, for the system's main celestial bodies such as planets
    public CelestialBody(string name, Star star, double distance, string imagePath, 
        DiscoveryStatus discoveryStatus = DiscoveryStatus.Undiscovered, 
        CelestialBodyType type = CelestialBodyType.DefaultPlanet){
        Name = name;
        Star = star;
        Distance = distance;
        DiscoveryStatus = discoveryStatus;
        _imagePath = imagePath;
        HasSatellites = true;
        Satellites = new List<CelestialBody>();
        IsSatellite = false;
        
        ActualState = new CelestialBodyState(type);
        KnownState = new CelestialBodyState();
        if (discoveryStatus == DiscoveryStatus.Explored)
        {
            KnownState.BodyType = type;
        }
    }

    // Constructor for Star
    public CelestialBody()
    {
        
        ActualState = new CelestialBodyState(CelestialBodyType.Star);
        KnownState = new CelestialBodyState(CelestialBodyType.Star);
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
        {
            ActualState = new CelestialBodyState(CelestialBodyType.Pallyria);
            KnownState = ActualState;
        }
        else
        {
            ActualState = new CelestialBodyState(CelestialBodyType.DefaultPlanet);
            KnownState = new CelestialBodyState();
        }
        
        HasSatellites = false;
        IsSatellite = false;
        
        ActualState = new CelestialBodyState();
        KnownState = new CelestialBodyState();
    }

    public void SetActualBodyType(CelestialBodyType type)
    {
        ActualState.BodyType = type;
    }
    public void SetKnownBodyType(CelestialBodyType type)
    {
        KnownState.BodyType = type;
    }
    public void HarmonizeBodyType()
    {
        KnownState.BodyType = ActualState.BodyType;
    }
    
    public CelestialBodyType GetActualBodyType()
    {
        return ActualState.BodyType;
    }
    public CelestialBodyType GetKnownBodyType()
    {
        return KnownState.BodyType;
    }

    /// <summary>
    /// Sets the amount of scientific potential of the celestial body
    /// </summary>
    /// <param name="amount">Can be anything but is clamped between 0 and 100</param>
    public void SetActualScientificPotential(double amount)
    {
        ActualState.SetScientificPotential(amount);
    }
    
    public void SetKnownScientificPotential(double amount)
    {
        KnownState.SetScientificPotential(amount);
    }
    
    public void HarmonizeScientificPotential()
    {
        KnownState.SetScientificPotential(ActualState.GetScientificPotential());
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
    }
    
    public void AddShipyard(Shipyard shipyard)
    {
        if (Shipyards.Count == 0) game_state.BodiesWithShipyards.Add(this);
        Shipyards.Add(shipyard);
    }
    
    /**Explore the body giving more information about it on the system map*/
    public void ExplorePlanet()
    {
        if (DiscoveryStatus == DiscoveryStatus.ExistenceKnown)
        {
            DiscoveryStatus = DiscoveryStatus.Explored;
            HarmonizeBodyType();
            
            var random = new Random();
            // The known science is randomized within 20% of actual value
            var actualScience = ActualState.GetScientificPotential();
            var max = actualScience + actualScience * 0.20;
            var min = actualScience - actualScience * 0.20;
            var knownScience = Math.Round(random.NextDouble() * (max  - min) + min, 2, MidpointRounding.AwayFromZero);
            SetKnownScientificPotential(knownScience);
        }            
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

    /// <summary>
    /// Perform some science on the celestial body (depletes scientific potential of the body).
    /// The amount used is randomized
    /// </summary>
    /// <returns>The amount of scientific potential that was extracted</returns>
    public double PerformScience()
    {
        var random = new Random();
        var max = 25;
        var amount = Math.Round(random.NextDouble() * max, 2, MidpointRounding.AwayFromZero);
        var potential = ActualState.GetScientificPotential();
        potential = Math.Round(potential - amount, 2, MidpointRounding.AwayFromZero);
        ActualState.SetScientificPotential(potential);
        if (potential >= 0)
        {
            return amount;
        }

        // if amount = 15 and _scientificPotential = 8, 8-15=-7, 15-(-7)=8
        var realExploitedAmount = amount - (-potential);
        ActualState.SetScientificPotential(0);
        return Math.Round(realExploitedAmount, 2, MidpointRounding.AwayFromZero);
    }

    public bool IsMissionCompatible(Mission mission)
    {
        switch (mission)
        {
            case PlanetExplorationMission:
                return (this is not global::Star) && DiscoveryStatus != DiscoveryStatus.Explored;
            case SystemExplorationMission: // Can still try to explore system even if everything is found
                return (this is Star);
            case PlanetaryExperiments:
                return (this is not global::Star);
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

    public string GetDescription()
    {
        var result = "";
        if (_customDescription != null)
        {
            result += _customDescription + "\n\n";
        }
        else
        {
            result += GetCelestialBodyTypeDesc() + "\n\n";
        }
        
        if (IsSatellite)
        {
            result += Name + " is orbiting " + ParentBody.Name + "\n";
            result += "Like it's parent body, it's orbiting " + Star.Name + " at a distance of " + ParentBody.Distance +
                      " light minutes" + "\n\n";
        }
        else
        {
            result += Name + " is orbiting " + Star.Name + " at a distance of " + Distance +
                      " light minutes" + "\n\n";
        }

        if (KnownState.BodyType is not (CelestialBodyType.Pallyria or CelestialBodyType.Earth))
        {
            result += GetDiscoveryStatusDesc() + "\n\n";
        }
        

        if (HasSatellites)
        {
            var knownSatellites = 
                Satellites.Count(
                    satellite => satellite.DiscoveryStatus is DiscoveryStatus.ExistenceKnown or DiscoveryStatus.Explored
                );
            if (knownSatellites == 1)
                result += Name + " has " + knownSatellites + " satellite around it that we know of" + "\n\n";
            else if (knownSatellites > 1)
                result += Name + " has " + knownSatellites + " satellites around it that we know of" + "\n\n";
        }

        if (KnownState.BodyType is not (CelestialBodyType.Pallyria or CelestialBodyType.Earth))
        {
            switch (KnownState.GetScientificPotential())
            {
                case -1:
                    result += "We don't know how valuable this planet could be for science yet" + "\n\n";
                    break;
                case < 1:
                    result += "This " + GetBodyTypeNameGeneralization() +
                              " appears to be [b][color=red]worthless[/color][/b] for science" + "\n\n";
                    break;
                case < 10:
                    result += "This " + GetBodyTypeNameGeneralization() +
                              " has [b][color=red]little[/color][/b] interest for science" + "\n\n";
                    break;
                case < 30:
                    result += "This " + GetBodyTypeNameGeneralization() +
                              " has [color=yellow]little[/color] interest for science" + "\n\n";
                    break;
                case < 70:
                    result += "This " + GetBodyTypeNameGeneralization() +
                              " has [color=green]a lot[/color] of scientific potential" + "\n\n";
                    break;
                case <= 100:
                    result += "This " + GetBodyTypeNameGeneralization() +
                              " has [b][color=green]extraordinary[/color][/b] value for science" + "\n\n";
                    break;
            }
        }
        

        return result;
    }
    

    public override string ToString()
    {
        var result = "";

        result += GetType().Name + ": " + Name + "\n";
        result += "Exploration status: " + DiscoveryStatus + "\n";
        result += "Image Path: " + _imagePath + "\n";
        
        return result;
    }
    
    
    
    
    private string GetDiscoveryStatusDesc()
    {
        return (DiscoveryStatus) switch
        {
            DiscoveryStatus.Undiscovered =>
                "It's undiscovered, you're not supposed seeing it, silly",
            DiscoveryStatus.ExistenceKnown =>
                "Existence of this body is almost certain",
            DiscoveryStatus.Explored =>
                "The surface of this body is well mapped",
            _ =>
                "Unknown status"
        };
    }
    
    private string GetCelestialBodyTypeDesc()
    {
        return KnownState.BodyType switch
        {
            CelestialBodyType.Pallyria => "This is Pallyria, home to our species",
            CelestialBodyType.Earth =>
                "This is a Pallyria-like planet, thriving with life and home to the Human civilization",
            CelestialBodyType.Luna =>
                "This big rocky moon doesn't look like much from afar but it has a big impact on the life of " +
                "Earth due to its sheer mass and proximity",
            CelestialBodyType.GasGiant =>
                "This giant body is mostly made of gas and could be a star if it had enough mass for fusion",
            CelestialBodyType.RingedGasGiant =>
                "This gas giant has a beautiful ring made of dust and sometimes bigger rocks around it",
            CelestialBodyType.IceGiant =>
                "This gas giant is made of heavier elements that traditional gas giants and can " +
                "have oceans beneath its clouds",
            CelestialBodyType.IcyPlanet => "This planet has extreme negative temperatures and is covered in ice",
            CelestialBodyType.MoltenPlanet =>
                "This planet is extremely hot and rivers of flowing lava can be seen from orbit",
            CelestialBodyType.ToxicPlanet =>
                "This rocky planet has a thick atmosphere made of gases toxic to traditional life",
            CelestialBodyType.RockyPlanet =>
                "This planet has a thin atmosphere and extreme temperatures " +
                "but in its base it's not much different from our home",
            CelestialBodyType.RockyMoon =>
                "This moon is essentially a giant round-shaped asteroid",
            CelestialBodyType.IcyMoon =>
                "This moon is covered in ice",
            CelestialBodyType.Asteroid =>
                "This is an asteroid, essentially a really big rock, sometimes a very expensive rock",
            CelestialBodyType.DefaultPlanet => "This is just your everyday planet",
            CelestialBodyType.Unknown => "We don't know the nature of this object",
            CelestialBodyType.UnknownMajorBody => "We don't know what kind of planet this is yet",
            CelestialBodyType.UnknownMinorBody => "We don't know what kind of celestial body this is yet",
            CelestialBodyType.Star => // This one isn't supposed to be used, for stars their class should be used
                "This is a star, apparently",
            _ => ""
        };
    }

    /// <returns> String "planet" or "moon" etc depending on celestial body type </returns>
    private string GetBodyTypeNameGeneralization()
    {
        switch (KnownState.BodyType)
        {
            case CelestialBodyType.Pallyria:
            case CelestialBodyType.Earth:
            case CelestialBodyType.ToxicPlanet:
            case CelestialBodyType.IcyPlanet:
            case CelestialBodyType.RockyPlanet:
            case CelestialBodyType.IceGiant:
            case CelestialBodyType.DefaultPlanet:
            case CelestialBodyType.MoltenPlanet: return "planet";
            case CelestialBodyType.Luna:
            case CelestialBodyType.RockyMoon:
            case CelestialBodyType.IcyMoon: return "moon";
            case CelestialBodyType.GasGiant:
            case CelestialBodyType.RingedGasGiant: return "gas giant";
            case CelestialBodyType.Asteroid: return "asteroid";
            case CelestialBodyType.Star: return "star";
            default: return "celestial object";
        }
    }
}

public class CelestialBodyState
{
    public CelestialBodyType BodyType;
    private double _scientificPotential;

    
    public CelestialBodyState(
        CelestialBodyType bodyType, 
        double scientificPotential = -1
        )
    {
        BodyType = bodyType;
        _scientificPotential = scientificPotential;
    }
    
    public CelestialBodyState()
    {
        BodyType = CelestialBodyType.Unknown;
        _scientificPotential = -1;
    }

    public void SetScientificPotential(double amount)
    {
        if (amount < 0) _scientificPotential = 0;
        else if (amount > 100) _scientificPotential = 100;
        else _scientificPotential = amount;
    }

    public double GetScientificPotential()
    {
        return _scientificPotential;
    }
    
}

public enum DiscoveryStatus
{
    Explored,
    ExistenceKnown,
    Undiscovered
}