using System.Collections.Generic;
using GloryOfRitiria.Scripts.ShipRelated.Missions;

namespace GloryOfRitiria.Scripts.ShipRelated;

public class Cargo
{
    public string Name;
    /**Number of uses left*/
    public int Durability;
    /**Cost of 0 means Cargo can only be given by event or something*/
    public int Cost;
    public string ImagePath;

    public List<Mission> PossibleMissions;


    public Cargo(string name)
    {
        Name = name;
        Durability = 1;
        Cost = 0;
        ImagePath = "";
        PossibleMissions = new List<Mission>();
    }

    public Cargo(string name, int durability, int cost = 0, string imagePath = "")
    {
        Name = name;
        Durability = durability;
        Cost = cost;
        ImagePath = imagePath;
        PossibleMissions = new List<Mission>();
    }

    public void AddMission(Mission mission)
    {
        mission.SetAssociatedCargo(this);
        PossibleMissions.Add(mission);
    }

    public void DecreaseDurability()
    {
        Durability--;
    }
}