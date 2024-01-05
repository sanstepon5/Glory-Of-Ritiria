using System.Collections.Generic;
using Godot;
using GloryOfRitiria;
using GloryOfRitiria.Scripts.Global;


// VBox container for all bodies. All bodies have the same stretch ratio (5 for example). 
// Star's ratio is 1 * nb of celestial bodies to keep the same size. 

public partial class star_system_view : Node2D
{
	private GlobalSignals _signals;

	private int _currentStarIndex = 0;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
		
		_signals.Connect(nameof(_signals.DetnuraBuildRequested), new Callable(this, nameof(BuildDetnuraSystem)));
		_signals.Connect(nameof(_signals.StarViewBuildRequested), new Callable(this, nameof(BuildSystemMap)));
	}
	
	private void PallyriaPressed()
	{
		_signals.EmitSignal(nameof(_signals.PallyriaClicked));
	}

	
	// Loads planets of Detnura System
	public void BuildDetnuraSystem()
	{
		GD.Print("Detnura Entered");
		BuildSystemMap("Detnura");
	}
	
	public void BuildSystemMap(string name)
	{
		GD.Print("System Map Build Initiated");
		GetNode<RichTextLabel>("SystemName").Text = name;
		
		// TODO: Manage multiple stars
		List<Star> stars;
		game_state.AllStarSystems.TryGetValue(name, out stars);
		var star = stars[_currentStarIndex]; // always 0

		var bodiesCont = GetNode<HBoxContainer>("BodiesHBox");
		bodiesCont.GetNode<TextureRect>("StarVCont/StarMCont/StarImage").Texture = (Texture2D)GD.Load(star.ImagePath);
		bodiesCont.GetNode<Label>("StarVCont/StarName").Text = star.Name;

		
		foreach (var body in star.Bodies)
		{
			var scene = GD.Load<PackedScene>("res://Scenes/Parts/CelestialBodyScene.tscn");
			var inst = (VBoxContainer)scene.Instantiate();
			inst.GetNode<Label>("BodyName").Text = body.Name;
			
			// Setting textures for the button
			inst.GetNode<TextureButton>("MarginContainer/BodyButton").TextureNormal = (Texture2D)GD.Load(body.ImagePath);
			// For others, it's probably better to use some kind of naming convention for files instead of storing all paths
			// For example only storing "Img/planet.png" and using "Img/planet"+"_hover"+"png"
			inst.GetNode<TextureButton>("MarginContainer/BodyButton").TextureHover = (Texture2D)GD.Load(body.ImagePath);
			inst.GetNode<TextureButton>("MarginContainer/BodyButton").TextureDisabled = (Texture2D)GD.Load(body.ImagePath);
			inst.GetNode<TextureButton>("MarginContainer/BodyButton").TextureFocused = (Texture2D)GD.Load(body.ImagePath);
			inst.GetNode<TextureButton>("MarginContainer/BodyButton").TexturePressed = (Texture2D)GD.Load(body.ImagePath);
			
			if (body.Name == "Pallyria")
				inst.GetNode<TextureButton>("MarginContainer/BodyButton").Pressed += PallyriaPressed;
			else 
				inst.GetNode<TextureButton>("MarginContainer/BodyButton").Pressed += () => PlanetButtonPressed();
			
			bodiesCont.AddChild(inst);
		}
	}
	
	private void PlanetButtonPressed()
	{
		var pallyriaScene = GD.Load<PackedScene>("res://Scenes/Parts/planet_info_window.tscn");
		var inst = (Panel) pallyriaScene.Instantiate();
		
		var title = inst.GetNode<RichTextLabel>("MCont/VBox/TitleExitHBox/Title");
		title.Text = "[b]Random Planet[/b]\nMolten Planet";
		
		var image = inst.GetNode<TextureRect>("MCont/VBox/ImageMargin/PlanetImage");
		image.Texture = (Texture2D)GD.Load("res://Assets/Img/tmp/MoltenPlanet.png");
		
		var exitButton = inst.GetNode<Button>("MCont/VBox/TitleExitHBox/ExitButton");
		
		exitButton.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.InfoWindowClosed));
		};
		
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

	// Draws the orbit line for an object given its distance from the star
	private void _drawOrbit(float distance){}
*/
}
