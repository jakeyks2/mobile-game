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

public class PointGenerator : MonoBehaviour
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
        
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawCube(point, new(0.1f, 0.1f, 0.1f));
    }

    public Vector3 GetRandomPointInRoom(Vector3 origin, float minDistance)
    {
        List<Vector3> directions = new();
        for (float angle = 0.0f; angle < 360.0f; angle += 1.0f)
        {
            directions.Insert(Random.Range(0, directions.Count + 1), Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward);
        }
        foreach (Vector3 direction in directions)
        {
            float maxDistance = 0.0f;
            if (GetDistanceToNearestPlane(origin, direction, out maxDistance))
            {
                if (maxDistance < minDistance) continue;
                float randomDistance = Random.Range(minDistance, maxDistance);
                return origin + direction * randomDistance;
            }
        }
        foreach (Vector3 direction in directions)
        {
            float maxDistance = 0.0f;
            if (GetDistanceToNearestPlane(origin, direction, out maxDistance))
            {
                float randomDistance = Random.Range(0.0f, maxDistance);
                return origin + direction * randomDistance;
            }
        }
        return origin + directions[0] * minDistance;
    }

    public bool GetDistanceToNearestPlane(Vector3 origin, Vector3 direction, out float distance)
    {
        direction.Normalize();
        Ray ray = new(origin, direction);
        List<ARRaycastHit> hitResults = new();
        raycastManager.Raycast(ray, hitResults, TrackableType.PlaneEstimated);
        if (hitResults.Count == 0)
        {
            distance = 0.0f;
            return false;
        }
        ARRaycastHit hit = hitResults[0];
        distance = hit.distance;
        return true;
    }
}
