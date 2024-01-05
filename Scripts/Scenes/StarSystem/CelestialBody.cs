using Godot;

public enum CelestialBodyType
{
    GenericPlanet,
    Pallyria
}

public class CelestialBody
{
    public string Name;
    // Distance from its parent star, probably in AU
    public float Distance;
    // Angle in degrees relative to the star (0 or 360 = to the right of the star)
    //public int Angle;
    // Random planet, some specific planet, asteroid, station....
    public string ImagePath;
    public CelestialBodyType BodyType;
    

    public CelestialBody(string name, float distance, string imagePath, CelestialBodyType type = CelestialBodyType.GenericPlanet){
        Name = name;
        Distance = distance;
        ImagePath = imagePath;
        BodyType = type;
    }

    // Get the position of the body on the map using distance and angle
    // public Vector2 GetGlobalPosition()
    // {
    //     return new Vector2();
    // }


    // Functions that get information about the body based on id or something
    // public string GetBodyImage(){
    //     return "";
    // }
    // public string GetBodyDescription(){
    //     return "";
    // }
}