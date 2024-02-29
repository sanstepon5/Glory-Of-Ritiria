using System;
using System.Collections.Generic;
using GloryOfRitiria.Scripts.Utils;
using GloryOfRitiria.Scripts.Utils.Events;
using Godot;

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
	
	/// <summary> List of star system that can be viewed by the player</summary>
	public static List<StarSystemInfo> DiscoveredSystems = new();

	public static StarSystemInfo Detnura;
	
	
	// public static ShipyardList ShipConstructionSlots = new();
	public static List<Ship> AllShips = new();
	
	public static List<CelestialBody> BodiesWithShipyards = new();

	public static Ship SelectedShip = null;
	
	
	
	
	
	public static GlobalSignals _signals;
	
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
		// Init star systems
		var solSystem = new StarSystemInfo("Sol", 4.22f, 30, StarSystemType.Sun);
		var barnardSystem = new StarSystemInfo("Barnard's star", 7.82f, 170);
		var detnuraSystem = new StarSystemInfo("Detnura-Aeria System", 0, 0, StarSystemType.Detnura);
		Detnura = detnuraSystem;
		DiscoveredSystems.Add(detnuraSystem);
		DiscoveredSystems.Add(solSystem);
		DiscoveredSystems.Add(barnardSystem);
		
		
		// Detnura
		var detnuraStar = new Star("Detnura", detnuraSystem, 700,AssetsDir+"Img/tmp/CelestialBodies/star2.png");

		// Detnura planets
		var pallyria = new CelestialBody("Pallyria", detnuraStar, 5, AssetsDir + "Img/tmp/PlanetIcon.png",
			CelestialBodyType.Pallyria);
		pallyria.AddShipyard("Sokhil Shipyard");
		detnuraStar.AddCelestialBody(pallyria);
		
		detnuraStar.AddCelestialBody(new CelestialBody("Other Planet",  detnuraStar, 15,AssetsDir+"Img/tmp/CelestialBodies/icePlanet.png"));
		
		// Gas giant that has satellites
		var gasGiant = new CelestialBody("Gas Giant", detnuraStar, 50, AssetsDir + "Img/tmp/CelestialBodies/gasGiant.png");
		var moon = new CelestialBody("Moon", detnuraStar, 0, AssetsDir + "Img/tmp/CelestialBodies/icePlanet.png", true, true);
		
		//var shipGroup = new ShipGroup("Fleet 1", AssetsDir + "Icons/shipGroup.png");
		//moon.AddShipGroup(shipGroup);
		gasGiant.AddSatellite(moon);
		gasGiant.AddSatellite(new CelestialBody("Moon 2", detnuraStar, 0, AssetsDir + "Img/tmp/CelestialBodies/icePlanet.png", true, true));
		detnuraStar.AddCelestialBody(gasGiant);
		
		var asteroid = new CelestialBody("Asteroid", detnuraStar, 65, AssetsDir + "Img/tmp/CelestialBodies/asteroid.png", true, true, CelestialBodyType.MinorBody);
		detnuraStar.AddCelestialBody(asteroid);
		
		detnuraSystem.SystemStars.Add(detnuraStar);
		
		
		// Sol
		var sunStar = new Star("Sun", solSystem, 500, AssetsDir+"Img/tmp/RedStar64.png");
		sunStar.AddCelestialBody(new CelestialBody("Mercury", sunStar, 1, AssetsDir+"Img/tmp/MoltenPlanet.png"));
		sunStar.AddCelestialBody(new CelestialBody("Venus", sunStar, 4, AssetsDir+"Img/tmp/MoltenPlanet.png"));
		var earth = new CelestialBody("Earth", sunStar, 7, AssetsDir + "Img/tmp/CelestialBodies/liveablePlanet.png");
		earth.AddShipyard("UNOOSA European Dockyard");
		sunStar.AddCelestialBody(earth);
		
		solSystem.SystemStars.Add(sunStar);
		
		// Barnard's star
		var barnardStar = new Star("Barnard's star", barnardSystem, 20, AssetsDir+"Img/tmp/RedStar64.png");
		barnardStar.AddCelestialBody(new CelestialBody("Something", barnardStar, 1, AssetsDir+"Img/tmp/CelestialBodies/icePlanet.png"));
		barnardSystem.SystemStars.Add(barnardStar);
		
		
		
		var irana = new Ship("Irana", earth);
		AllShips.Add(irana);
		
		var baraba = new Ship("Baraba", pallyria, true);
		pallyria.AddBusyShipyard("Eradian Shipyard", baraba, 7);
	}
	
}