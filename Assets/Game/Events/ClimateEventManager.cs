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
    public ClimateEvent NextClimateEvent;
    public List<ClimateEventChoice> ClimateDecisions = new List<ClimateEventChoice>();

    /**
     * The most recent response made by the player
     */
    [SerializeReference]
    public ClimateEventResponse LatestResponse;

    private TurnManager _tm;
    private IIntelligence _agent;

    public IEnumerable<ClimateEventChoice> ActiveClimateDecisions
    {
        get
        {
            var currentTurn = _tm.CurrentTurn;
            return from d in ClimateDecisions
                where d != null && d.choice != null
                where (currentTurn - d.turn) <= d.choice.TurnDuration
                select d;
        }
    }

    public IEnumerable<ClimateEventChoice> InactiveClimateDecisions
    {
        get
        {
            var currentTurn = _tm.CurrentTurn;
            return from d in ClimateDecisions
                   where (currentTurn - d.turn) > d.choice.TurnDuration
                   select d;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _tm = GameObject.FindObjectOfType<TurnManager>();
        _agent = GameObject.FindObjectOfType<RandomizerAgent>();

        Debug.Log("Climate Events:");
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

        // move next event to current event
        CurrentClimateEvent = NextClimateEvent;
        NextClimateEvent = null;

        // call the AI to make a choice about adding an event to the queue
        // This will implicitly make NextClimateEvent equal to the next event that we want to run. 
        _agent.DoTheThing();

        Debug.Assert(NextClimateEvent != null, "Morgan made a mistake.");
    }
}
