using GloryOfRitiria.Scripts.Utils;
using Godot;

namespace GloryOfRitiria;

public partial class GlobalSignals : Node
{
    // Scene loading
    [Signal]
    public delegate void SkyClickedEventHandler();
    [Signal]
    public delegate void PallyriaClickedEventHandler();
    [Signal]
    public delegate void HangarButtonClickedEventHandler();
    [Signal]
    public delegate void DetnuraSystemRequestedEventHandler();
    [Signal]
    public delegate void StarViewRequestedEventHandler(StarSystemInfo systemInfo);
    
    
    
    // Star System Building
    [Signal]
    public delegate void StarViewBuildRequestedEventHandler(StarSystemInfo systemInfo);
    [Signal]
    public delegate void DetnuraBuildRequestedEventHandler();
    
    // Updating
    [Signal]
    public delegate void TopBarUpdateRequiredEventHandler();
    [Signal]
    public delegate void TurnPassedEventHandler();
    
    // Closing UI windows
    [Signal]
    public delegate void EventWindowClosedEventHandler();
    [Signal]
    public delegate void InfoWindowClosedEventHandler();
    
    // Opening UI windows
    [Signal]
    public delegate void WarningWindowRequestedEventHandler(string message);
    [Signal]
    public delegate void PlanetInfoWindowRequestedEventHandler(Panel window);
    
    // Other UI handling
    [Signal]
    public delegate void HidePassTurnButtonRequestedEventHandler();
    [Signal]
    public delegate void ShowPassTurnButtonRequestedEventHandler();
    
    
    
    // Hangar
    [Signal]
    public delegate void ConstructionWindowRequestedEventHandler(ShipConstructionSlot slot);
    
    [Signal]
    public delegate void ShipBuildStartedEventHandler();
    
    [Signal]
    public delegate void ShipFinishedBuildingEventHandler();
    
    
    // Ship update
    [Signal]
    public delegate void ShipMovedEventHandler();
    [Signal]
    public delegate void ShipStartedRouteEventHandler();
    [Signal]
    public delegate void ShipClickedEventHandler();
}