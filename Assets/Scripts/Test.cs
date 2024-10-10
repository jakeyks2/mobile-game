using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Test : MonoBehaviour
{
    public ARPlaneManager planeManager;

    public List<ARPlane> planes;

    void Start()
    {
        planeManager.planesChanged += OnPlanesChanged;
    }

    void OnPlanesChanged(ARPlanesChangedEventArgs changes)
    {
        foreach (ARPlane plane in changes.added)
        {
            planes.Add(plane);
        }

        foreach (ARPlane plane in changes.removed)
        {
            planes.Remove(plane);
        }
    }
}
