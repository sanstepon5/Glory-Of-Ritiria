using Godot;
using GloryOfRitiria;

public partial class star_system_view : Node2D
{
	private GlobalSignals _signals;

	//private int _currentStarIndex


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
		
		var pallyriaButton = GetNode<TextureButton>("PlanetButton");
		pallyriaButton.Pressed += PallyriaPressed;
		
		var planetButton = GetNode<Button>("Planet2Button");
		// Calling function with parameters using signals
		planetButton.Pressed += () => PlanetButtonPressed(planetButton);
		
		_signals.Connect(nameof(_signals.DetnuraSystemRequested), new Callable(this, nameof(LoadDetnuraSystem)));
	}
	
	private void PallyriaPressed()
	{
		_signals.EmitSignal(nameof(_signals.PallyriaClicked));
	}

	// Loads planets of Detnura System
	public void LoadDetnuraSystem()
	{
		
	}
	
	private void PlanetButtonPressed(Button button)
	{
		var pallyriaScene = GD.Load<PackedScene>("res://Scenes/Parts/planet_info_window.tscn");
		var inst = (Panel) pallyriaScene.Instantiate();
		
		var title = inst.GetNode<RichTextLabel>("MCont/VBox/TitleExitHBox/Title");
		title.Text = "[b]Random Planet[/b]\nMolten Planet";
		
		var image = inst.GetNode<TextureRect>("MCont/VBox/ImageMargin/PlanetImage");
		image.Texture = (Texture2D)GD.Load("res://Assets/Img/tmp/MoltenPlanet.png");
		
		var exitButton = inst.GetNode<Button>("MCont/VBox/TitleExitHBox/ExitButton");
		// On click of exit button destroy the info window and enable button again
		exitButton.Pressed += () =>
		{
			GetTree().Paused = false;
			inst.QueueFree();
		};
		
		// This sets the info window in the top right corner
		// Better to do it with H/VBoxes I guess...
		AddChild(inst);

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
