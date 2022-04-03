using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using System.Linq;

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

    public List<ResourceEffect> SortedResourceEffects
    {
        get
        {
            var sorted = from re in ResourceEffects
                         orderby (int)re.AffectedResource
                         select re;
            return sorted.ToList();
        }
    }

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
