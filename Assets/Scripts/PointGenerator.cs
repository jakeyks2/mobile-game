using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class WallDetector : MonoBehaviour
{
    ARRaycastManager raycastManager;
    XROrigin xrOrigin;
    Camera xrCamera;

    Vector3 point = new();
    
    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        xrOrigin = GetComponent<XROrigin>();
        xrCamera = xrOrigin.Camera;
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            point = GetRandomPointInRoom(xrCamera.transform.position);
            Debug.Log(point);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawCube(point, new(0.1f, 0.1f, 0.1f));
    }

    public Vector3 GetRandomPointInRoom(Vector3 origin)
    {
        List<Vector3> directions = new();
        for (float angle = 0.0f; angle < 360.0f; angle += 1.0f)
        {
            directions.Insert(Random.Range(0, directions.Count + 1), Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward);
        }
        foreach (Vector3 direction in directions)
        {
            float maxDistance = GetDistanceToNearestPlane(origin, direction);
            if (Mathf.Approximately(maxDistance, 0.0f)) continue;
            float randomDistance = Random.Range(0.0f, maxDistance);
            return origin + direction * randomDistance;
        }
        return new(0.0f, 0.0f, 0.0f);
    }

    public float GetDistanceToNearestPlane(Vector3 origin, Vector3 direction)
    {
        direction.Normalize();
        Ray ray = new(origin, direction);
        List<ARRaycastHit> hitResults = new();
        raycastManager.Raycast(ray, hitResults, TrackableType.PlaneEstimated);
        if (hitResults.Count == 0) return 0.0f;
        ARRaycastHit hit = hitResults[0];
        return hit.distance;
    }
}
