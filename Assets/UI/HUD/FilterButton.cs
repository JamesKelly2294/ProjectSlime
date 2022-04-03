using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilterButton : MonoBehaviour
{
    public FilterButtonRow filterButtonRow;
    public ResourceType resourceType;
    public Image resourceImage;

    public void SetResourceType(ResourceType type, FilterButtonRow filterButtonRow)
    {
        this.filterButtonRow = filterButtonRow;
        this.resourceType = type;
        resourceImage.sprite = FindObjectOfType<GameResourceManager>().GetSpriteForResourceWithOneIndexedScale(type, 2);
    }

    public void SetFilter()
    {
        filterButtonRow.SetFilter(resourceType, this);
    }
}
