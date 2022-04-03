using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour
{

    public Color readyA, readyB, blocked;
    public GameObject blockedText, buttonArea;

    public bool isBlocked;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        blockedText.SetActive(isBlocked);
        GetComponent<StandardButton>().interactable = !isBlocked;

        if (isBlocked) {
            buttonArea.GetComponent<Image>().color = blocked;
        } else {
            buttonArea.GetComponent<Image>().color = Color.Lerp(readyA, readyB, Mathf.PingPong(Time.time, 1));
        }
    }
}
