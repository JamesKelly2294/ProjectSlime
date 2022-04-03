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
    public string Title;

    /**
     * Event flavor text. Snark optional.
     */
    public string FlavorText;

    /**
     * Ways the player can response to the event.
     */
    public List<ClimateEventResponse> Responses;

    /**
     * Events which are preferentially chosen after this one becomes active.
     */
    public List<ClimateEvent> FollowOnEvents;

    /**
     * Scales the cost of the event. Used by the AI to budget events.
     */
    public double AgentCost;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
