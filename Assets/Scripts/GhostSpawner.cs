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

    float ghostTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ghostTimer += Time.deltaTime;
        if (ghostTimer >= 15.0f)
        {
            ghostTimer = 0.0f;
            if (ghost == null)
            {
                ghost = Instantiate(ghostPrefab);
                ghost.GetComponent<Billboard>().playerCamera = xrCamera;
            }
            ghost.transform.position = pointGenerator.GetRandomPointInRoom(xrCamera.transform.position, 3.0f);
            ghost.GetComponent<Renderer>().enabled = false;
        }
    }
}
