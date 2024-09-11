using GloryOfRitiria.Scenes.Utils;
using Godot;

namespace GloryOfRitiria.Scripts.Utils;


// This Node should be attached to the node subject of the tooltip. It controls what text and texture should be shown
// for the subject node. It controls what happens on hover, delays etc.
public partial class TooltipController : Node2D
{
	// Export variables
	[Export] public NodePath OwnerPath;

	[Export(PropertyHint.Range, "0,10,0.05")] 
	public double  EnterDelay = 0.5;
	[Export(PropertyHint.Range, "0,10,0.05")] 
	public double  ExitDelay = 1.5;

	[Export] public bool FollowMouse = true;
	[Export] public int MinimumXSize = 150;
	
	[Export] public string VisualsText = "[b]Default text[/b]";
	[Export] public Texture2D VisualsTexture;

	// How far away from owner the tooltip should be shown
	[Export] public Vector2 Offset;
	[Export] public Vector2 Padding;

	// Private variables
	private Node _ownerNode;
	private CustomTooltip _tooltip;
	private Timer _enterTimer;
	private Timer _exitTimer;
	private Vector2 _border;

	private bool _active; // Active when mouse hovers over this controller
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_ownerNode = GetNode(OwnerPath);
		_tooltip = GetTree().CurrentScene.GetNode<CustomTooltip>("UICanvas/TooltipVisual");
		_tooltip.GetNode<PanelContainer>("Panel").CustomMinimumSize = new Vector2(MinimumXSize, 0);
		_tooltip.Visible = false;
		// _initVisuals();

		_ownerNode.Connect("mouse_entered", new Callable(this, nameof(_mouseEntered)));
		_ownerNode.Connect("mouse_exited", new Callable(this, nameof(_mouseExited)));

		_enterTimer = new Timer();
		_exitTimer = new Timer();
		_enterTimer.Connect("timeout", new Callable(this, nameof(_customShow)));
		_exitTimer.Connect("timeout", new Callable(this, nameof(_customHide)));
		AddChild(_enterTimer);
		AddChild(_exitTimer);

		_border = GetViewportRect().Size - Padding;
	}

	private void _initVisuals()
	{
		// _tooltip.SetTexture(VisualsTexture);
	}
	
	public override void _Process(double delta)
	{
		if (!_active) return;
		if (!_tooltip.Visible) return;
		var tooltipSize = _tooltip.GetNode<PanelContainer>("Panel").Size;
		
		var basePosition = GetViewport().GetMousePosition();
		var finalX = basePosition.X + Offset.X;
		if (finalX + tooltipSize.X > _border.X)
			finalX = basePosition.X - Offset.X -tooltipSize.X;
		var finalY = basePosition.Y - tooltipSize.Y - Offset.Y;
		if (finalY < Padding.Y)	
			finalY = basePosition.Y - Offset.Y;
		var position = new Vector2(finalX, finalY);
		_tooltip.SetPosition(position);
		_tooltip.SetText(VisualsText);
	}
	
	
	// Show the tooltip after a delay
	private void _mouseEntered()
	{
		_enterTimer.Start(EnterDelay);
	}
	private void _customShow()
	{
		_active = true;
		_tooltip.Show();
		_enterTimer.Stop();
	}
	
	// Hide the tooltip after a delay
	private void _mouseExited()
	{
		_exitTimer.Start(ExitDelay);
	}
	private void _customHide()
	{
		_active = false;
		_tooltip.Hide();
		_exitTimer.Stop();
	}
}
