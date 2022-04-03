using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizerAgent : MonoBehaviour, IIntelligence
{
    private GameResourceManager _gm;
    public int Budget;

    // Start is called before the first frame update
    void Start()
    {
        _gm = GameObject.FindObjectOfType<GameResourceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
