using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceEffectTurnCountPair
{
    [SerializeField]
    public ResourceEffect ResourceEffect;

    [Min(0)]
    public int TurnCount;
}

[System.Serializable]
public class SpecialResourceEffectTurnCountPair
{
    [SerializeField]
    public SpecialEffect SpecialResourceEffect;

    [Min(0)]
    public int TurnCount;
}

[System.Serializable]
public class ClimateEventResponse
{
    /**
     * Flavor text describing the response to the player.
     */
    [SerializeField]
    public string FlavorText;

    /**
     * The resulting effect of accepting the response.
     */
    [SerializeField]
    public List<ResourceEffectTurnCountPair> ResourceEffects;

    /**
     * The resulting special effect of accepting the response.
     */
    [SerializeField]
    public List<SpecialResourceEffectTurnCountPair> SpecialEffects;
}
