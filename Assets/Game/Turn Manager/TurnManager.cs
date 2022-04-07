using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public bool EnableTutorial;

    private GameResourceManager _gm;

    public bool GameStarted { get; protected set; }

    public int CurrentTurn { get; private set; }
    public ClimateEvent KnownGoodFirstEvent;

    public List<ClimateEvent> tutorialEvents, realEvents;
    private List<ClimateEvent> executedOneShotEvents = new List<ClimateEvent>();

    public bool PointOfInterestSelectionAvailable;
    public bool NewBuildingButtonAvailable;
    public bool StatsAvailable;
    public bool YearAvailable;
    public bool YearsUntilExtinctionAvailable;
    public bool ExtendedResourceInfoAvailable;

    private List<ClimateEvent> FilteredClimateEvents
    {
        get
        {
            return (from e in realEvents
                    where executedOneShotEvents.Contains(e) == false
                    select e).ToList();
        }
    }

    public int CurrentTurnAsYear
    {
        get
        {
            return (2050 + CurrentTurn);
        }
    }

    public int TurnsUntilGameEndAsYear
    {
        get
        {
            int yearsRemaining = Mathf.Min(_gm.TurnsToExtinction - CurrentTurn);
            return yearsRemaining;
        }
    }

    private ClimateEventManager climateEventManager;

    // Start is called before the first frame update
    private void Awake()
    {
        if (EnableTutorial == false)
        {
            tutorialEvents.Clear();
            var tm = FindObjectOfType<TutorialManager>();
            if (tm)
            {
                tm.gameObject.SetActive(false);
                Destroy(tm);
            }
        }
    }

    void Start()
    {
        _gm = GameObject.FindObjectOfType<GameResourceManager>();
        climateEventManager = GameObject.FindObjectOfType<ClimateEventManager>();

        StartGame();
    }

    private void StartGame()
    {
        climateEventManager.AllClimateEvents = new List<ClimateEvent>();
        if(EnableTutorial)
        {
            climateEventManager.AllClimateEvents.Add(tutorialEvents[0]);
        }
        else
        {
            climateEventManager.AllClimateEvents.Add(KnownGoodFirstEvent);
        }

        _gm.CalculateResources();
        climateEventManager.StepClimateState(CurrentTurn);
        PublishBeginTurnNotification();

        GameStarted = true;
    }

    private void ProcessEndOfTurnEvents()
    {
        
    }

    private bool EndGameConditionIsReached()
    {
        Debug.Log(TurnsUntilGameEndAsYear);
        return _gm.currentBiodiversity <= _gm.minBiodiversity || _gm.currentSeaLevels >= _gm.maxSeaLevels || TurnsUntilGameEndAsYear <= 0 || _gm.money < 0;
    }

    private void EndGame()
    {
        GameObject.FindObjectOfType<GameManager>().ShowLoseScreen();
    }

    private void PrepareEventSystem() {
        // Make sure we onboard the player
        if (CurrentTurn < tutorialEvents.Count) {
            climateEventManager.AllClimateEvents = new List<ClimateEvent>();
            climateEventManager.AllClimateEvents.Add(tutorialEvents[CurrentTurn]);
        } else if (EnableTutorial == false || tutorialEvents.Count >= CurrentTurn) {
            climateEventManager.AllClimateEvents = FilteredClimateEvents;
        }
        climateEventManager.StepClimateState(CurrentTurn);
    }

    private void AdvanceToNextTurn()
    {
        _gm.CalculateResources();
        _gm.ProcessEndOfTurnResourceUpdates();

        CurrentTurn++;

        PrepareEventSystem();
        _gm.DeductNextTurnCost();
        _gm.CalculateResources();

        userDidHandleEvent = false;
        PublishBeginTurnNotification();
        CheckAndSendValidNextTurnEvents();
    }

    public void EndTurn()
    {
        ProcessEndOfTurnEvents();
        AdvanceToNextTurn();

        if (EndGameConditionIsReached())
        {
            EndGame();
        }
    }

    public void CheckAndSendValidNextTurnEvents() {
        if (GameStarted == false)
        {
            return;
        }

        if (!IsCurrentEventHandled()) {
            PublishForbidNextTurnNotification("Current Event not Handled");
            return;
        }

        var nextTurnCost = _gm.ResourceManagerApprovesNextTurn();
        if (nextTurnCost < 0) {
            PublishAllowButNotRecommendNextTurnNotification("Will reduce Years Until Extinction by " + nextTurnCost.ToString());
            return;
        }

        PublishAllowNextTurnNotification();
    }

    public void PublishBeginTurnNotification() {
        GetComponent<PubSubSender>().Publish("turn.begin");
    }

    public void PublishAllowNextTurnNotification() {
        Debug.Log("Next Turn Allowed!");
        GetComponent<PubSubSender>().Publish("turn.next.allow");
    }

    public void PublishForbidNextTurnNotification(string reason) {
        Debug.Log("Next Turn Forbidden:" + reason);
        GetComponent<PubSubSender>().Publish("turn.next.forbidden", reason);
    }

    public void PublishAllowButNotRecommendNextTurnNotification(string reason) {
        Debug.Log("Next Turn Allowed but not Recommended:" + reason);
        GetComponent<PubSubSender>().Publish("turn.next.allowedButNotRecommended", reason);
    }

    private bool userDidHandleEvent = false;
    public bool IsCurrentEventHandled() {
        if (climateEventManager.CurrentClimateEvent == null) { return true; }
        if (climateEventManager.CurrentClimateEvent.Responses.Count == 0) { return true; }
        return userDidHandleEvent;
    }

    public void UserDidRespondToEvent(PubSubListenerEvent e)
    {
        Debug.Log("UserDidRespondToEvent " + e.value);
        if (!(e.value is ClimateEvent))
        {
            return;
        }

        var response = (ClimateEvent)e.value;

        if(response.IsOneShot)
        {
            executedOneShotEvents.Add(response);
        }
    }

    public void UserDidHandleEvent() {
        userDidHandleEvent = true;
        CheckAndSendValidNextTurnEvents();
    }

    public void ResourcesChanged() {
        CheckAndSendValidNextTurnEvents();
    }

}
