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

[CreateAssetMenu(fileName = "Climate Event Response", menuName = "Game/Climate Event Response", order = 3)]
public class ClimateEventResponse : ScriptableObject
{
    /**
     * Flavor text describing the response to the player.
     */
    public string FlavorText;

    /**
     * Smarmy text to share with the player after they accept the response.
     */
    public string AcceptText;

    /**
     * A one time cost for accepting the response.
     */
    public List<ResourceEffectTurnCountPair> OneTimeAcceptCost;

    /**
     * The recurring cost of accepting the response.
     */
    public List<ResourceEffectTurnCountPair> RecurringAcceptCost;

    /**
     * Reference to a behavior object with some callbacks for 
     */
    public ClimateEventAcceptBehavior AcceptBehavior;
}
