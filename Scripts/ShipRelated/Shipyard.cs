using System;
using GloryOfRitiria.Scripts.Global;
using GloryOfRitiria.Scripts.StarSystem;
using Godot;

namespace GloryOfRitiria.Scripts.ShipRelated;


// Has to inherit Node to pass in signals
public partial class Shipyard : GodotObject
{
    public string ShipyardName;
    public CelestialBody Location;
    public Ship Ship;
    public SlotState State;
    public int TurnCost;
    public double CurrentProgress;

    public double BuildingEfficiency; // Base building efficiency
    public double BuildingSpeedModifier;
    /**Final speed after modifiers*/ 
    public double BuildingSpeed;
    
    
    // Constructor for empty shipyard
    public Shipyard(string shipyardName, CelestialBody location, double buildingEfficiency = 1.0)
    {
        ShipyardName = shipyardName;
        Location = location;
        State = SlotState.Empty;
        Ship = null;
        CurrentProgress = 0;
        BuildingSpeedModifier = 0.0;
        BuildingEfficiency = buildingEfficiency;
        BuildingSpeed = BuildingEfficiency;
    }
    
    
    /**Returns true if shipyard finished building*/
    public bool Update()
    {
        if (State != SlotState.Busy) return false;
        
        CurrentProgress += BuildingSpeed;
        UpdateBuildingSpeed();
        
        if (CurrentProgress < TurnCost) return false;
        
        Ship.SetLocation(Location);
        game_state.AllShips.Add(Ship);
        State = SlotState.Empty;
        CurrentProgress = 0;
        BuildingSpeed = BuildingEfficiency;

        return true;
    }

    // TODO: Remove test speed
    public void UpdateBuildingSpeed()
    {
        BuildingSpeed = Math.Round(100 + BuildingEfficiency + (BuildingEfficiency * (game_state.ScientificRes / 100)), 4);
    }

    public void StartConstruction(Ship ship, int turnCost)
    {
        State = SlotState.Busy;
        Ship = ship;
        CurrentProgress = 0;
        TurnCost = turnCost;
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