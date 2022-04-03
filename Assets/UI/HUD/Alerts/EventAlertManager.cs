using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if (UNITY_EDITOR) 
using UnityEditor;
#endif

public class EventAlertManager : MonoBehaviour
{

    public GameObject alertPrefab, dividerAlertPrefab;

    private ClimateEventManager climateEventManager;

    // Start is called before the first frame update
    void Start()
    {
        climateEventManager = GameObject.FindObjectOfType<ClimateEventManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject SummonEvent() {
        return GetComponent<AlertManager>().SummonNotification(alertPrefab);
    }

    public GameObject SummonDivider() {
        return GetComponent<AlertManager>().SummonNotification(dividerAlertPrefab);
    }

    public void SummonCurrentEvent() {
        Debug.Log(climateEventManager.CurrentClimateEvent);
        if (climateEventManager.CurrentClimateEvent != null) {
            GameObject gameObject = SummonEvent();
            EventAlert eventAlert = gameObject.GetComponentInChildren<EventAlert>();
            eventAlert.climateEvent = climateEventManager.CurrentClimateEvent;
        }
    }
}



#if (UNITY_EDITOR) 
// Declare type of Custom Editor
[CustomEditor(typeof(EventAlertManager))]
public class EventAlertManagerEditor : Editor 
{

    // OnInspector GUI
    public override void OnInspectorGUI()
    {

        // Call base class method
        base.DrawDefaultInspector();

        // Custom form for Player Preferences
        EventAlertManager alertManager = (EventAlertManager) target;

        if (GUILayout.Button("Summon Event"))
        {
            alertManager.SummonEvent();
        }

        if (GUILayout.Button("Summon Divider"))
        {
            alertManager.SummonDivider();
        }
    }
}
#endif
