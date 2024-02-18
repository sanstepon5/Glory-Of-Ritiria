using System.Collections.Generic;
using System.Linq;
using Godot;
using GloryOfRitiria;
using GloryOfRitiria.Scripts;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.Utils;


// VBox container for all bodies. All bodies have the same stretch ratio (5 for example). 
// Star's ratio is 1 * nb of celestial bodies to keep the same size. 

public partial class star_system_view : Node2D
{
	private GlobalSignals _signals;

	private int _currentStarIndex = 0;
	
	private StarSystemInfo _currentSystem;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
		
		_signals.Connect(nameof(_signals.DetnuraBuildRequested), new Callable(this, nameof(BuildDetnuraSystem)));
		_signals.Connect(nameof(_signals.StarViewBuildRequested), new Callable(this, nameof(BuildSystemMap)));
		_signals.Connect(nameof(_signals.ShipFinishedBuilding), new Callable(this, nameof(ResetSystem)));
		_signals.Connect(nameof(_signals.ShipMoved), new Callable(this, nameof(ResetSystem)));
	}
	
	private void PallyriaPressed()
	{
		_signals.EmitSignal(nameof(_signals.PallyriaClicked));
	}

	// Should somehow add all the ships and stuff that were created that turn
	public void ResetSystem()
	{
		var bodiesCont = GetNode<HBoxContainer>("BodiesHBox");
		// Clear planets (1 because first is the star)
		for (int i = 1; i < bodiesCont.GetChildren().Count; i++)
		{
			var child = bodiesCont.GetChild(i);
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
		GetNode<RichTextLabel>("SystemName").Text = starSystem.Name;
		_currentSystem = starSystem;
		
		// TODO: Manage multiple stars
		List<Star> stars = starSystem.SystemStars;
		var star = stars[_currentStarIndex]; // always 0 for now

		var bodiesCont = GetNode<HBoxContainer>("BodiesHBox");
		bodiesCont.GetNode<TextureRect>("StarVCont/StarMCont/StarImage").Texture = (Texture2D)GD.Load(star.ImagePath);
		bodiesCont.GetNode<Label>("StarVCont/StarName").Text = star.Name;

		// Adding system's main celestial bodies
		// I use while here to make the last white line invisible. It's ugly, yes, and I should redo it.
		var i = 0;
		while (i < star.Bodies.Count-1)
		{
			var body = star.Bodies[i];
			bodiesCont.AddChild(BuildCelestialBody(body));
			i++;
		}
		var lastBody = BuildCelestialBody(star.Bodies[i], true);
		bodiesCont.AddChild(lastBody);


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
				innerShipsHBox.AddChild(BuildShipInst(ship));
			}
			innerShipsLabel.Text = $"Inner Space - {star.InnerSpace.ShipsInOrbit.Count} ships in transit";
		}
	}

	private GridContainer BuildCelestialBody(CelestialBody body, bool lastBody = false)
	{
		// Set main body's properties
		var scene = GD.Load<PackedScene>("res://Scenes/Parts/CelestialBodyScene.tscn");
		var inst = (GridContainer)scene.Instantiate();
		inst.GetNode<Label>("BodyContainer/BodyName").Text = body.Name;
			
		// Setting textures for the button
		inst.GetNode<TextureButton>("BodyContainer/MarginContainer/BodyButton").TextureNormal = (Texture2D)GD.Load(body.ImagePath);
		// For others, it's probably better to use some kind of naming convention for files instead of storing all paths
		// For example only storing "Img/planet.png" and using "Img/planet"+"_hover"+"png"
		inst.GetNode<TextureButton>("BodyContainer/MarginContainer/BodyButton").TextureHover = (Texture2D)GD.Load(body.ImagePath);
		inst.GetNode<TextureButton>("BodyContainer/MarginContainer/BodyButton").TextureDisabled = (Texture2D)GD.Load(body.ImagePath);
		inst.GetNode<TextureButton>("BodyContainer/MarginContainer/BodyButton").TextureFocused = (Texture2D)GD.Load(body.ImagePath);
		inst.GetNode<TextureButton>("BodyContainer/MarginContainer/BodyButton").TexturePressed = (Texture2D)GD.Load(body.ImagePath);
			
		if (body.Name == "Pallyria")
			inst.GetNode<TextureButton>("BodyContainer/MarginContainer/BodyButton").Pressed += PallyriaPressed;
		else 
			inst.GetNode<TextureButton>("BodyContainer/MarginContainer/BodyButton").Pressed += () => PlanetButtonPressed(body);
		
		
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
					satellitesVCont.AddChild(BuildCelestialBody(satellite, true));
				else
					satellitesVCont.AddChild(BuildCelestialBody(satellite));
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
					satellitesHCont.AddChild(BuildCelestialBody(satellite, true));
				else
					satellitesHCont.AddChild(BuildCelestialBody(satellite));
			}
		}

		// Adding ships vertically
		if (body.ShipsInOrbit.Count > 0 && !body.IsSatellite)
		{
			for (var i = 0; i < body.ShipsInOrbit.Count; i++)
			{
				var ship = body.ShipsInOrbit[i];
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
		var inst = (GridContainer)scene.Instantiate();
		inst.GetNode<Label>("ShipContainer/ShipName").Text = ship.Name;

		if (game_state.SelectedShip == ship)
			inst.GetNode<TextureButton>("ShipContainer/MarginContainer/ShipButton").ButtonPressed = true;
		else
			inst.GetNode<TextureButton>("ShipContainer/MarginContainer/ShipButton").ButtonPressed = false;

		inst.GetNode<TextureButton>("ShipContainer/MarginContainer/ShipButton").Pressed += () =>
		{
			if (game_state.SelectedShip == ship)
				game_state.SelectedShip = null;
			else
				game_state.SelectedShip = ship;
			if (game_state.SelectedShip != null) GD.Print(game_state.SelectedShip.Name);
		};
		
		// Hide the white line
		if (lastBody) inst.GetNode<MarginContainer>("NextLineHCont/MarginContainer").Visible = false;
		
		return inst;
	}

	
	
	private void PlanetButtonPressed(CelestialBody body)
	{
		var pallyriaScene = GD.Load<PackedScene>("res://Scenes/Parts/planet_info_window.tscn");
		var inst = (Panel) pallyriaScene.Instantiate();
		
		var title = inst.GetNode<RichTextLabel>("MCont/VBox/TitleExitHBox/Title");
		title.Text = "[b]"+ body.Name +"[/b]\n" + body.BodyType;
		
		var image = inst.GetNode<TextureRect>("MCont/VBox/ImageMargin/PlanetImage");
		image.Texture = (Texture2D)GD.Load(body.ImagePath);
		
		var exitButton = inst.GetNode<Button>("MCont/VBox/TitleExitHBox/ExitButton");
		exitButton.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.InfoWindowClosed));
		};
		
		var sendButtonMargin = inst.GetNode<MarginContainer>("MCont/VBox/SendShipMargin");
		if (game_state.SelectedShip == null)
			sendButtonMargin.Visible = false;
		else
		{
			var sendButton = sendButtonMargin.GetNode<Button>("SendShipButton");
			sendButton.Pressed += () =>
			{
				game_state.SelectedShip.StartRoute(body); 
				_signals.EmitSignal(nameof(_signals.ShipStartedRoute));
			};
		}
		
		// Add inst to the infoWindow control node in base scene UI canvas node
		_signals.EmitSignal(nameof(_signals.PlanetInfoWindowRequested), inst);
		
		// Pause the rest of the game while this window is active.
		GetTree().Paused =  true;
	}

	// Loads objects of the Star at _currentStarIndex
	/*public void InitStarSystem(){
		GD.Print(game_state.SelectedStarSystem[_currentStarIndex])
	}
	// Removes all objects relative to the current system
	public void ClearCurrentSystem(){}

	public void NextStar(){ClearCurrentSystem(); _currentStarIndex++; InitStarSystem();}
	public void PreviousStar(){ClearCurrentSystem(); _currentStarIndex--; InitStarSystem();}
*/
}
