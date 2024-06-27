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
    private double _distance;

    public DiscoveryStatus DiscoveryStatus;
    
    
    
    // Determines if body should display satellites
    public bool HasSatellites;
    // Determines if bodies' satellites will be displayed vertically or horizontally
    public bool IsSatellite;
    public List<CelestialBody> Satellites;
    public List<Ship> ShipsInOrbit = new(); //public ShipGroup ShipsInOrbit;
    
    // Random planet, some specific planet, asteroid, station....
    protected string ImagePath; 
    // Later the size could be deduced from other parameters but right now it fully depends on the image
    public float SizeFactor = 1.0f;

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
        _distance = distance;
        DiscoveryStatus = discoveryStatus;
        ImagePath = imagePath;
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
        _distance = 0;
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

    public void SetDistance(double dist)
    {
        _distance = Math.Round(dist, 2, MidpointRounding.AwayFromZero);
    }
    
    public double GetDistance()
    {
        return _distance;
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

    public void UpdateKnownState(CelestialBodyState state)
    {
        
    }
    
    private string GetRandomFilename(string dirName)
    {
        var r = new Random();
        var dir = DirAccess.Open(game_state.AssetsDir + "Img/tmp/CelestialBodies/"+ dirName + "/");
        var files = new List<String>();
        foreach (var filename in dir.GetFiles())
        {
            if (filename.Contains(".png") && !filename.Contains("import"))
            {
                files.Add(filename);
            }
        }
        var fileIndex = r.Next(files.Count);
        return files[fileIndex];
    }

    public virtual void SetImagePath()
    {
        var r = new Random();
        var (minimum,  maximum) = (1.0f, 1.0f);
        ImagePath = game_state.AssetsDir + "Img/tmp/CelestialBodies/UndiscoveredPlanet.png";
        switch (ActualState.BodyType)
        {   // TODO: Randomize image size
            case CelestialBodyType.Pallyria:
                ImagePath = game_state.AssetsDir + "Img/tmp/CelestialBodies/Pallyria.png";
                SizeFactor = 0.7f;
                break;
            case CelestialBodyType.Earth:
                ImagePath = game_state.AssetsDir + "Img/tmp/CelestialBodies/Green/GreenPlanet.png"; 
                SizeFactor = 0.75f;
                break;
            case CelestialBodyType.RingedGasGiant:
                ImagePath = game_state.AssetsDir + "Img/tmp/CelestialBodies/RingedGasGiant/" 
                                                  + GetRandomFilename("RingedGasGiant");
                (minimum,  maximum) = (1.5f, 2.1f);
                SizeFactor = (float)(r.NextDouble() * (maximum - minimum) + minimum);
                break;
            case CelestialBodyType.GasGiant:
                ImagePath = game_state.AssetsDir + "Img/tmp/CelestialBodies/GasGiant/"
                                                  + GetRandomFilename("GasGiant");
                (minimum,  maximum) = (1.2f, 1.4f);
                SizeFactor = (float)(r.NextDouble() * (maximum - minimum) + minimum);
                break;
            case CelestialBodyType.MoltenPlanet:
                ImagePath = game_state.AssetsDir + "Img/tmp/CelestialBodies/Molten/" 
                                                  + GetRandomFilename("Molten");
                (minimum,  maximum) = (0.5f, 0.8f);
                SizeFactor = (float)(r.NextDouble() * (maximum - minimum) + minimum);
                break;
            case CelestialBodyType.RockyMoon: 
                (minimum,  maximum) = (0.1f, 0.2f);
                SizeFactor = (float)(r.NextDouble() * (maximum - minimum) + minimum);
                break;
            case CelestialBodyType.Luna:
                ImagePath = game_state.AssetsDir + "Img/tmp/CelestialBodies/Moon/" 
                                                  + GetRandomFilename("Moon");
                (minimum,  maximum) = (0.3f, 0.4f);
                SizeFactor = (float)(r.NextDouble() * (maximum - minimum) + minimum);
                break;
            case CelestialBodyType.RockyPlanet:
                ImagePath = game_state.AssetsDir + "Img/tmp/CelestialBodies/Red/" 
                                                  + GetRandomFilename("Red");
                (minimum,  maximum) = (0.4f, 0.8f);
                SizeFactor = (float)(r.NextDouble() * (maximum - minimum) + minimum);
                break;
            case CelestialBodyType.IcyPlanet:
                ImagePath = game_state.AssetsDir + "Img/tmp/CelestialBodies/IcePlanet/" 
                                                  + GetRandomFilename("IcePlanet");
                (minimum,  maximum) = (0.4f, 0.7f);
                SizeFactor = (float)(r.NextDouble() * (maximum - minimum) + minimum);
                break;
            case CelestialBodyType.IcyMoon:
                ImagePath = game_state.AssetsDir + "Img/tmp/CelestialBodies/IcePlanet/" 
                                                  + GetRandomFilename("IcePlanet");
                (minimum,  maximum) = (0.2f, 0.4f);
                SizeFactor = (float)(r.NextDouble() * (maximum - minimum) + minimum);
                break;
            case CelestialBodyType.IceGiant:
                ImagePath = game_state.AssetsDir + "Img/tmp/CelestialBodies/IceGiant/" 
                                                  + GetRandomFilename("IceGiant");
                (minimum,  maximum) = (1.0f, 1.2f);
                SizeFactor = (float)(r.NextDouble() * (maximum - minimum) + minimum);
                break;
            case CelestialBodyType.ToxicPlanet:
                ImagePath = game_state.AssetsDir + "Img/tmp/CelestialBodies/White/" 
                                                  + GetRandomFilename("White");
                (minimum,  maximum) = (0.6f, 0.8f);
                SizeFactor = (float)(r.NextDouble() * (maximum - minimum) + minimum);
                break;
            case CelestialBodyType.DefaultPlanet:
                ImagePath = game_state.AssetsDir + "Img/tmp/CelestialBodies/Red/" 
                                                  + GetRandomFilename("Red");
                (minimum,  maximum) = (1.0f, 1.0f);
                SizeFactor = (float)(r.NextDouble() * (maximum - minimum) + minimum);
                break;
            case CelestialBodyType.Asteroid:
                ImagePath = game_state.AssetsDir + "Img/tmp/CelestialBodies/Asteroid/" + GetRandomFilename("Asteroid");
                (minimum,  maximum) = (0.3f, 0.5f);
                SizeFactor = (float)(r.NextDouble() * (maximum - minimum) + minimum);
                break;
        }
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
            // The known science is randomized within 35% of actual value
            var actualScience = ActualState.GetScientificPotential();
            var max = actualScience + actualScience * 0.35;
            var min = actualScience - actualScience * 0.35;
            var knownScience = Math.Round(random.NextDouble() * (max  - min) + min, 2, MidpointRounding.AwayFromZero);
            SetKnownScientificPotential(knownScience);
        }

        if (DiscoveryStatus == DiscoveryStatus.Explored) // More precise information if already explored
        {
            HarmonizeBodyType();
            
            var random = new Random();
            // The known science is randomized within 10% of actual value
            var actualScience = ActualState.GetScientificPotential();
            var max = actualScience + actualScience * 0.10;
            var min = actualScience - actualScience * 0.10;
            var knownScience = Math.Round(random.NextDouble() * (max  - min) + min, 2, MidpointRounding.AwayFromZero);
            SetKnownScientificPotential(knownScience);
        }
        else
            GD.Print("A ship tried exploring unknown body. How? : " + Name);
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
                return (this is not global::Star); //&& DiscoveryStatus != DiscoveryStatus.Explored;
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

    /**
     * Get the image selected for this Celestial Body (fixed "undiscoveredPlanet texture if planet not explored)
     */
    public virtual string GetImage()
    {
        if (DiscoveryStatus == DiscoveryStatus.Explored) return ImagePath;
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
            result += "Like it's parent body, it's orbiting " + Star.Name + " at a distance of " + ParentBody._distance +
                      " light minutes" + "\n\n";
        }
        else
        {
            result += Name + " is orbiting " + Star.Name + " at a distance of " + _distance +
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
        result += "Image Path: " + ImagePath + "\n";
        
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
    
    public CelestialBodyState(CelestialBodyState stateToCopy)
    {
        BodyType = stateToCopy.BodyType;
        _scientificPotential = stateToCopy.GetScientificPotential();
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