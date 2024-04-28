using System.Collections.Generic;
using GloryOfRitiria.Scenes.Parts;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.Utils;
using Godot;
using Ship = GloryOfRitiria.Scripts.ShipRelated.Ship;


// VBox container for all bodies. All bodies have the same stretch ratio (5 for example). 
// Star's ratio is 1 * nb of celestial bodies to keep the same size. 

namespace GloryOfRitiria.Scripts.Scenes;

public partial class star_system_view : Node2D
{
	private GlobalSignals _signals;

	private int _currentStarIndex = 0;
	
	private StarSystemInfo _currentSystem;

	private List<Ship> _shipsInSystem;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_shipsInSystem = new List<Ship>();
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
		
		_signals.Connect(nameof(_signals.DetnuraBuildRequested), new Callable(this, nameof(BuildDetnuraSystem)));
		_signals.Connect(nameof(_signals.StarViewBuildRequested), new Callable(this, nameof(BuildSystemMap)));
		_signals.Connect(nameof(_signals.ShipFinishedBuilding), new Callable(this, nameof(ResetSystem)));
		_signals.Connect(nameof(_signals.ShipMoved), new Callable(this, nameof(ResetSystem)));
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
		BuildSystemMap(_currentSystem);
		GD.Print("System view updated");
	}

	
	// Loads planets of Detnura System
	public void BuildDetnuraSystem()
	{
		GD.Print("Detnura Entered");
		BuildSystemMap(game_state.Detnura);
	}
	
	public void BuildSystemMap(StarSystemInfo starSystem)
	{
		GD.Print("System Map Build Initiated");
		GetNode<RichTextLabel>("SystemName").Text = starSystem.SystemName;
		_currentSystem = starSystem;
		
		// TODO: Manage multiple stars
		List<Star> stars = starSystem.SystemStars;
		var star = stars[_currentStarIndex]; // always 0 for now
		
		var bodiesCont = GetNode<HBoxContainer>("BodiesHBox");
		bodiesCont.AddChild(BuildStar(star));

		// Adding system's main celestial bodies
		if (star.Bodies.Count > 0)
		{
			var i = 0;
			// TODO: I use while here to make the last white line invisible. It's ugly, yes, and I should redo it.
			while (i < star.Bodies.Count-1)
			{
				var body = star.Bodies[i];
				if (body.DiscoveryStatus != DiscoveryStatus.Undiscovered)
					bodiesCont.AddChild(BuildCelestialBody(body));
				i++;
			}
			if (i>=0 && star.Bodies[i].DiscoveryStatus != DiscoveryStatus.Undiscovered)
			{
				var lastBody = BuildCelestialBody(star.Bodies[i], true);
				bodiesCont.AddChild(lastBody);
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

	private GridContainer BuildStar(Star star)
	{
		var scene = GD.Load<PackedScene>("res://Scenes/Parts/CelestialBodyScene.tscn");
		var inst = (GridContainer)scene.Instantiate();
		inst.GetNode<Label>("BodyContainer/BodyName").Text = star.Name;
		
		inst.GetNode<TextureButton>("BodyContainer/MarginContainer/BodyButton").TextureNormal = (Texture2D)GD.Load(star.GetImage());
		inst.GetNode<TextureButton>("BodyContainer/MarginContainer/BodyButton").Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			_starPressed(star);
		};
		
		var satellitesVCont = inst.GetNode<VBoxContainer>("SatellitesVCont");
		if (star.ShipsInOrbit.Count > 0)
		{
			foreach (var ship in star.ShipsInOrbit)
			{
				_shipsInSystem.Add(ship);
				// TODO: Add vertical white lines
				satellitesVCont.AddChild(BuildShipInst(ship, true));
			}
		}
		
		return inst;
	}

	private GridContainer BuildCelestialBody(CelestialBody body, bool lastBody = false)
	{
		// Set main body's properties
		var scene = GD.Load<PackedScene>("res://Scenes/Parts/CelestialBodyScene.tscn");
		var inst = (GridContainer)scene.Instantiate();
		inst.GetNode<Label>("BodyContainer/BodyName").Text = body.Name;
			
		// Setting textures for the button
		inst.GetNode<TextureButton>("BodyContainer/MarginContainer/BodyButton").TextureNormal = (Texture2D)GD.Load(body.GetImage());
		// For others, it's probably better to use some kind of naming convention for files instead of storing all paths
		// For example only storing "Img/planet.png" and using "Img/planet"+"_hover"+"png"
			
		if (body.Name == "Pallyria")
			inst.GetNode<TextureButton>("BodyContainer/MarginContainer/BodyButton").Pressed += () =>
			{
				_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
				CelestialBodyPressed(body, true);
			};
		else 
			inst.GetNode<TextureButton>("BodyContainer/MarginContainer/BodyButton").Pressed += () =>
			{
				_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
				CelestialBodyPressed(body);
			};
		
		
		if (lastBody) inst.GetNode<MarginContainer>("SatellitesHCont/MarginContainer").Visible = false;
		
		var satellitesVCont = inst.GetNode<VBoxContainer>("SatellitesVCont");
		var satellitesHCont = inst.GetNode<HBoxContainer>("SatellitesHCont");
		// Adding satellites vertically
		if (body.HasSatellites && !body.IsSatellite)
		{
			
			for (var i = 0; i < body.Satellites.Count; i++)
			{
				var satellite = body.Satellites[i];
				// last body
				if (i == body.Satellites.Count - 1)
				{
					if (satellite.DiscoveryStatus != DiscoveryStatus.Undiscovered)
						satellitesVCont.AddChild(BuildCelestialBody(satellite, true));
				}
				else
				{
					if (satellite.DiscoveryStatus != DiscoveryStatus.Undiscovered)
						satellitesVCont.AddChild(BuildCelestialBody(satellite));
				}
			}
		}
		// Adding satellites horizontally
		if (body.HasSatellites && body.IsSatellite)
		{
			
			for (var i = 0; i < body.Satellites.Count; i++)
			{
				var satellite = body.Satellites[i];
				// last body
				if (i == body.Satellites.Count - 1)
				{
					if (satellite.DiscoveryStatus != DiscoveryStatus.Undiscovered)
						satellitesHCont.AddChild(BuildCelestialBody(satellite, true));
				}
				else
				{
					if (satellite.DiscoveryStatus != DiscoveryStatus.Undiscovered)
						satellitesHCont.AddChild(BuildCelestialBody(satellite));
				}
			}
		}

		// Adding ships vertically
		if (body.ShipsInOrbit.Count > 0 && !body.IsSatellite)
		{
			for (var i = 0; i < body.ShipsInOrbit.Count; i++)
			{
				var ship = body.ShipsInOrbit[i];
				_shipsInSystem.Add(ship);
				// TODO: Add vertical white lines
				satellitesVCont.AddChild(BuildShipInst(ship, true));
			}
		}
		
		// Adding ships horizontally
		if (body.ShipsInOrbit.Count > 0 && body.IsSatellite)
		{
			for (var i = 0; i < body.ShipsInOrbit.Count; i++)
			{
				var ship = body.ShipsInOrbit[i];
				_shipsInSystem.Add(ship);
				// last body
				if (i == body.ShipsInOrbit.Count - 1)
					satellitesHCont.AddChild(BuildShipInst(ship, true));
				else
					satellitesHCont.AddChild(BuildShipInst(ship));
			}
		}

		return inst;
	}
	
	
	private GridContainer BuildShipInst(Ship ship, bool lastBody = false)
	{
		// Set main body's properties
		var scene = GD.Load<PackedScene>("res://Scenes/Parts/ShipScene.tscn");
		var inst = (ShipScene)scene.Instantiate();
		inst.GetNode<Label>("ShipContainer/ShipName").Text = ship.Name;
		
		// Set textures of this ship instance (doing it here to avoid updating all instances)
		ship.SimpleUpdate();
		var textureButton = inst.GetNode<TextureButton>("ShipContainer/MarginContainer/ShipButton");
		textureButton.TextureNormal = (Texture2D)GD.Load(ship.GetImagePath());
		textureButton.TexturePressed = (Texture2D)GD.Load(ship.GetImagePath());
		textureButton.TextureHover = (Texture2D)GD.Load(ship.GetImagePath());
		textureButton.TextureHover = (Texture2D)GD.Load(ship.GetImagePath());

		textureButton.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			_handleShipButtonPress(ship);
		};
		
		// Hide the white line
		if (lastBody) inst.GetNode<MarginContainer>("NextLineHCont/MarginContainer").Visible = false;

		inst.Ship = ship;
		
		return inst;
	}

	private void _handleShipButtonPress(Ship ship)
	{
		//if (ship.State == ShipState.InRoute) return;
		if (game_state.SelectedShip != null) GD.Print("Current selected ship: "+game_state.SelectedShip.Name);
		ship.ChangeSelected();
		ship.SimpleUpdate();
		foreach (var otherShip in _shipsInSystem)
		{
			if (ship == otherShip) continue;
			otherShip.Selected = false;
			otherShip.SimpleUpdate();
		}
		_signals.EmitSignal(nameof(_signals.ShipClicked)); // Update all ship scenes
		_signals.EmitSignal(nameof(_signals.TopBarUpdateRequired)); // Update the topbar (really only need one part but later...)
	}

	private void CelestialBodyPressed(CelestialBody body, bool isPallyria = false)
	{
		var planetInfoScene = GD.Load<PackedScene>("res://Scenes/Parts/PlanetInfoWindow.tscn");
		var inst = (PlanetInfoWindow)planetInfoScene.Instantiate();

		inst.Body = body;

		var title = inst.GetNode<RichTextLabel>("MCont/VBox/TitleExitHBox/Title");
		title.Text = "[b]" + body.Name + "[/b]\n" + body.BodyType;

		var image = inst.GetNode<TextureRect>("MCont/VBox/ImageMargin/PlanetImage");
		image.Texture = (Texture2D)GD.Load(body.GetImage());

		var descLabel = inst.GetNode<RichTextLabel>("MCont/VBox/DescMargin/Description");
		descLabel.Text = body.GetDescription();

		var exitButton = inst.GetNode<Button>("MCont/VBox/TitleExitHBox/ExitButton");
		exitButton.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			_signals.EmitSignal(nameof(_signals.InfoWindowClosed));
		};

		_setupSendButtons(body, inst);

		if (isPallyria)
		{
			var toPallyriaMargin = inst.GetNode<MarginContainer>("MCont/VBox/ToPallyriaMargin");
			toPallyriaMargin.Visible = true;
			var toPallyriaButton = toPallyriaMargin.GetNode<Button>("ToPallyriaButton");
			toPallyriaButton.Pressed += () =>
			{
				_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
				_signals.EmitSignal(nameof(_signals.PallyriaClicked));
				_signals.EmitSignal(nameof(_signals.InfoWindowClosed));
			};
		}

		// Add inst to the infoWindow control node in base scene UI canvas node
		_signals.EmitSignal(nameof(_signals.PlanetInfoWindowRequested), inst);

		// Pause the rest of the game while this window is active.
		GetTree().Paused = true;
	}
	
	private void _starPressed(Star star)
	{
		var planetInfoScene = GD.Load<PackedScene>("res://Scenes/Parts/PlanetInfoWindow.tscn");
		var inst = (PlanetInfoWindow)planetInfoScene.Instantiate();

		inst.Body = star;

		var title = inst.GetNode<RichTextLabel>("MCont/VBox/TitleExitHBox/Title");
		title.Text = "[b]" + star.Name + "[/b]\n" + star.BodyType;

		var image = inst.GetNode<TextureRect>("MCont/VBox/ImageMargin/PlanetImage");
		image.Texture = (Texture2D)GD.Load(star.GetImage());

		var exitButton = inst.GetNode<Button>("MCont/VBox/TitleExitHBox/ExitButton");
		exitButton.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.InfoWindowClosed));
		};

		_setupSendButtons(star, inst);
		
		// Add inst to the infoWindow control node in base scene UI canvas node
		_signals.EmitSignal(nameof(_signals.PlanetInfoWindowRequested), inst);

		// Pause the rest of the game while this window is active.
		GetTree().Paused = true;
	}

	private void _setupSendButtons(CelestialBody body, PlanetInfoWindow inst)
	{
		var sendButtonMargin = inst.GetNode<MarginContainer>("MCont/VBox/SendShipMargin");
		var sendButton = sendButtonMargin.GetNode<Button>("SendShipButton");
		if (game_state.SelectedShip != null
			&& game_state.SelectedShip.State != ShipState.InRoute
			&& game_state.SelectedShip.Location != body)
		{
			var missionButtonMargin = inst.GetNode<MarginContainer>("MCont/VBox/MissionMargin");
			
			missionButtonMargin.Visible = true;
			var missionButton = missionButtonMargin.GetNode<OptionButton>("MissionSelect");
			if (game_state.SelectedShip.IsInRouteTo(body))
			{	// If the ship is already in route here, show the disabled button
				sendButtonMargin.Visible = true;
				missionButton.Disabled = true;
				//missionButton.Selected = 0; Set current mission of the ship
				sendButton.Disabled = true;
				sendButton.Text = game_state.SelectedShip.Name + " in route here";
			}
			else sendButton.Disabled = false;

			missionButton.AddItem("Move ship");
			for (var i = 0; i < game_state.SelectedShip.GetAllMissions().Count; i++)
			{
				var mission = game_state.SelectedShip.GetAllMissions()[i];
				missionButton.AddItem(mission.Name);
				if (!body.IsMissionCompatible(mission))
					missionButton.SetItemDisabled(i+1, true); // index 0 is for moving ship
			}
			
			missionButton.Selected = -1;

			missionButton.ItemSelected += index => // After player chooses a mission
			{
				sendButtonMargin.Visible = true;
	
				sendButton.Pressed += () =>
				{
					_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
					_handleSendButton(body, sendButton, missionButton, index);
				};
			};
		}
	}

	/**
	 * <param name="body">body that was clicked</param>
	 * <param name="sendButton">Button sendButton: send button node</param>
	 * <param name="missionButton">OptionButton missionButton: select mission button</param>
	 * <param name="index">long index: index of the option selected in missionButton</param>
	 */
	private void _handleSendButton(CelestialBody body, Button sendButton, OptionButton missionButton, long index)
	{
		game_state.SelectedShip.StartRoute(body);
		_signals.EmitSignal(nameof(_signals.ShipStartedRoute));
		sendButton.Disabled = true;
		sendButton.Text = game_state.SelectedShip.Name + " is in route here";

		if (missionButton.Selected > 0)
		{ // The first one is simple movement which isn't a mission
			game_state.SelectedShip.SetShipMission(missionButton.GetItemText((int)index), body);
		}
	}
}
