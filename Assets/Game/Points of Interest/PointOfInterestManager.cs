using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterestManager : MonoBehaviour
{
    public GameObject PointOfInterestSelectionPrefab;
    private GameObject _selectionObj;

    public List<PointOfInterest> PointsOfInterest { get; private set; }

    public PointOfInterest SelectedPointOfInterest { get; private set; }

    public void RegisterPointOfInterest(PointOfInterest pointOfInterest)
    {
        PointsOfInterest.Add(pointOfInterest);
    }

    public void SetSelectedPointOfInterest(PointOfInterest pointOfInterest)
    {
        var deselected = SelectedPointOfInterest;
        SelectedPointOfInterest = pointOfInterest;

        if (deselected) {
            deselected.WasDeselected();
        }

        if (SelectedPointOfInterest)
        {
            SelectedPointOfInterest.WasSelected();

            _selectionObj.SetActive(true);
            _selectionObj.transform.position = SelectedPointOfInterest.transform.position;
        }
        else
        {
            _selectionObj.SetActive(false);
        }
    }

    private void Awake()
    {
        PointsOfInterest = new List<PointOfInterest>();
        _selectionObj = Instantiate(PointOfInterestSelectionPrefab);
        _selectionObj.transform.name = "Point of Interest Selection";
        _selectionObj.transform.parent = transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
