using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{

    [Range(0f,1f)]
    public float progress = 0.5f;

    public RectTransform inner;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        inner.sizeDelta = new Vector2(rectTransform.sizeDelta.x * progress, inner.sizeDelta.y);
    }
}
