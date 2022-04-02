using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesDisplay : MonoBehaviour
{

    public ResourceSummary money, energy, steel, titanium, biomass, science;

    private GameResourceManager gameResourceManager;

    // Start is called before the first frame update
    void Start()
    {
        gameResourceManager = GameObject.FindObjectOfType<GameResourceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        money.value.text = gameResourceManager.money.ToString("#,0") + "B";
        
    }
}
