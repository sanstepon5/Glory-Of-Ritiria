using System;
using System.Collections.Generic;
using System.IO;
using GloryOfRitiria.Scripts.Events;
using GloryOfRitiria.Scripts.ShipRelated;
using GloryOfRitiria.Scripts.ShipRelated.Missions;
using GloryOfRitiria.Scripts.StarSystem;
using GloryOfRitiria.Scripts.Utils;
using Godot;
using FileAccess = Godot.FileAccess;
using StarSystemInfo = GloryOfRitiria.Scripts.StarSystem.StarSystemInfo;

// This is a singleton that's loaded in every scene and will contain persistent state of the game.
// It should be empty on main screen a be loaded when loading a save.
namespace GloryOfRitiria.Scripts.Global;

public partial class game_state : Node
{
	/*Tutorial flags*/
	public static int CurrentPallyriaTutorial = 1;
	public static int CurrentSystemTutorial = 1;
	public static int CurrentInterstellarTutorial = 1;
	public static int CurrentShipyardTutorial = 1;
	public static int CurrentFleetBureauTutorial = 1;

	public static string AssetsDir;
	
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
	public static CelestialBody Pallyria;
	
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

	// TODO: Check if this is used
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


	
	public static Cargo FindCargo(string cargoName)
	{
		foreach (var cargo in CargoStorage)
		{
			if (cargo.Name.Equals(cargoName))
			{
				return cargo;
			}
		}

		return null;
	}	
	
	/** Finds cargo by name and removes it from storage*/
	public static Cargo PopCargo(string cargoName)
	{
		Cargo res = FindCargo(cargoName);
		CargoStorage.Remove(res);
		return res;
	}
	
	public static void AddCargo(Cargo cargo)
	{
		CargoStorage.Add(cargo);
	}
	
	public static void RemoveCargo(Cargo cargo)
	{
		CargoStorage.Remove(cargo);
	}

	public static void AddScientificRes(double amount)
	{
		ScientificRes += amount;
		if (ScientificRes < 0) ScientificRes = 0;
		if (ScientificRes > 100) ScientificRes = 100;
		_signals.EmitSignal(nameof(_signals.TopBarUpdateRequired));
	}
	public static void AddPoliticalRes(double amount)
	{
		PoliticalRes += amount;
		if (PoliticalRes < 0) PoliticalRes = 0;
		if (PoliticalRes > 100) PoliticalRes = 100;
		_signals.EmitSignal(nameof(_signals.TopBarUpdateRequired));
	}
	public static void AddMaterialRes(double amount)
	{
		Res1 += amount;
		if (Res1 < 0) Res1 = 0;

		_signals.EmitSignal(nameof(_signals.MaterialResChanged), Res1);
		_signals.EmitSignal(nameof(_signals.TopBarUpdateRequired));
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
		// ScientificRes = Math.Round((ScientificRes + random.NextDouble() * (maxRandomRes - minRandomRes) + minRandomRes), 2, MidpointRounding.AwayFromZero);
		
		if (ScientificRes < 0) ScientificRes = 0;
		if (PoliticalRes < 0) PoliticalRes = 0;
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


	private static void Init()
	{
		//TODO: Check for file not found
		var file = FileAccess.Open(AssetsDir + "StellarSystems.txt", FileAccess.ModeFlags.Read);
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
		Detnura = data.Detnura;
		

		// Cargo at the start of the game
		var planetExplorationKit = new Cargo("planet_exploration_kit");
		var spaceTelescope = new Cargo("space_telescope");
		var spaceTelescope2 = new Cargo("space_telescope");
		var beacon = new Cargo("system_beacon");

		CargoStorage.Add(planetExplorationKit);
		CargoStorage.Add(spaceTelescope);
		CargoStorage.Add(spaceTelescope2);
		CargoStorage.Add(beacon);
	}
	
}