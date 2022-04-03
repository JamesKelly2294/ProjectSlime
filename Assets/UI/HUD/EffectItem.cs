using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EffectItem : MonoBehaviour
{

    public TextMeshProUGUI amountTMP;

    public Image image;

    public Color positive, negative;

    private GameResourceManager gameResourceManager;

    public ResourceEffect displayedEffect;

    // Start is called before the first frame update
    void Start()
    {
        gameResourceManager = GameObject.FindObjectOfType<GameResourceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        DisplayEffect(displayedEffect); // If set in the UI
    }

    public void DisplayEffect(ResourceEffect effect) {
        if (displayedEffect.AffectedResource == effect.AffectedResource && displayedEffect.EffectAmount == effect.EffectAmount) {
            return;
        }

        if (gameResourceManager == null) {
            gameResourceManager = GameObject.FindObjectOfType<GameResourceManager>();
        }
        
        amountTMP.text = effect.EffectAmount.ToString();
        image.sprite = gameResourceManager.GetSpriteForResourceWithOneIndexedScale(effect.AffectedResource, 3);

        if (effect.EffectAmount >= 0) {
            amountTMP.color = positive;
        } else {
            amountTMP.color = negative;
        }
    }
}
