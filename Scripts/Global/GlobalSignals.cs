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
    
    // Closing UI windows
    [Signal]
    public delegate void EventWindowClosedEventHandler();
    [Signal]
    public delegate void InfoWindowClosedEventHandler();
    
    // Warning windows
    [Signal]
    public delegate void WarningWindowRequestedEventHandler(string message);
    
    
    [Signal]
    public delegate void PlanetInfoWindowRequestedEventHandler(Panel window);
}