using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EffectItem : MonoBehaviour
{

    [Range(-5, 5)]
    public int amount;
    
    public ResourceType resourceType;

    public TextMeshProUGUI amountTMP;

    public Image image;

    public Color positive, negative;

    private GameResourceManager gameResourceManager;

    // Start is called before the first frame update
    void Start()
    {
        gameResourceManager = GameObject.FindObjectOfType<GameResourceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        amountTMP.text = amount.ToString();
        image.sprite = gameResourceManager.GetSpriteForResourceWithOneIndexedScale(resourceType, 3);

        if (amount >= 0) {
            amountTMP.color = positive;
        } else {
            amountTMP.color = negative;
        }
    }
}
