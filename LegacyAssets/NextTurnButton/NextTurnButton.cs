using GloryOfRitiria.Scripts.Global;
using Godot;

namespace GloryOfRitiria.Scripts.Scenes.Parts;

public partial class NextTurnButton : Area2D
{
	private GlobalSignals _signals;
	
	public override void _Ready()
	{
		_signals = GetNode<GlobalSignals>("/root/GlobalSignals");
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event is not InputEventMouseButton mouseEvent) return;
		// If mouse clicked and it's left button
		// Should replace Pressed by when the button is up, else you can hold the click...
		if (mouseEvent.Pressed && mouseEvent.ButtonIndex is MouseButton.Left)
		{
			// Check if clicked on the button
			// There is probably a way of making it only work within the node area...
			var clickPosition = GetGlobalMousePosition();
			var collisionShape = GetNode<CollisionPolygon2D>("AreaCollision");
			if (!Geometry2D.IsPointInPolygon(clickPosition, collisionShape.Polygon)) return;

			switch (CheckNextTurnPossible())
			{
				case "ok":
					_signals.EmitSignal(nameof(_signals.TurnPassed));
					UpdateLabels();
					break;
				case "events":
					string message = "Before going to next turn you have to read all events first";
					_signals.EmitSignal(nameof(_signals.WarningWindowRequested), message);
					break;
			}
		}
	}

	public string CheckNextTurnPossible()
	{
		if (game_state.EventsForTurn.Count != 0) return "events";
		// Other turn blocking conditions here
		return "ok";

	}
	
	private void UpdateLabels()
	{
		var turnNumber = GetNode<Label>("CurrentTurn");
		turnNumber.Text = "Current Turn: " + game_state.CurrentTurn;
	}
	
	
	
		
	// public override void _Input(InputEvent @event)
	// {
	// 	if (@event is not InputEventMouseButton mouseEvent) return;
	// 	// If mouse clicked and it's left button
	// 	// Should replace Pressed by when the button is up, else you can hold the click...
	// 	if (mouseEvent.Pressed && mouseEvent.ButtonIndex is MouseButton.Left)
	// 	{
	// 		// Check if clicked on the button
	// 		// There is probably a way of making it only work within the node area...
	// 		var clickPosition = GetGlobalMousePosition();
	// 		var collisionShape = GetNode<CollisionPolygon2D>("AreaCollision");
	// 		if (!Geometry2D.IsPointInPolygon(clickPosition, collisionShape.Polygon)) return;
	//
	// 		switch (CheckNextTurnPossible())
	// 		{
	// 			case "ok":
	// 				_signals.EmitSignal(nameof(_signals.TurnPassed));
	// 				UpdateLabels();
	// 				break;
	// 			case "events":
	// 				string message = "Before going to next turn you have to read all events first";
	// 				_signals.EmitSignal(nameof(_signals.WarningWindowRequested), message);
	// 				break;
	// 		}
	// 	}
	// }
}
