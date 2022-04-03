using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using PublishKeyType = System.String;

public class DecideNextUpcomingEvent : MonoBehaviour, IDecision
{
    public ClimateEventManager climateEventManager;
    private RandomizerAgent _agent;

    private IEnumerable<ClimateEvent> AllEventsInBudget
    {
        get
        {
            return climateEventManager.ClimateEvents.Where((e) => e.AgentCost < _agent.Budget);
        }
    }

    private BinaryTree<System.Func<bool>> _decisionNodes
    {
        get
        {
            var elements = new (System.Func<bool>, bool)[] {
                (() => AnyCurrentEventFollowOnCardsInBudget(), false),
                (() => PickRandomFollowOnCardInBudget(), true)
            };
            return new BinaryTree<System.Func<bool>>(elements);
        }
    }

    public bool AnyCurrentEventFollowOnCardsInBudget()
    {
        var followOnEvents = from e in AllEventsInBudget
                                where e.FollowOnEvents.Contains(climateEventManager.currentClimateEvent)
                                select e;
        return followOnEvents.Count() > 0;
    }

    public bool PickRandomFollowOnCardInBudget()
    {
        var followOnEvents = from e in AllEventsInBudget
                             where e.FollowOnEvents.Contains(climateEventManager.currentClimateEvent)
                             select e;
        var index = Random.Range(0, followOnEvents.Count() - 1);
        SelectAgentEvent(new List<ClimateEvent>(followOnEvents)[index]);
        return true;
    }

    public void SelectAgentEvent(ClimateEvent agentEvent)
    {
        // TODO
    }

    public void Start()
    {
        _agent = FindObjectOfType<RandomizerAgent>();
    }
}