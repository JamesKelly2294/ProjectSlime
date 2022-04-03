using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventAlert : MonoBehaviour
{

    public TextMeshProUGUI title, flavorText;
    public GameObject eventAlertButtonPrefab;
    public GameObject optionsList;

    public ClimateEvent climateEvent;
    private ClimateEvent displayedEvent;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DisplayEvent(climateEvent); // If set in the GUI
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

            index++; // Yes this is bad form, @ me. I dare you.
        }
    }
}
