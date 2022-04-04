using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizerAgent : MonoBehaviour, IIntelligence
{
    private GameResourceManager _gm;
    private DecideNextUpcomingEvent decideNextUpcomingEvent;
    private ClimateEventManager _cem;

    private float _budget = 1.0f;
    public float Budget
    {
        get { return _budget; }
        set { _budget = Mathf.Clamp(value, 0.0f, 1.0f); }
    }

    // TODO: Have this relate to the number of turns that have passed?
    // e.g. the AI gets more budget to work with the longer the game goes on.
    public float BudgetProductionPerTurn = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        _gm = FindObjectOfType<GameResourceManager>();
        decideNextUpcomingEvent = FindObjectOfType<DecideNextUpcomingEvent>();
        _cem = FindObjectOfType<ClimateEventManager>();
    }

    public void DoTheThing()
    {
        Budget += BudgetProductionPerTurn;
        decideNextUpcomingEvent.Decide();
        var cost = Mathf.Clamp((float)_cem.NextClimateEvent.AgentCost, 0.0f, 1.0f);
        Budget -= cost;
    }
}
