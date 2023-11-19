using Godot;

namespace GloryOfRitiria;

public partial class GlobalSignals : Node
{
    [Signal]
    public delegate void SkyClickedEventHandler();
    [Signal]
    public delegate void PallyriaClickedEventHandler();
    [Signal]
    public delegate void TurnPassedEventHandler();
    
}