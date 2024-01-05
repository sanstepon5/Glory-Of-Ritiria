using GloryOfRitiria.Scenes.Utils;
using Godot;

namespace GloryOfRitiria.Scripts.Utils;


// This Node should be attached to the node subject of the tooltip. It controls what text and texture should be shown
// for the subject node. It controls what happens on hover, delays etc.
public partial class TooltipController : Control
{
	// Export variables
	[Export] public NodePath OwnerPath;

	[Export(PropertyHint.Range, "0,10,0.05")] 
	public float  EnterDelay = 0.5f;
	[Export(PropertyHint.Range, "0,10,0.05")] 
	public float  ExitDelay = 1.5f;

	[Export] public bool FollowMouse = true;
	
	[Export] public string VisualsText = "[b]Default text[/b]";
	[Export] public Texture2D VisualsTexture;

	// How far away from owner the tooltip should be shown
	[Export(PropertyHint.Range, "0,100,1")] public Vector2 Offset;

	// Private variables
	private Node _ownerNode;
	private CustomTooltip _tooltip;
	private Timer _enterTimer;
	private Timer _exitTimer;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_ownerNode = GetNode(OwnerPath);
		// Current scene should always be base_game_scene. In theory...
		_tooltip = GetTree().CurrentScene.GetNode<CustomTooltip>("TooltipVisual");
		_initVisuals();
		// For some reason MouseEntered seem to exist in C# but godot doesn't see it, only the GDScript name...
		_ownerNode.Connect("mouse_entered", new Callable(this, nameof(_mouseEntered)));
		_ownerNode.Connect("mouse_exited", new Callable(this, nameof(_mouseExited)));

		_enterTimer = new Timer();
		_exitTimer = new Timer();
		_enterTimer.Connect("timeout", new Callable(this, nameof(_customShow)));
		_exitTimer.Connect("timeout", new Callable(this, nameof(_customHide)));
		AddChild(_enterTimer);
		AddChild(_exitTimer);
	}

	private void _initVisuals()
	{
		_tooltip.SetTexture(VisualsTexture);
		_tooltip.SetText(VisualsText);
	}
	
	public override void _Process(double delta)
	{
		if (!_tooltip.Visible) return;
		_tooltip.SetPosition(_getScreenPosition() + Offset);
	}

	// Gets the global position of the center of the node
	private Vector2 _getScreenPosition()
	{
		var position = new Vector2();
		
		if (_ownerNode is Node2D owner2D)
		{
			return owner2D.GetGlobalTransformWithCanvas().Origin;
		}
		// Have to copy to handle both cases (impossible with a || operator)
		if (_ownerNode is Control ownerControl)
		{
			return ownerControl.GetGlobalTransformWithCanvas().Origin + ownerControl.Size/2;
		}
		
		return position;
	}
	
	
	// Show the tooltip after a delay
	private void _mouseEntered()
	{
		_enterTimer.Start(EnterDelay);
	}
	private void _customShow()
	{
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
		_tooltip.Hide();
		_exitTimer.Stop();
	}
}
