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
	
	public static Dictionary<string, List<Star>> AllStarSystems = new();
	
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

		AddStarSystems();
		
		
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


	// tmp function to create test star systems, until I make an actual parser and stars file
	public static void AddStarSystems()
	{
		// Detnura
		var detnuraSystem = new List<Star>();
		var detnuraStar = new Star("Detnura", AssetsDir+"Img/tmp/CelestialBodies/star2.png");
		
		detnuraStar.AddCelestialBody(new CelestialBody("Pallyria", 10, AssetsDir+"Img/tmp/PlanetIcon.png" , CelestialBodyType.Pallyria));
		
		detnuraStar.AddCelestialBody(new CelestialBody("Other Planet", 20, AssetsDir+"Img/tmp/CelestialBodies/icePlanet.png"));
		
		// Gas giant that has satellites
		var gasGiant = new CelestialBody("Gas Giant", 30, AssetsDir + "Img/tmp/CelestialBodies/gasGiant.png");
		var moon = new CelestialBody("Moon", 0, AssetsDir + "Img/tmp/CelestialBodies/icePlanet.png", true, true);
		
		var shipGroup = new ShipGroup("Fleet 1", 0, true, AssetsDir + "Icons/shipGroup.png");
		moon.AddSatellite(shipGroup);
		gasGiant.AddSatellite(moon);
		gasGiant.AddSatellite(new CelestialBody("Moon 2", 0, AssetsDir + "Img/tmp/CelestialBodies/icePlanet.png", true, true));
		detnuraStar.AddCelestialBody(gasGiant);
		
		var asteroid = new MinorBody("Asteroid", 0, AssetsDir + "Img/tmp/CelestialBodies/asteroid.png", false);
		detnuraStar.AddCelestialBody(asteroid);
		
		detnuraSystem.Add(detnuraStar);
		
		
		// Sun
		var solSystem = new List<Star>();
		var sunStar = new Star("Sun", AssetsDir+"Img/tmp/RedStar64.png");
		sunStar.AddCelestialBody(new CelestialBody("Mercury", 10, AssetsDir+"Img/tmp/MoltenPlanet.png"));
		sunStar.AddCelestialBody(new CelestialBody("Venus", 20, AssetsDir+"Img/tmp/MoltenPlanet.png"));
		sunStar.AddCelestialBody(new CelestialBody("Earth", 30, AssetsDir+"Img/tmp/CelestialBodies/liveablePlanet.png"));
		solSystem.Add(sunStar);
		
		// Barnard's star
		var barnardSystem = new List<Star>();
		var barnardStar = new Star("Barnard's star", AssetsDir+"Img/tmp/RedStar64.png");
		barnardStar.AddCelestialBody(new CelestialBody("Something", 10, AssetsDir+"Img/tmp/CelestialBodies/icePlanet.png"));
		barnardSystem.Add(barnardStar);
		
		// TODO: I really need to redo all names and acces for stars/star sustems
		AllStarSystems.Add("Detnura", detnuraSystem);
		AllStarSystems.Add("Sun", solSystem);
		AllStarSystems.Add("Barnard's star", barnardSystem);
	}
	
}