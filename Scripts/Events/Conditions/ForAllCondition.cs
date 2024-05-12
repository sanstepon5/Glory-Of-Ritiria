using System.Collections.Generic;

namespace GloryOfRitiria.Scripts.Events.Conditions;

public class ForAllCondition : IEventCondition
{
    private List<IEventCondition> _conditions;

    public ForAllCondition()
    {
        _conditions = new List<IEventCondition>();
    }
    
    public ForAllCondition(List<IEventCondition> conditions)
    {
        _conditions = conditions;
    }

    public void AddCondition(IEventCondition condition)
    {
        _conditions.Add(condition);
    }

    public bool IsSatisfied()
    {
        foreach (var condition in _conditions)
        {
            if (!condition.IsSatisfied())
            {
                return false;
            };
        }
        return true;
    }
}