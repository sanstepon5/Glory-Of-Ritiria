using GloryOfRitiria.Scripts.Global;
using Godot;

namespace GloryOfRitiria.Scripts.Utils;


// Has to inherit Node to pass in signals
public partial class Shipyard : Node
{
    public string ShipyardName;
    public CelestialBody Location;
    public Ship Ship;
    public SlotState State;
    public int TurnCost;
    public int CurrentProgress;
    
    // Constructor for empty shipyard
    public Shipyard(string shipyardName, CelestialBody location)
    {
        ShipyardName = shipyardName;
        Location = location;
        State = SlotState.Empty;
        Ship = null;
        CurrentProgress = 0;
    }
    
    // Constructor for a shipyard with a ship in construction
    public Shipyard(string shipyardName, CelestialBody location, Ship ship, int currentProgress = 0)
    {
        ShipyardName = shipyardName;
        Location = location;
        State = SlotState.Busy;
        Ship = ship;
        CurrentProgress = currentProgress;
        TurnCost = ship.Design.Cost;
    }
    
    public bool Update()
    {
        if (State == SlotState.Busy)
        {
            CurrentProgress++;
            if (CurrentProgress == TurnCost)
            {
                Ship.SetLocation(Location);
                game_state.AllShips.Add(Ship);
                State = SlotState.Empty;
                //_signals.EmitSignal(nameof(_signals.ShipFinishedBuilding)); doesn't work, it's 
                return true;
            }
        }

        return false;
    }

    public void StartConstruction(Ship ship)
    {
        State = SlotState.Busy;
        Ship = ship;
        CurrentProgress = 0;
        TurnCost = ship.Design.Cost;
    }

    public void CancelBuilding()
    {
        State = SlotState.Empty;
    }
    
}

public enum SlotState
{
    Busy,
    Empty,
}