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

		var allBodies = GetNode<Control>("Star");
		foreach (var child in allBodies.GetChildren())
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

		
		var starCenter = GetNode<Control>("Star");
		var starInst = BuildCelestialBodyInst(star);

		starCenter.AddChild(starInst);
		starInst.Position = new Vector2(-(starInst.Size.X / 2), -(starInst.ImageSize.Y / 2));
		
		// Lines are created with 0 size so they don't appear
		var horizontalLine = CreateOrbitalLine(vertical: false);
		starCenter.AddChild(horizontalLine);
		var verticalLine = CreateOrbitalLine(vertical: true);
		starCenter.AddChild(verticalLine);

		// Adding bodies horizontally
		if (star.Bodies.Count > 0)
		{
			// Build bodies of the star
			AddSatellites(starCenter, starInst, horizontalLine, isVertical: false);
		}
		
		// Add ships vertically to the star (to the star center)
		if (star.ShipsInOrbit.Count > 0)
		{
			var previousPosition = starInst.Position;
			var previousSize = starInst.Size;

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
		
		// Update camera bounds based on bodies present
		// TODO: Some tweaking to exact positions, otherwise it's ok
		var camera = GetNode<MapCamera>("MapCamera");
		camera.MostRightLowPoint = FindFarthestLowestPoint(starCenter, new Vector2(0, 0));
	}
	
	
	public Vector2 FindFarthestLowestPoint(Node node, Vector2 max)
	{
		var result = max;
		foreach (var child in node.GetChildren())
		{
			if (child is FixedMapObject bodyNode)
			{
				var x = bodyNode.GlobalPosition.X + bodyNode.ImageSize.X / 2;
				var y = bodyNode.GlobalPosition.Y + bodyNode.ImageSize.Y / 2;
				if (x > result.X)
					result.X = x;
				if (y > result.Y)
					result.Y = y;
				
				result = FindFarthestLowestPoint(bodyNode, result);
			}
			else
			{
				result = FindFarthestLowestPoint(child, result);
			}
		}
		return result;
	}

	// Really not a fan of this approach but it's a pain to try and make it better, sorry future me
	private (Control, FixedMapObject) AddSatellites(
		Control parentCenter, FixedMapObject parent, ColorRect orbitLine, bool isVertical = false
		) 
	{
		var lastBody = (center: parentCenter, inst: parent); // Hopefully never stays that
		
		// TODO: Honestly star's bodies and body's satellites should be the same
		List<CelestialBody> bodies;
		if (parent.Body is Star star)
			bodies = star.Bodies;
		else
			bodies = parent.Body.Satellites;
		
		var previousPosition = parent.Position;
		var previousSize = parent.Size;
		foreach (var body in bodies)
		{
			if (body.DiscoveryStatus == DiscoveryStatus.Undiscovered) continue;

			// Must add both before calculating positions to account for relative position
			var bodyCenter = new Control();
			parentCenter.AddChild(bodyCenter);
			
			var bodyInst = BuildCelestialBodyInst(body);
			bodyCenter.AddChild(bodyInst);

			if (isVertical)
			{
				bodyCenter.Position = new Vector2(-(bodyInst.Size.X / 2), 
					previousPosition.Y + previousSize.Y + InterBodiesDistance + (bodyInst.Size.Y / 2));
				// TODO: Take horizontal satellites into account when calculating distance
				// between horizontal bodies higher. Maybe give the center point a size or something like that. 
				bodyInst.Position = new Vector2(0, -(bodyInst.ImageSize.Y / 2));
				// Increase line length until the center point of the body
				orbitLine.Size = new Vector2(OrbitLineSize, bodyCenter.Position.Y);
			}
			else
			{
				bodyCenter.Position = new Vector2(previousPosition.X + previousSize.X + InterBodiesDistance +
												  (bodyInst.Size.X / 2), 0);
				// TODO: Take horizontal satellites into account when calculating distance
				// between horizontal bodies higher. Maybe give the center point a size or something like that. 
				bodyInst.Position = new Vector2(-(bodyInst.ImageSize.X / 2), -(bodyInst.ImageSize.Y / 2) - (OrbitLineSize/2));
				orbitLine.Size = new Vector2(bodyCenter.Position.X, OrbitLineSize);
			}

			// Line will be 0 length if no satellites so not visible
			var line = CreateOrbitalLine(!isVertical);
			bodyCenter.AddChild(line);

			lastBody = (bodyCenter, bodyInst);
			
			if (body.HasSatellites)
			{
				lastBody = AddSatellites(bodyCenter, bodyInst, line, !isVertical);
			}
			if (body.ShipsInOrbit.Count > 0)
				AddShipsInOrbit((bodyCenter, bodyInst), lastBody, line, !isVertical);

			previousPosition = bodyCenter.Position;
			previousSize = bodyInst.Size;

			lastBody.center = bodyCenter;
			lastBody.inst = bodyInst;
		}

		return lastBody;
	}
	

	private void AddShipsInOrbit((Control parentCenter, FixedMapObject parentInst) parent, (Control center, FixedMapObject inst) lastBody, ColorRect orbitLine, bool isVertical)
	{
		var previousPosition = lastBody.center.Position;
		var previousSize = lastBody.inst.Size;
		
		// TODO: Horizontal ships
		foreach (var ship in parent.parentInst.Body.ShipsInOrbit)
		{
			var shipCenter = new Control();
			parent.parentCenter.AddChild(shipCenter);

			var shipInst = BuildShipInst(ship);
			shipCenter.AddChild(shipInst);
			
			if (isVertical)
			{
				shipCenter.Position = new Vector2(
					-(shipInst.Size.X / 2), 
					previousPosition.Y + previousSize.Y + InterBodiesDistance + (shipInst.Size.Y / 2)
				);
				shipInst.Position = new Vector2(0, -(shipInst.ImageSize.Y / 2));
				orbitLine.Size = new Vector2(OrbitLineSize, shipCenter.Position.Y);
			}
			else
			{
				var imageSize = shipInst.GetNode<TextureButton>("ShipButton").Size;
				shipCenter.Position = new Vector2(
					previousPosition.X + previousSize.X + InterBodiesDistance + (shipInst.Size.X / 2), -(imageSize.Y / 2)
					);
				shipInst.Position = new Vector2(-(shipInst.ImageSize.X / 2), -(shipInst.ImageSize.Y / 2));
			
				orbitLine.Size = new Vector2(shipCenter.Position.X, OrbitLineSize);
			}
			

			previousPosition = shipCenter.Position;
			previousSize = shipInst.Size;
		}
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

	private FixedMapObject BuildCelestialBodyInst(CelestialBody body)
	{
		// TODO: Find a better spot for the file
		var scene = GD.Load<PackedScene>("res://Scenes/StarSystemScene/FixedMapObject.tscn");
		var inst = (FixedMapObject)scene.Instantiate();
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
	
