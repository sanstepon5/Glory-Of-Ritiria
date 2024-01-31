using GloryOfRitiria.Scripts.Global;
using Godot;

namespace GloryOfRitiria.Scripts.Utils;



public partial class ShipConstructionSlot : Node
{
    public CelestialBody Location;
    public Ship Ship;
    public SlotState State;
    public int TurnCost;
    public int TurnsLeft;
    

    public ShipConstructionSlot(CelestialBody location, SlotState state = SlotState.Locked, Ship ship = null)
    {
        Location = location;
        State = state;
        Ship = ship;
        TurnsLeft = 0;
        TurnCost = 0;
    }
    
    public ShipConstructionSlot()
    {
        Location = null;
        State = SlotState.Locked;
        Ship = null;
        TurnsLeft = 0;
        TurnCost = 0;
    }

    public void Update()
    {
        if (State == SlotState.Building)
        {
            TurnsLeft--;
            if (TurnsLeft == 0)
            {
                game_state.AllShips.Add(Ship);
                State = SlotState.Full;
                // Emit Signal????
            }
        }
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