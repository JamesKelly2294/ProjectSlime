using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ClimateEventRequirement
{
    [System.Serializable]
    public enum ClimateEventCondition
    {
        LessThan,
        LessThanOrEqualTo,
        GreaterThan,
        GreatThanOrEqualTo,
        EqualTo
    }

    [System.Serializable]
    public enum ResourceMode
    {
        Production,
        Consumption,
    }

    [SerializeField]
    public ResourceType AffectedResource;

    [SerializeField]
    public ResourceMode Mode;

    [SerializeField]
    public ClimateEventCondition Condition;

    [SerializeField]
    public int Value;

    private static ResourceType[] _oneShots = new ResourceType[]
    {
        ResourceType.Money,
        ResourceType.Biodiversity,
        ResourceType.SeaLevel,
        ResourceType.Pop
    };

    public bool IsMet(int value)
    {
        switch (Condition)
        {
            case ClimateEventCondition.EqualTo:
                return value == Value;
            case ClimateEventCondition.GreaterThan:
                return value > Value;
            case ClimateEventCondition.GreatThanOrEqualTo:
                return value >= Value;
            case ClimateEventCondition.LessThan:
                return value < Value;
            case ClimateEventCondition.LessThanOrEqualTo:
                return value <= Value;
        }
        return false;
    }

    public bool IsOneShotResourceType
    {
        get
        {
            return _oneShots.Contains(AffectedResource);
        }
    }

    public override string ToString()
    {
        switch (Condition)
        {
            case ClimateEventCondition.EqualTo:
                return AffectedResource + " " + Mode + " == " + Value;
            case ClimateEventCondition.GreaterThan:
                return AffectedResource + " " + Mode + " > " + Value;
            case ClimateEventCondition.GreatThanOrEqualTo:
                return AffectedResource + " " + Mode + " >= " + Value;
            case ClimateEventCondition.LessThan:
                return AffectedResource + " " + Mode + " < " + Value;
            case ClimateEventCondition.LessThanOrEqualTo:
                return AffectedResource + " " + Mode + " <= " + Value;
        }
        return "";
    }
}