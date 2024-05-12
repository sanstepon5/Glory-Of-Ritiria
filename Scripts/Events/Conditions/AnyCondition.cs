using System.Collections.Generic;

namespace GloryOfRitiria.Scripts.Events.Conditions;

public class AnyCondition : IEventCondition
{
    private List<IEventCondition> _conditions;

    public AnyCondition()
    {
        _conditions = new List<IEventCondition>();
    }
    
    public AnyCondition(List<IEventCondition> conditions)
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
            if (condition.IsSatisfied())
            {
                return true;
            };
        }
        return false;
    }
}