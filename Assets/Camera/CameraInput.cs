using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraInput : MonoBehaviour
{
    static int pointOfInterestLayer = 7;

    public static bool IsPointerOverUIElement()
    {
        var eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Where(r => r.gameObject.layer == 5).Count() > 0;
    }

    private MouseOrbit _mouseOrbit;
    private PointOfInterestManager _poim;

    // Start is called before the first frame update
    void Start()
    {
        _mouseOrbit = FindObjectOfType<MouseOrbit>();
        _poim = FindObjectOfType<PointOfInterestManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            _mouseOrbit.ProcessCameraOrbit = false;
        }
        else if (!IsPointerOverUIElement() && Input.GetMouseButtonDown(0))
        {
            _mouseOrbit.ProcessCameraOrbit = true;

            int layerMask = ~0;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(Input.mousePosition, ray.direction * hit.distance, Color.yellow);

                if (hit.collider.gameObject.layer == pointOfInterestLayer)
                {
                    Debug.Log("Hit point of interest!");
                    var poi = hit.collider.transform.parent.GetComponent<PointOfInterest>();

                    if (poi == _poim.SelectedPointOfInterest) {
                        _poim.SetSelectedPointOfInterest(null);
                    }
                    else
                    {
                        _poim.SetSelectedPointOfInterest(poi);
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            _poim.SetSelectedPointOfInterest(null);
        }
    }
}
