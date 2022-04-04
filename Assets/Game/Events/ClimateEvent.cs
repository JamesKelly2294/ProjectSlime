using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName ="Climate Events", menuName = "Game/Climate Event", order = 2)]
public class ClimateEvent : ScriptableObject
{
    /**
     * The name of the event.
     */
    [SerializeField]
    public string Title;

    /**
     * Event flavor text. Snark optional.
     */
    [TextArea(10, 100)]
    public string FlavorText;

    /**
     * Resource requirements that must be met before the event is available.
     */
    [SerializeField]
    public List<ClimateEventRequirement> Requirements;

    /**
     * Ways the player can response to the event.
     */
    [SerializeField]
    public List<ClimateEventResponse> Responses;

    /**
     * Events which are preferentially chosen after this one becomes active.
     */
    [SerializeField]
    public List<ClimateEvent> FollowOnEvents;

    /**
     * The result of an event appearing.
     */
    [SerializeField]
    public List<SpecialEffect> SpecialEffects;

    /**
     * Scales the cost of the event. Used by the AI to budget events.
     */
    [SerializeField]
    public double AgentCost;

    /**
     * If true, this event can only fire once.
     */
    [SerializeField]
    public bool IsOneShot = false;
}
