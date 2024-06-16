using System.Collections.Generic;
using GloryOfRitiria.Scenes.Parts.SystemMap;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.StarSystem;
using Godot;
using Ship = GloryOfRitiria.Scripts.ShipRelated.Ship;
using StarSystemInfo = GloryOfRitiria.Scripts.StarSystem.StarSystemInfo;


// VBox container for all bodies. All bodies have the same stretch ratio (5 for example). 
// Star's ratio is 1 * nb of celestial bodies to keep the same size. 

namespace GloryOfRitiria.Scripts.Scenes;

public partial class star_system_view : Node2D
{
	private GlobalSignals _signals;

	private int _currentStarIndex = 0;

	private StarSystemInfo _currentSystem;

	private List<Ship> _shipsInSystem;

	// In pixels
	[Export] public int InterBodiesDistance = 100;
	[Export] public int OrbitLineSize = 10;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_shipsInSystem = new List<Ship>();
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");

		_signals.Connect(nameof(_signals.DetnuraBuildRequested), new Callable(this, nameof(BuildDetnuraSystem)));
		_signals.Connect(nameof(_signals.StarViewBuildRequested), new Callable(this, nameof(BuildMap)));
		_signals.Connect(nameof(_signals.ShipFinishedBuilding), new Callable(this, nameof(ResetSystem)));
		_signals.Connect(nameof(_signals.ShipMoved), new Callable(this, nameof(ResetSystem)));
		_signals.Connect(nameof(_signals.ShipClicked), new Callable(this, nameof(DeselectOtherShips)));
	}

	// Should somehow add all the ships and stuff that were created that turn
	public void ResetSystem()
	{
		_shipsInSystem = new List<Ship>();

		var bodiesCont = GetNode<HBoxContainer>("BodiesHBox");
		foreach (var child in bodiesCont.GetChildren())
		{
			child.QueueFree();
		}

		// Clear all ships in inner space
		var innerShipsHBox = GetNode<HBoxContainer>("InnerSpaceVBox/CenterCont/HBox");
		for (var i = 0; i < innerShipsHBox.GetChildren().Count; i++)
		{
			var child = innerShipsHBox.GetChild(i);
			child.QueueFree();
		}

		// Rebuild system map
		BuildMap(_currentSystem);
		GD.Print("System view updated");
	}


	// Loads planets of Detnura System
	public void BuildDetnuraSystem()
	{
		GD.Print("Detnura Entered");
		BuildMap(game_state.Detnura);
	}

	public void BuildMap( /*Star star*/ StarSystemInfo starSystem)
	{
		GD.Print("System Map Build Initiated");
		GetNode<RichTextLabel>("SystemName").Text = starSystem.SystemName;
		_currentSystem = starSystem;

		// TODO: Manage multiple stars
		List<Star> stars = starSystem.SystemStars;
		var star = stars[_currentStarIndex]; // always 0 for now

		
		var starCenter = GetNode<Node2D>("Star");
		var starInst = BuildCelestialBodyInst(star);

		starCenter.AddChild(starInst);
		starInst.Position = new Vector2(-(starInst.Size.X / 2), -(starInst.ImageSize.Y / 2));

		// Adding bodies horizontally
		if (star.Bodies.Count > 0)
		{
			
			var previousPosition = starInst.Position;
			var previousSize = starInst.Size;

			// Bodies are present, add horizontal line
			var horizontalLine = CreateOrbitalLine(vertical: false);
			starCenter.AddChild(horizontalLine);
			
			// Call build satellites method here
			AddSatellites(starCenter, starInst, horizontalLine, vertical: false);

			foreach (var body in star.Bodies)
			{
				if (body.DiscoveryStatus == DiscoveryStatus.Undiscovered) continue;

				var bodyCenter = new Node2D();
				starCenter.AddChild(bodyCenter);

				var bodyInst = BuildCelestialBodyInst(body);
				bodyCenter.AddChild(bodyInst);

				// Now that we know the size, we can find its center position
				bodyCenter.Position = new Vector2(previousPosition.X + previousSize.X + InterBodiesDistance +
												  (bodyInst.Size.X / 2), -(bodyInst.ImageSize.Y / 2) - 5);

				// TODO: Take horizontal satellites into account when calculating distance
				// between horizontal bodies higher. Maybe give the center point a size or something like that. 

				bodyInst.Position = new Vector2(-(bodyInst.ImageSize.X / 2), 0);

				// Increase line length until the center point of the body
				horizontalLine.Size = new Vector2(bodyCenter.Position.X, OrbitLineSize);

				// Add vertical satellites
				if (body.HasSatellites && !body.IsSatellite)
				{
					var verticalLine = CreateOrbitalLine(vertical: true);
					bodyCenter.AddChild(verticalLine);
					foreach (var satellite in body.Satellites)
					{

					}

					// Add ships
					foreach (var ship in body.ShipsInOrbit)
					{

					}
				}

				previousPosition = bodyCenter.Position;
				previousSize = bodyInst.Size;
			}
		}

		// Add ships vertically to the star (to the star center)
		if (star.ShipsInOrbit.Count > 0)
		{
			var previousPosition = starInst.Position;
			var previousSize = starInst.Size;

			// Ships are present, add vertical line
			var verticalLine = CreateOrbitalLine(vertical: true);
			starCenter.AddChild(verticalLine);

			foreach (var ship in star.ShipsInOrbit)
			{
				var shipInst = BuildShipInst(ship);

				// Set scene position
				shipInst.Position = new Vector2(-(shipInst.Size.X / 2),
					previousPosition.Y + previousSize.Y + InterBodiesDistance);

				previousPosition = shipInst.Position;
				previousSize = shipInst.Size;

				// Increase line length
				verticalLine.Size += new Vector2(0, previousSize.Y);

				starCenter.AddChild(shipInst);
			}
		}



		var emptyImg = GetNode<TextureRect>("InnerSpaceVBox/CenterCont/TextureRect");
		var innerShipsHBox = GetNode<HBoxContainer>("InnerSpaceVBox/CenterCont/HBox");
		var innerShipsLabel = GetNode<Label>("InnerSpaceVBox/Label");
		if (star.InnerSpace.ShipsInOrbit.Count == 0)
		{
			emptyImg.Visible = true;
			innerShipsHBox.Visible = false;
			innerShipsLabel.Text = "Inner Space - No ships in transit";
		}
		else
		{
			innerShipsHBox.Visible = true;
			emptyImg.Visible = false;
			foreach (var ship in star.InnerSpace.ShipsInOrbit)
			{
				_shipsInSystem.Add(ship);
				innerShipsHBox.AddChild(BuildShipInst(ship));
			}

			innerShipsLabel.Text = $"Inner Space - {star.InnerSpace.ShipsInOrbit.Count} ships in transit";
		}
	}

	private void AddSatellites(Node2D parentCenter, CBOnSystemMap parent, ColorRect orbitLine, bool vertical = false)
	{
		var previousPosition = parent.Position;
		var previousSize = parent.Size;
	}

	private void AddShipsInOrbit(Node2D parent, ColorRect orbitLine, bool vertical = false)
	{

	}

	private ColorRect CreateOrbitalLine(bool vertical = false)
	{
		var line = new ColorRect();
		line.Color = new Color(1f, 1f, 1f, 0.7f); // Slightly transparent white line
		// Line size should be set later increasing for each body
		line.ZIndex = -1;
		// The line is before everything else in the hierarchy so we need to ignore clicks on it
		line.MouseFilter = Control.MouseFilterEnum.Ignore;

		line.Position = vertical ? new Vector2(-(OrbitLineSize / 2), 0) : new Vector2(0, -(OrbitLineSize / 2));
		return line;
	}

	private ShipOnSystemMap BuildShipInst(Ship ship)
	{
		_shipsInSystem.Add(ship);

		var shipScene = GD.Load<PackedScene>("res://Scenes/Parts/SystemMap/ShipOnSystemMap.tscn");
		var shipInst = (ShipOnSystemMap)shipScene.Instantiate();
		shipInst.Init(ship);

		return shipInst;
	}

	private CBOnSystemMap BuildCelestialBodyInst(CelestialBody body)
	{
		var scene = GD.Load<PackedScene>("res://Scenes/Parts/SystemMap/CBOnSystemMap.tscn");
		var inst = (CBOnSystemMap)scene.Instantiate();
		if (body is Star)
			inst.Init(body, isStar: true);
		else
			inst.Init(body);

		return inst;
	}

	public void DeselectOtherShips(Ship ship)
	{
		foreach (var otherShip in _shipsInSystem)
		{
			if (ship == otherShip) continue;
			otherShip.Selected = false;
			otherShip.SimpleUpdate();
		}
	}
}
	
