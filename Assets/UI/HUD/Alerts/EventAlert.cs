using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventAlert : MonoBehaviour
{

    public TextMeshProUGUI title, flavorText, outcomeTMP, outcomeDurationTMP, outcomeTotalDurationTMP, outcomeYouChoose;
    public EffectList outcomeEffects;
    public int yearDone = 0;
    public GameObject eventAlertButtonPrefab;
    public GameObject optionsList, responseSection;

    public ClimateEvent climateEvent;
    private ClimateEvent displayedEvent;

    [SerializeReference]
    public ClimateEventResponse response = null;
    private ClimateEventResponse displayedResponse = null;

    public AnimationCurve transitionCurve;
    public float transitionProgress;
    public float transitionSpeed;

    public Color currentEventColor, activeEventColor;

    private TurnManager turnManager;


    // Start is called before the first frame update
    void Start()
    {
        turnManager = GameObject.FindObjectOfType<TurnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
        // Animate going from one state to another.
        float amount = Time.deltaTime * transitionSpeed;
        if(displayedResponse == null) {
            transitionProgress -= amount;
        } else {
            transitionProgress += amount;
        }
        transitionProgress = Mathf.Clamp(transitionProgress, 0, 1);
        float effectiveProgress = transitionCurve.Evaluate(transitionProgress);

        GetComponent<Image>().color = Color.Lerp(currentEventColor, activeEventColor, effectiveProgress);
        optionsList.SetActive(displayedResponse == null);
        responseSection.SetActive(displayedResponse != null);

        if (yearDone > 0) {
            outcomeDurationTMP.gameObject.SetActive(true);
            int remaining = yearDone - turnManager.CurrentTurnAsYear;
            if (remaining == 1) {
                outcomeDurationTMP.text = "Final Year";
            } else {
                outcomeDurationTMP.text = remaining.ToString() + " Years Remaining";
            }
        } else {
            outcomeDurationTMP.gameObject.SetActive(false);
        }

        DisplayEvent(climateEvent); // If set in the GUI
        DisplayOutcome();
    }

    void DisplayEvent(ClimateEvent e) {
        if (e == displayedEvent) { return; }

        title.text = e.Title;
        flavorText.text = e.FlavorText;

        displayedEvent = e;

        // Set the options
        foreach(Transform child in optionsList.transform) {
            Destroy(child.gameObject);
        }
        int index = 0;
        foreach(ClimateEventResponse response in e.Responses) {
            GameObject gameObject = GameObject.Instantiate(eventAlertButtonPrefab, optionsList.transform);
            EventAlertButton eventAlertButton = gameObject.GetComponent<EventAlertButton>();
            eventAlertButton.title.text = response.FlavorText;
            eventAlertButton.effectList.DisplayEffects(response.SortedResourceEffects);
            eventAlertButton.index = index;
            eventAlertButton.durationTMP.gameObject.SetActive(response.TurnDuration > 0);
            if (response.TurnDuration == 1) {
                eventAlertButton.durationTMP.text = "Active for 1 year";
            } else {
                eventAlertButton.durationTMP.text = "Active for " + response.TurnDuration.ToString() + " years";
            }

            index++; // Yes this is bad form, @ me. I dare you.
        }
    }
    
    void DisplayOutcome() {
        if (displayedResponse == response) { return; }
        displayedResponse = response;

        outcomeTMP.text = response.FlavorText;
        outcomeEffects.DisplayEffects(response.SortedResourceEffects);
        outcomeTotalDurationTMP.gameObject.SetActive(response.TurnDuration > 0);
        if (response.TurnDuration == 1) {
            outcomeTotalDurationTMP.text = "Active for 1 year";
        } else {
            outcomeTotalDurationTMP.text = "Active for " + response.TurnDuration.ToString() + " years";
        }
    }

    public void ChooseOutcome(int index) {
        if (climateEvent == null) { return; }
        response = climateEvent.Responses[index];
        if (response.TurnDuration > 0) {
            yearDone = turnManager.CurrentTurnAsYear + response.TurnDuration;
        }
        DisplayOutcome();

        GetComponent<PubSubSender>().Publish("event.response.selected", response);
    }

    public bool IsBlockingTurn() {
        return climateEvent.Responses.Count > 0 && response == null;
    }

    public void DismissIfReady() {
        if (IsBlockingTurn()) { return; }

        int remaining = yearDone - turnManager.CurrentTurnAsYear;
        if (yearDone == 0 || remaining == 0) {
            Alert alert = GetComponent<Alert>();
            alert.Dismiss(true);
        }
    }
}
