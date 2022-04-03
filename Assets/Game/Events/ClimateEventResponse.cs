using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

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
    public List<ResourceEffect> ResourceEffects;

    /**
     * The resulting special effect of accepting the response.
     */
    [SerializeField]
    public List<SpecialEffect> SpecialEffects;

    /**
     * How long the effects are active for.
     */
    [Min(0)]
    public int TurnDuration;
}
