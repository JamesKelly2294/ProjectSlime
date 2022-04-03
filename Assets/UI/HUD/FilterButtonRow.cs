using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FilterButtonRow : MonoBehaviour
{
    public GameObject filterButtonPrefab;

    public List<ResourceType> filterableResourceTypes;

    private List<FilterButton> filterButtons = new List<FilterButton>();

    public int ResourceTypeFilterMask = ~0;

    public UnityEvent onFilterChanged;

    public void Start()
    {
        foreach(var resourceType in filterableResourceTypes)
        {
            var go = Instantiate(filterButtonPrefab);

            var filterButton = go.GetComponent<FilterButton>();
            filterButton.SetResourceType(resourceType, this);

            go.transform.parent = transform;

            filterButtons.Add(filterButton);
        }
    }

    public void SetFilter(ResourceType resourceType, FilterButton sender)
    {
        var bit = (1 << (int)resourceType);
        bool resourceTypeWasFilteredFor = ResourceTypeFilterMask == bit;

        // This is a hack since we don't have toggle button
        foreach(var filterButton in filterButtons)
        {
            var image = filterButton.GetComponentInChildren<Image>();
            if (image)
            {
                if (resourceTypeWasFilteredFor)
                {
                    // we are enabling all buttons filter
                    image.color = new Color(image.color.r, image.color.g, image.color.b, 1.0f);
                }
                else
                {
                    if (filterButton == sender)
                    {
                        image.color = new Color(image.color.r, image.color.g, image.color.b, 1.0f);
                    }
                    else
                    {
                        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.4f);
                    }
                }
            }
        }

        if (resourceTypeWasFilteredFor)
        {
            ResourceTypeFilterMask = ~0;
        }
        else
        {
            ResourceTypeFilterMask = bit;
        }

        if (onFilterChanged != null)
        {
            onFilterChanged.Invoke();
        }
    }
}
