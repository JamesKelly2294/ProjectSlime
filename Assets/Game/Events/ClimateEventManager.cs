using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClimateEventChoice
{
    public ClimateEvent climateEvent;
    public ClimateEventResponse choice;

    // The turn that the choice was made on.
    public int turn;

    public ClimateEventChoice(ClimateEvent climateEvent, ClimateEventResponse choice, int turn)
    {
        this.climateEvent = climateEvent;
        this.choice = choice;
        this.turn = turn;
    }
}

public class ClimateEventManager : MonoBehaviour
{
    public List<ClimateEvent> AllClimateEvents = new List<ClimateEvent>();

    public ClimateEvent CurrentClimateEvent;
    public List<ClimateEventChoice> ClimateDecisions = new List<ClimateEventChoice>();

    /**
     * The most recent response made by the player
     */
    [SerializeReference]
    public ClimateEventResponse LatestResponse;

    private TurnManager _tm;
    private IIntelligence _agent;

    private GameResourceManager _gm;

    public IEnumerable<ClimateEventChoice> ActiveClimateDecisions
    {
        get
        {
            var currentTurn = _tm.CurrentTurn;
            foreach (var d in ClimateDecisions)
            {
                if (d == null || d.choice == null) { continue; }
                Debug.Log("Decision " + d.choice.FlavorText + " was made on " + d.turn + " with a duration of " + d.choice.TurnDuration + ". It is currently turn " + currentTurn + "and currentTurn - d.turn) <= d.choice.TurnDuration =" + ((currentTurn - d.turn) <= d.choice.TurnDuration));
            }
            return from d in ClimateDecisions
                where d != null && d.choice != null
                where (currentTurn - d.turn) < d.choice.TurnDuration
                select d;
        }
    }

    public IEnumerable<ClimateEventChoice> InactiveClimateDecisions
    {
        get
        {
            var currentTurn = _tm.CurrentTurn;
            return from d in ClimateDecisions
                   where d != null && d.choice != null
                   where (currentTurn - d.turn) > d.choice.TurnDuration
                   select d;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _tm = GameObject.FindObjectOfType<TurnManager>();
        _agent = GameObject.FindObjectOfType<RandomizerAgent>();
        _gm = GameObject.FindObjectOfType<GameResourceManager>();

        Debug.Log("Climate Events: (" + AllClimateEvents.Count() + " total)");
        foreach (var e in AllClimateEvents)
        {
            Debug.Log(e.Title);
            foreach(var r in e.Responses)
            {
                Debug.Log("\t" + r.FlavorText);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerable<ClimateEvent> EventsInBudget(double budget)
    {
        return AllClimateEvents.Where((e) => e.AgentCost < budget);
    }

    public IEnumerable<ClimateEvent> EventsWithResourceTypeInResponse(double budget, ResourceType resourceType)
    {
        var list = new ResourceType[] { resourceType };
        return EventsWithResourceTypeInResponse(budget, list);
    }

    public IEnumerable<ClimateEvent> EventsWithResourceTypeInResponse(double budget, ResourceType[] resourceTypes)
    {
        var candidates = from e in AllClimateEvents
                         where e.AgentCost < budget
                         where e.Responses.Any(r => r.ResourceEffects.Any(re => resourceTypes.Contains(re.AffectedResource)))
                         select e;
        return candidates;
    }

    public void StepClimateState(int turn)
    {
        // if current event has long-running stuff, move to active
        var choice = new ClimateEventChoice(CurrentClimateEvent, LatestResponse, turn);
        ClimateDecisions.Add(choice);
        LatestResponse = null;

        // call the AI to make a choice about adding an event to the queue
        // This will implicitly make CurrentClimateEvent equal to the next event that we want to run. 
        _agent.DoTheThing();

        Debug.Assert(CurrentClimateEvent != null, "Morgan made a mistake.");
    }

    public void PlayerSelectedResponse(PubSubListenerEvent e) {
        ClimateEventResponse response = (ClimateEventResponse)e.value;
        LatestResponse = response;
        _gm.CalculateResources();
    }
}
