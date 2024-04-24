using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using GloryOfRitiria.Scripts.ShipRelated;
using GloryOfRitiria.Scripts.ShipRelated.Missions;
using GloryOfRitiria.Scripts.Utils;
using GloryOfRitiria.Scripts.Utils.Events;
using Godot;
using FileAccess = Godot.FileAccess;

// This is a singleton that's loaded in every scene and will contain persistent state of the game.
// It should be empty on main screen a be loaded when loading a save.
namespace GloryOfRitiria.Scripts.Global;

public partial class game_state : Node
{

	public static string AssetsDir;
	
	public static string CurrentYear { set; get; } = "970 APE\n(2017 CE)";
	public static int CurrentTurn { set; get; } = 0;

	// Resources
	public static double PoliticalRes { set; get; } = 0;
	public static double ScientificRes { set; get; } = 0;
	public static double Res1 { set; get; } = 0;
	public static double Res1Rate { set; get; } = 1.5;
	
	// Events
	public static List<Flags> CurrentFlags = new();
	public static List<GameEvent> EventsForTurn = new();
	
	// Has stars of the current star system. Should be empty when
	// Current scene isn't a star system
	public static List<Star> SelectedStarSystem = new();
	public static StarSystemInfo Detnura;
	
	/// <summary> List of star system that can be viewed by the player</summary>
	public static List<StarSystemInfo> DiscoveredSystems = new();
	public static List<StarSystemInfo> AllSystems;
	
	
	// public static ShipyardList ShipConstructionSlots = new();
	public static List<Ship> AllShips = new();
	public static List<Cargo> CargoStorage = new();
	
	public static List<CelestialBody> BodiesWithShipyards = new();

	public static Ship SelectedShip = null;
	


	private static GlobalSignals _signals;
	
	// Attributes
	public static Dictionary<string, Tuple<Type, object>> GetAttributeValues()
	{
		var attributeValues = new Dictionary<string, Tuple<Type, object>>
		{
			{ "CurrentYear", Tuple.Create(typeof(string), (object)CurrentYear) },
			{ "CurrentTurn", Tuple.Create(typeof(int), (object)CurrentTurn) },
			{ "Res1", Tuple.Create(typeof(double), (object)Res1) },
			{ "Res1Rate", Tuple.Create(typeof(double), (object)Res1Rate) }
			// Add other attributes as needed...
		};

		return attributeValues;
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (OS.HasFeature("editor")) AssetsDir = "res://Assets/";
		// For built project
		else AssetsDir = OS.GetExecutablePath().GetBaseDir() + "/Assets/";

		Init();
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
	}

	public static void DiscoverPlanet(string planetName)
	{
		//TODO: transform into while and print error if not found
		foreach (var starSystem in DiscoveredSystems)
		{
			foreach (var star in starSystem.SystemStars)
			{
				foreach (var body in star.Bodies)
				{
					if (body.Name.Equals(planetName))
					{
						body.ExplorePlanet();
					}
				}
			}
		}
	}
	
	public static void DiscoverSystem(Star star)
	{
		foreach (var body in star.Bodies)
        {
            body.DiscoverBody();
        }
	}

	public static Cargo PopCargo(string cargoName)
	{
		Cargo res = null;
		var found = false;
		var i = 0;
		while (!found)
		{
			if (CargoStorage[i].Name.Equals(cargoName))
			{
				found = true;
				res = CargoStorage[i];
			}
			i++;
		}

		CargoStorage.Remove(res);
		return res;
	}

	public static void UpdateResources()
	{
		var random = new Random();
		var maxRandomRes = 10;
		var minRandomRes = -5;
		
		// Res 1
		Res1 += Res1Rate;
		//if (Res1 < 0) Res1 = 0;
		
		// Just random for the moment
		PoliticalRes = Math.Round((PoliticalRes + random.NextDouble() * (maxRandomRes - minRandomRes) + minRandomRes), 2, MidpointRounding.AwayFromZero);
		ScientificRes = Math.Round((ScientificRes + random.NextDouble() * (maxRandomRes - minRandomRes) + minRandomRes), 2, MidpointRounding.AwayFromZero);
		
		if (ScientificRes < 0) ScientificRes = 0;
		if (PoliticalRes < 0) PoliticalRes = 0;
	}
	
	public static void SetCurrentYear()
	{
		// For now
		CurrentYear = "9" + (70 + CurrentTurn) + " APE\n(20" + (17 + CurrentTurn) + " CE)";
	}

	public static void UpdateShipConstruction()
	{
		foreach (var body in BodiesWithShipyards)
		{
			foreach (var shipyard in body.Shipyards)
			{
				if (shipyard.Update())
                {
                	_signals.EmitSignal(nameof(_signals.ShipFinishedBuilding));
                }
			}
			
		}
	}
	
	public static void UpdateActiveShips()
	{
		foreach (var ship in AllShips)
		{
			if (ship.MovementUpdate()) // if ship changed location
			{
				_signals.EmitSignal(nameof(_signals.ShipMoved));
			}
		}
	}



