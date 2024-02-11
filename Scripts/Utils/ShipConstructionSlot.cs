using GloryOfRitiria.Scripts.Global;
using Godot;

namespace GloryOfRitiria.Scripts.Utils;



public partial class ShipConstructionSlot : Node
{
    public CelestialBody Location;
    public Ship Ship;
    public SlotState State;
    public int TurnCost;
    public int CurrentProgress;
    
    
    

    public ShipConstructionSlot(CelestialBody location, SlotState state = SlotState.Locked, Ship ship = null)
    {
        
        Location = location;
        State = state;
        Ship = ship;
        CurrentProgress = 0;
        if (ship != null) TurnCost = ship.Design.Cost;
    }
    
    // Constructor for a slot in process of building
    public ShipConstructionSlot(CelestialBody location, Ship ship, int currentProgress = 0)
    {
        Location = location;
        State = SlotState.Building;
        Ship = ship;
        CurrentProgress = currentProgress;
        TurnCost = ship.Design.Cost;
    }
    
    // Constructor for a simple locked slot
    public ShipConstructionSlot()
    {
        Location = null;
        State = SlotState.Locked;
        Ship = null;
        CurrentProgress = 0;
        TurnCost = 0;
    }

    public bool Update()
    {
        if (State == SlotState.Building)
        {
            CurrentProgress++;
            if (CurrentProgress == TurnCost)
            {
                Ship.SetLocation(Location);
                game_state.AllShips.Add(Ship);
                State = SlotState.Full;
                //_signals.EmitSignal(nameof(_signals.ShipFinishedBuilding)); doesn't work, it's 
                return true;
            }
        }

        return false;
    }

    public void SetShip(Ship ship)
    {
        if (State == SlotState.Empty)
        {
            Ship = ship;
        }
    }

    public void CancelBuilding()
    {
        State = SlotState.Empty;
    }
    
}

public enum SlotState
{
    Full,
    Building,
    Empty,
    Locked
}