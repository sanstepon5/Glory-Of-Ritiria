using System;
using System.Collections.Generic;
using Godot;

namespace GloryOfRitiria.Scripts.StarSystem;

public enum StarSystemType
{
    GenericSimple,
    GenericBinary,
    
    // Special named star systems
    Detnura,
    Sun
}

/// <summary> Class containing information about a star system on the interstellar map </summary>
public partial class StarSystemInfo:Node
{
    /// <summary> Name of the star system (usually the biggest star) </summary>
    public string Id;
    /// <summary> Name of the star system (usually the biggest star) </summary>
    public string SystemName;
    /// <summary> Distance from Detnura to this system in light years </summary>
    public float Distance;
    /// <summary> Angle in degrees indicating the position of the system relative to Detnura on 2D plan </summary>
    public int Angle;
    /// <summary> The type of the star system: Binary system, Neutron star, special systems like Sun etc </summary>
    public StarSystemType SystemType;
    /// <summary> If system is not discovered it won't show on the interstellar map </summary>
    public bool Discovered;

    public List<Star> SystemStars = new();

    public StarSystemInfo(string systemName, float distance, int angle, StarSystemType systemType = StarSystemType.GenericSimple)
    {
        SystemName = systemName;
        Distance = distance;
        Angle = angle;
        SystemType = systemType;
    }
    
    /**Empty constructor for use in parsing*/
    public StarSystemInfo(string id)
    {
        Id = id;
    }

    /// <summary>
    /// Calculates the position where the system icon should be placed on screen based on distance and angle from Detnura
    /// </summary>
    public Vector2 GetPositionOnPlan(Vector2 centerPoint)
    {
        // 50 seem to be the ideal number for distance, at least with 250 radius for a 5 LY circle
        // TODO: Redo this calculation, it's exactly accurate for more than 5 l.y. (even if close)
        var radians = Angle * Math.PI / 180;
        var pointX = (float) (0 + (Distance*50) * Math.Cos(radians));
        var pointY = (float) (0 + (Distance*50) * Math.Sin(radians));
        return new Vector2(pointX, pointY) + centerPoint;
    }
}