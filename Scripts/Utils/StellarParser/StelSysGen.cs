using System.Collections.Generic;
using GloryOfRitiria.Scripts.ShipRelated;

namespace GloryOfRitiria.Scripts.Utils;

public class StelSysGen
{
    public static string CurrentText;
    public static int CurrentInt;
    public static float CurrentFloat;
    
    public static StelSysData Data;
    public static StarSystemInfo CurrentSystem;
    public static Star CurrentStar;

    public static void pt(StellarGeneratorPoint point)
    {
        switch (point)
        {
            case StellarGeneratorPoint.INITMAP:
            {
                Data.Systems = new List<StarSystemInfo>();
                break;
            }
            case StellarGeneratorPoint.INITSYSTEM:
            {
                // CurrentText holds id
                CurrentSystem = new StarSystemInfo(CurrentText);
                break;
            }
            case StellarGeneratorPoint.ADDSYSTEM:
            {
                Data.Systems.Add(CurrentSystem);
                break;
            }
        }
    }


    /**This function has to be called after all necessary information has been extracted to game_state.
     * If not done, memory will be occupied by redundant information about the initial state of stellar systems.
     * Returns a struct containing all parsed information.
     */
    public static StelSysData UnloadData()
    {
        // Saving parsing result in a non static variable to return
        // In theory it simply holds references to the static context, so memory wise it should be ok
        var res = new StelSysData();
        res.Systems = Data.Systems;
        res.Ships = Data.Ships;
        res.BodiesWithShipyards = Data.BodiesWithShipyards;
        res.Detnura = Data.Detnura;
        
        // Cleaning static data
        Data = null;

        CurrentText = null;
        CurrentInt = 0;
        CurrentFloat = 0;
        CurrentSystem = null;
        CurrentStar = null;

        return res;
    }
}

/**Utility class that stores parsed data*/
public class StelSysData
{
    public List<StarSystemInfo> Systems;
    public List<Ship> Ships;
    public List<CelestialBody> BodiesWithShipyards;
    public StarSystemInfo Detnura;
    
}


public enum StellarGeneratorPoint
{
    INITMAP, INITSYSTEM, SETSYSTEMNAME, ADDSYSTEM
}