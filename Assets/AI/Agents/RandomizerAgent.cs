using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizerAgent : MonoBehaviour, IIntelligence
{
    private GameResourceManager _gm;
    public int Budget;

    // TODO: Have this relate to the number of turns that have passed?
    // e.g. the AI gets more budget to work with the longer the game goes on.
    public int BudgetProduction
    {
        get { return 100; }
    }

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
