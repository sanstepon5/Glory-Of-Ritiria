namespace GloryOfRitiria.Scripts.Effects;

class DiscoverBodiesEffect : MissionEffect
{
    public DiscoverBodiesEffect(string desc = "idunnolol") : base(null, desc)
    {
        
    }
    
    public override void ApplyEffect()
    {
        if (TargetBody is not Star targetStar) return;
        foreach (var body in targetStar.Bodies)
        {
            body.DiscoverBody();
        }

    }
}