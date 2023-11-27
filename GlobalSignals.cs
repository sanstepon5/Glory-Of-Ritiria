using Godot;

namespace GloryOfRitiria;

public partial class GlobalSignals : Node
{
    // Scene transitions
    [Signal]
    public delegate void SkyClickedEventHandler();
    [Signal]
    public delegate void PallyriaClickedEventHandler();
    [Signal]
    public delegate void TurnPassedEventHandler();
    
    
    // Scene loading
    [Signal]
    public delegate void DetnuraSystemRequestedEventHandler();
    
    
    // Updating
    [Signal]
    public delegate void TopBarUpdateRequiredEventHandler();
}