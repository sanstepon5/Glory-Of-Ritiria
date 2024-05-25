namespace GloryOfRitiria.Scripts.Effects;

class ExploreBodyEffect : MissionEffect
{
    public ExploreBodyEffect(string desc = "idunnolol") : base(null, desc)
    {
        
    }
    
    public override void ApplyEffect()
    {
        TargetBody.ExplorePlanet();
    }
}