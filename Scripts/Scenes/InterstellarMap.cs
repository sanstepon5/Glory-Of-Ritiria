using Godot;
using System;
using System.Collections.Generic;
using GloryOfRitiria;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.Utils;
using StarSystemInfo = GloryOfRitiria.Scripts.StarSystem.StarSystemInfo;

public partial class InterstellarMap : Node2D
{
	private GlobalSignals _signals;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
		
		//BuildDetnura();
		
		foreach (var starSystemInfo in game_state.DiscoveredSystems)
		{
			BuildSystem(starSystemInfo);
		}
		
	}

	public void BuildSystem(StarSystemInfo systemInfo)
	{
		var starSystem = GD.Load<PackedScene>("res://Scenes/Utils/StarSystem.tscn");
		var starSystemInst = (VBoxContainer) starSystem.Instantiate();
		
		starSystemInst.GetNode<RichTextLabel>("SystemName").Text = "[center]" + systemInfo.SystemName;
		
		// Position the system relative to the center of the map (which is Detnura)
		var centerVBox = GetNode<VBoxContainer>("DetnuraVBox");
		starSystemInst.Position = systemInfo.GetPositionOnPlan(centerVBox.Position); // + centerVBox.Size/2
		

		var systemButton = starSystemInst.GetNode<TextureButton>("MarginContainer/StarButton");
		var stars = systemInfo.SystemStars;
		if (stars.Count == 1)
		{
			switch (stars[0].StarClass)
			{
				case StarClass.YellowDwarf:
					systemButton.TextureNormal = (Texture2D)GD.Load("res://Assets/GUI/InterstellarMap/BrightYellowStar64.png"); break;
				case StarClass.OrangeDwarf: 
					systemButton.TextureNormal = (Texture2D)GD.Load("res://Assets/GUI/InterstellarMap/YellowStar64.png"); break;
				case StarClass.RedDwarf: 
					systemButton.TextureNormal = (Texture2D)GD.Load("res://Assets/GUI/InterstellarMap/FaintRedStar64.png"); break;
				default: // TODO: Add star classes
					systemButton.TextureNormal = (Texture2D)GD.Load("res://Assets/GUI/InterstellarMap/BlueStar64.png"); break;
			}
		}
		else
		{	// TODO: Add multiple stars images
			systemButton.TextureNormal = (Texture2D)GD.Load("res://Assets/GUI/InterstellarMap/BrightYellowStar64.png");
		}

		systemButton.Pressed += () =>
		{
			_signals.EmitSignal(nameof(_signals.SimpleButtonClicked));
			_signals.EmitSignal(nameof(_signals.StarViewRequested), systemInfo);
		};
		
		AddChild(starSystemInst);
	}

	// It's better to somehow merge it into BuildSystem since the only difference with any other system
	// Is it's position in the center and of an anchor to orbit lines but not today. 
	// public void BuildDetnura()
	// {
	// 	var systemButton = GetNode<TextureButton>("DetnuraVBox/MarginContainer/DetnuraButton");
	// 	systemButton.Pressed += () => _signals.EmitSignal(nameof(_signals.DetnuraSystemRequested));
	// }
	
	
}
