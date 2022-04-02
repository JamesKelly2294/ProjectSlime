using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentEvent : MonoBehaviour
{
    /**
     * The cost for the AI to use the event, which is based on how long the game has been running? 
     */
    public double agentCostMultiplier = 0.01;

    /**
     * List of events which can happen after this one.
     */
    public IEnumerable<AgentEvent> FollowOnEvents = new List<AgentEvent>();

    public int AgentCost
    {
        get { return (int) (Time.time * agentCostMultiplier); }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
