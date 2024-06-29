using Godot;

// This node corresponds to the visuals of the tooltip. It should be child of the main scene to freely move.
namespace GloryOfRitiria.Scenes.Utils;

public partial class CustomTooltip : Node2D
{
	private RichTextLabel _textLabel;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_textLabel = GetNode<RichTextLabel>("Panel/MCont/TooltipText");
	}
	
	public void SetText(string text)
	{
		_textLabel = GetNode<RichTextLabel>("Panel/MCont/TooltipText");
		_textLabel.Text = text;
	}

	// public void SetTexture(Texture2D texture)
	// {
	// 	Texture = texture;
	// }

	public void SetPosition(Vector2 position)
	{
		GlobalPosition = position;
	}
}
