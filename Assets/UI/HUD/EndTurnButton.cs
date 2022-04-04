using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndTurnButton : MonoBehaviour
{

    public Color readyA, readyB, badA, badB, blocked;
    public GameObject blockedText, buttonArea;

    public bool isBlocked, isGoingToCostYou;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        blockedText.SetActive(isBlocked || isGoingToCostYou);
        GetComponent<StandardButton>().interactable = !isBlocked;

        if (isGoingToCostYou) {
            buttonArea.GetComponent<Image>().color = Color.Lerp(badA, badB, Mathf.PingPong(Time.time, 1));
        } else if (isBlocked) {
            buttonArea.GetComponent<Image>().color = blocked;
        } else {
            buttonArea.GetComponent<Image>().color = Color.Lerp(readyA, readyB, Mathf.PingPong(Time.time, 1));
        }
    }

    public void Enable() {
        isBlocked = false;
        isGoingToCostYou = false;
    }

    public void Disable(PubSubListenerEvent reason) {
        isBlocked = true;
        isGoingToCostYou = false;
        blockedText.GetComponent<TextMeshProUGUI>().text = (string)reason.value;
    }

    public void Manable(PubSubListenerEvent reason) {
        isBlocked = false;
        isGoingToCostYou = true;
        blockedText.GetComponent<TextMeshProUGUI>().text = (string)reason.value;
    }
}
