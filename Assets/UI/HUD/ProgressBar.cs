using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{

    [Range(0f,1f)]
    public float progress = 0.5f;

    public RectTransform inner;

    public bool highIsBad;

    public Color goodColor, mediumColor, badColor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        inner.sizeDelta = new Vector2(rectTransform.sizeDelta.x * progress, inner.sizeDelta.y);

        float normalizedProgress = progress;
        if (highIsBad) {
            normalizedProgress = 1 - normalizedProgress;
        }

        Image img = inner.gameObject.GetComponent<Image>();
        if (normalizedProgress >= 0.75) {
            img.color = goodColor;
        } else if (normalizedProgress >= 0.25) {
            img.color = mediumColor;
        } else {
            img.color = badColor;
        }
    }
}
