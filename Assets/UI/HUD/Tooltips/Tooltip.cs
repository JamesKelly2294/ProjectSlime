using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum TooltipDirection
    {
        Left,
        Right
    }
    public GameObject tooltipPrefab;

    public TooltipDirection direction;

    private GameObject _instantiatedTooltip;

    private GameObject _hud;
    public void Start()
    {
        _hud = GameObject.Find("HUD");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _instantiatedTooltip = Instantiate(tooltipPrefab);
        _instantiatedTooltip.transform.SetParent(_hud.transform, false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(_instantiatedTooltip);
    }
}
