using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimateEventManager : MonoBehaviour
{
    public List<ClimateEvent> ClimateEvents;

    public ClimateEvent currentClimateEvent;
    public ClimateEvent nextClimateEvent;
    public List<ClimateEvent> upcomingClimateEvents;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Climate Events:");
        foreach (var e in ClimateEvents)
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
}
