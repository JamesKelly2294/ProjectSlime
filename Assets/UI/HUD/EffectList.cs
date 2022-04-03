using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectList : MonoBehaviour
{

    public GameObject effectPrefab;

    private List<ResourceEffect> displayedEffects = new List<ResourceEffect>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayEffects(List<ResourceEffect> effects) {

        // Make sure we need to update...
        if (displayedEffects.Count == effects.Count) {
            bool returnEarly = true;
            for(int i = 0; i < effects.Count; i++) {
                if (displayedEffects[i].EffectAmount != effects[i].EffectAmount || displayedEffects[i].AffectedResource != effects[i].AffectedResource) {
                    returnEarly = false;
                    break;
                }
            }
            if (returnEarly)
            {
                return;
            }
        }
        
        // Add effects
        foreach(Transform child in transform) {
            GameObject.Destroy(child.gameObject);
        }
        foreach(ResourceEffect effect in effects) {
            GameObject gameObject = GameObject.Instantiate(effectPrefab, transform);
            EffectItem effectItem = gameObject.GetComponent<EffectItem>();
            effectItem.DisplayEffect(effect);
        }

        displayedEffects = effects;
    }
}
