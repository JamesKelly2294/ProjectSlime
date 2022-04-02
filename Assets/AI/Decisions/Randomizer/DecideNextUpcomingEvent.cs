using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using PublishKeyType = System.String;

public class DecideNextUpcomingEvent : MonoBehaviour, IDecision
{
    public int Budget = 100;
    public AgentEvent CurrentEvent;
    public IEnumerable<AgentEvent> UpcomingEvents;

    private IEnumerable<AgentEvent> AllEvents { get { return FindObjectsOfType<AgentEvent>(); } }
    private IEnumerable<AgentEvent> AllEventsInBudget { get { return AllEvents.Where((e) => e.AgentCost < Budget); } }

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
                                where e.FollowOnEvents.Contains(CurrentEvent)
                                select e;
        return followOnEvents.Count() > 0;
    }

    public bool PickRandomFollowOnCardInBudget()
    {
        var followOnEvents = from e in AllEventsInBudget
                             where e.FollowOnEvents.Contains(CurrentEvent)
                             select e;
        var index = Random.Range(0, followOnEvents.Count() - 1);
        SelectAgentEvent(new List<AgentEvent>(followOnEvents)[index]);
        return true;
    }

    public void SelectAgentEvent(AgentEvent agentEvent)
    {
        // TODO
    }
}