	// tmp function to create test star systems, until I make an actual parser and stars file
	public static void Init()
	{
		//TODO: Check for file not found
		var file = FileAccess.Open(AssetsDir + "StellarSystems.txt", FileAccess.ModeFlags.Read);
		//var fileStream = new StreamReader(AssetsDir + "StellarSystems.txt");
		var stellarSystemsParser = new StellarSystemsParser(new StringReader(file.GetAsText()));
		var data = stellarSystemsParser.Parse();
		AllSystems = data.Systems;
		foreach (var system in AllSystems)
		{
			foreach (var star in system.SystemStars)
			{
				if (star.DiscoveryStatus is DiscoveryStatus.Explored or DiscoveryStatus.ExistenceKnown)
				{
					DiscoveredSystems.Add(system);
					break;
				}
			}
		}
		AllShips = data.Ships;
		//BodiesWithShipyards = data.BodiesWithShipyards;
		Detnura = data.Detnura;
		
		// This approach in theory doubles memory consumption for a brief moment when both parser and game_state are full of data
		// DiscoveredSystems = stellarSystemsParser.Systems;
		// AllShips = stellarSystemsParser.Ships;
		// BodiesWithShipyards = stellarSystemsParser.BodiesWithShipyards;
		// Detnura = stellarSystemsParser.Detnura; // I don't like this but for now it'll do
		
		
		// Init star systems
		/*
		var solSystem = new StarSystemInfo("Sol", 4.22f, 30, StarSystemType.Sun);
		var barnardSystem = new StarSystemInfo("Barnard's star", 7.82f, 170);
		var detnuraSystem = new StarSystemInfo("Detnura-Aeria System", 0, 0, StarSystemType.Detnura);
		Detnura = detnuraSystem;
		DiscoveredSystems.Add(detnuraSystem);
		DiscoveredSystems.Add(solSystem);
		DiscoveredSystems.Add(barnardSystem);
		
		
		// Detnura
		var detnuraStar = new Star("Detnura", detnuraSystem, 700, StarClass.OrangeDwarf);

		// Detnura planets
		var pallyria = new CelestialBody("Pallyria", detnuraStar, 5, 
			AssetsDir + "Img/tmp/PlanetIcon.png", DiscoveryStatus.Explored, CelestialBodyType.Pallyria);
		pallyria.AddShipyard("Sokhil Shipyard");
		pallyria.AddShipyard("Eradian Shipyard");
		detnuraStar.AddCelestialBody(pallyria);
		
		detnuraStar.AddCelestialBody(new CelestialBody("Other Planet",  detnuraStar, 15,AssetsDir+"Img/tmp/CelestialBodies/icePlanet.png", DiscoveryStatus.Explored));
		
		// Gas giant that has satellites
		var gasGiant = new CelestialBody("Gas Giant", detnuraStar, 50, AssetsDir + "Img/tmp/CelestialBodies/gasGiant.png", DiscoveryStatus.Explored);
		var moon = new CelestialBody("Moon", detnuraStar, 0, AssetsDir + "Img/tmp/CelestialBodies/icePlanet.png", true, true, DiscoveryStatus.Explored);
		
		//var shipGroup = new ShipGroup("Fleet 1", AssetsDir + "Icons/shipGroup.png");
		//moon.AddShipGroup(shipGroup);
		gasGiant.AddSatellite(moon);
		gasGiant.AddSatellite(new CelestialBody("Moon 2", detnuraStar, 0, AssetsDir + "Img/tmp/CelestialBodies/icePlanet.png", true, true, DiscoveryStatus.Explored));
		detnuraStar.AddCelestialBody(gasGiant);
		
		var asteroid = new CelestialBody("Asteroid", detnuraStar, 65, AssetsDir + "Img/tmp/CelestialBodies/asteroid.png", true, true, DiscoveryStatus.Explored, CelestialBodyType.MinorBody);
		detnuraStar.AddCelestialBody(asteroid);
		
		detnuraSystem.SystemStars.Add(detnuraStar);
		
		
		// Sol
		var sunStar = new Star("Sun", solSystem, 500, StarClass.YellowDwarf);
		sunStar.AddCelestialBody(new CelestialBody("Mercury", sunStar, 1, AssetsDir+"Img/tmp/MoltenPlanet.png", DiscoveryStatus.ExistenceKnown));
		sunStar.AddCelestialBody(new CelestialBody("Venus", sunStar, 4, AssetsDir+"Img/tmp/MoltenPlanet.png", DiscoveryStatus.ExistenceKnown));
		var earth = new CelestialBody("Earth", sunStar, 7, AssetsDir + "Img/tmp/CelestialBodies/liveablePlanet.png", DiscoveryStatus.Explored);
		sunStar.AddCelestialBody(earth);
		
		solSystem.SystemStars.Add(sunStar);
		
		// Barnard's star
		var barnardStar = new Star("Barnard's star", barnardSystem, 20);
		barnardStar.AddCelestialBody(new CelestialBody("Something", barnardStar, 1, AssetsDir+"Img/tmp/CelestialBodies/icePlanet.png"));
		barnardSystem.SystemStars.Add(barnardStar);
		
		
		// Ships at the start of the game
		var columbia = new Ship("Columbia", earth, true);
		var planetExplorationKit = new Cargo("planet_exploration_kit", 5);
		planetExplorationKit.AddMission(new PlanetExplorationMission());
		columbia.ShipCargo.Add(planetExplorationKit);
		earth.AddBusyShipyard("UNOOSA European Dockyard", columbia, 7);
		
		var irana = new Ship("Irana", pallyria);
		var spaceTelescope = new Cargo("space_telescope", 1);
		spaceTelescope.AddMission(new SystemExplorationMission());
		irana.ShipCargo.Add(spaceTelescope);
		
		AllShips.Add(irana);
		
		*/
		// Cargo at the start of the game
		var planetExplorationKit2 = new Cargo("Planet exploration kit", 5);
		planetExplorationKit2.AddMission(new PlanetExplorationMission());
		var spaceTelescope2 = new Cargo("Space Telescope", 1);
		spaceTelescope2.AddMission(new SystemExplorationMission());
		var spaceTelescope3 = new Cargo("Space Telescope", 1);
		spaceTelescope3.AddMission(new SystemExplorationMission());
		CargoStorage.Add(planetExplorationKit2);
		CargoStorage.Add(spaceTelescope2);
		CargoStorage.Add(spaceTelescope3);
	}
	
}