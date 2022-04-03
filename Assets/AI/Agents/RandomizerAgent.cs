using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizerAgent : MonoBehaviour, IIntelligence
{
    private GameResourceManager _gm;
    public int Budget;

    private List<ClimateEvent> UpcomingClimateEventQueue;

    // Start is called before the first frame update
    void Start()
    {
        _gm = GameObject.FindObjectOfType<GameResourceManager>();
    }

    public void DoTheThing()
    {
        // do AI magic
        // update climate manager
    }
}
