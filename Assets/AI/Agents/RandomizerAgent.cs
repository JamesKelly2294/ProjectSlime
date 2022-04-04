using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    /**
     * List of the events that the Agent has access to.
     */
    public IEnumerable<ClimateEvent> UsableEvents
    {
        get
        {
            // Criteria:
            // 1. Event is in budget
            // 2. Event's requirements are all met.
            // 3. Event is not a one shot that has already shot.
            var result = from e in _cem.AllClimateEvents
                         where e.AgentCost <= Budget
                         where e.Requirements.All(r =>
                         {
                             var value = _gm.GetValueForResourceType(r.AffectedResource, r.Mode, r.IsOneShotResourceType);
                             return r.IsMet(value);
                         })
                         select e;
            return result;
        }
    }

    public IEnumerable<ClimateEvent> EventsWithRequirementsMet
    {
        get
        {
            var result = from e in _cem.AllClimateEvents
                         where e.Requirements.All(r =>
                         {
                             var value = _gm.GetValueForResourceType(r.AffectedResource, r.Mode, r.IsOneShotResourceType);
                             return r.IsMet(value);
                         })
                         orderby e.AgentCost ascending
                         select e;
            return result;
        }
    }

    public IEnumerable<ClimateEvent> FollowOnEvents
    {
        get
        {
            if (_cem.CurrentClimateEvent != null)
            {
                return from e in UsableEvents
                       where _cem.CurrentClimateEvent.FollowOnEvents.Contains(e)
                       select e;
            }
            else
            {
                return new List<ClimateEvent>();
            }
        }
    }

    // Start is called before the first frame update
    public void Start()
    {
        _gm = FindObjectOfType<GameResourceManager>();
        decideNextUpcomingEvent = FindObjectOfType<DecideNextUpcomingEvent>();
        _cem = FindObjectOfType<ClimateEventManager>();
    }

    public IEnumerable<ClimateEvent> UsableEventsWithResourceTypeInResponse(ResourceType resourceType)
    {
        return from e in UsableEvents
               where e.Responses.Any(r => r.ResourceEffects.Any(re => re.AffectedResource == resourceType))
               select e;
    }

    public IEnumerable<ClimateEvent> UsableEventsWithResourceTypeInResponse(ICollection<ResourceType> resourceTypes)
    {
        return from e in UsableEvents
               where e.Responses.Any(r => r.ResourceEffects.Any(re => resourceTypes.Contains(re.AffectedResource)))
               select e;
    }

    public void DoTheThing()
    {
        Budget += BudgetProductionPerTurn;
        decideNextUpcomingEvent.Decide();
        var cost = Mathf.Clamp((float)_cem.CurrentClimateEvent.AgentCost, 0.0f, 1.0f);
        Budget -= cost;
    }
}
