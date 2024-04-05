using GloryOfRitiria.Scripts;
using GloryOfRitiria.Scripts.Utils;
using Godot;
using Ship = GloryOfRitiria.Scripts.ShipRelated.Ship;

namespace GloryOfRitiria;

public partial class GlobalSignals : Node
{
    // Scene loading
    [Signal]
    public delegate void SkyClickedEventHandler();
    [Signal]
    public delegate void PallyriaClickedEventHandler();
    [Signal]
    public delegate void ShipyardsButtonClickedEventHandler();
    [Signal]
    public delegate void FleetBureauButtonClickedEventHandler();
    [Signal]
    public delegate void DetnuraSystemRequestedEventHandler();
    [Signal]
    public delegate void StarViewRequestedEventHandler(StarSystemInfo systemInfo);
    
    
    // Process Priority
    // First level: Everything is unpaused
    // Second level: Game is paused except for the active window
    // Third level: A new window is active while the old window is now paused with the rest of the game
    [Signal]
    public delegate void ThirdLevelProcessEnteredEventHandler();
    [Signal]
    public delegate void ThirdLevelProcessExitedEventHandler();
    
    
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
    
    //UI sound management (clicking, hovering etc)
    [Signal]
    public delegate void SimpleButtonClickedEventHandler();
    [Signal]
    public delegate void NextEventButtonClickedEventHandler();
    [Signal]
    public delegate void EventOptionButtonClickedEventHandler();
    
    // Other UI handling
    [Signal]
    public delegate void HidePassTurnButtonRequestedEventHandler();
    [Signal]
    public delegate void ShowPassTurnButtonRequestedEventHandler();
    
    
    
    // Hangar
    [Signal]
    public delegate void ConstructionWindowRequestedEventHandler(Shipyard slot);
    
    [Signal]
    public delegate void ShipBuildStartedEventHandler();
    
    [Signal]
    public delegate void ShipFinishedBuildingEventHandler();
    
    [Signal]
    public delegate void ShipyardsSceneOpenedEventHandler();
    [Signal]
    public delegate void FleetBureauSceneOpenedEventHandler();
    
    [Signal] // index of the body in list of bodies with shipyards
    public delegate void ShipyardsBodyChangedEventHandler(int index);
    
    [Signal]
    public delegate void FullSlotClickedEventHandler(Ship ship);
    
    [Signal]
    public delegate void AddCargoClickedEventHandler();
    
    [Signal]
    public delegate void CargoAddedEventHandler();
    
    [Signal]
    public delegate void CargoSelectedForOutfitEventHandler(string cargoName); //TODO: find something better for this...
    
    
    // Ship update
    [Signal]
    public delegate void ShipMovedEventHandler();
    [Signal]
    public delegate void ShipStartedRouteEventHandler();
    [Signal]
    public delegate void ShipClickedEventHandler();
}