public class Test
{
    public static string CurrentText;
    public static int CurrentInt;
    public static float CurrentFloat;
    public static string CurrentSystem;
    
    
    public static void pt(StellarGeneratorPoints point)
    {
        switch (point)
        {
            case StellarGeneratorPoints.INITMAP:
            {
                Console.WriteLine("Ok");
                var a =float.Parse("10");
                break;
            }
            case StellarGeneratorPoints.INITSYSTEM:
            {
                CurrentSystem = "";
                break;
            }
            case StellarGeneratorPoints.SETSYSTEMNAME:
            {
                CurrentSystem = CurrentText;
                break;
            }
        }
    }
}

public enum StellarGeneratorPoints
{
    INITMAP, INITSYSTEM, SETSYSTEMNAME, INITBODY
}