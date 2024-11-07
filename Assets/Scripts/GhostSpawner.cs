using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GhostSpawner : MonoBehaviour
{
    public GameObject ghostPrefab;
    public PointGenerator pointGenerator;
    public Camera xrCamera;
    public GameObject ghost;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (ghost != null) Destroy(ghost);
            ghost = Instantiate(ghostPrefab, pointGenerator.GetRandomPointInRoom(xrCamera.transform.position, 3.0f), Quaternion.identity);
        }
    }
}
