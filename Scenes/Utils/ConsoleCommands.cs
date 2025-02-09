using Godot;
using System;
using GloryOfRitiria;
using GloryOfRitiria.Scripts.Global;

public partial class ConsoleCommands : Node
{
	private Node _limboConsole; // Store reference to LimboConsole
	private GlobalSignals _signals;

	public override void _Ready()
	{
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
		// Get the LimboConsole singleton. Need to do this because it's in GDScript.
		_limboConsole = GetNode("/root/LimboConsole");

		RegisterCommands();
	}

	private void RegisterCommands()
	{
		_limboConsole.Call("register_command", new Callable(this, nameof(AddMaterialRes)), "add_material_res");
		_limboConsole.Call("register_command", new Callable(this, nameof(AddScience)), "add_science");
		_limboConsole.Call("register_command", new Callable(this, nameof(AddPolitical)), "add_political");
	}
	
	private void AddMaterialRes(float amount)
	{
		game_state.AddMaterialRes(amount);
	}
	private void AddScience(float amount)
	{
		game_state.AddScientificRes(amount);
	}
	private void AddPolitical(float amount)
	{
		game_state.AddPoliticalRes(amount);
	}

}
