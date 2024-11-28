using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Camera playerCamera;

    void Update()
    {
        transform.rotation = Quaternion.Euler(0.0f, Quaternion.LookRotation(transform.position - playerCamera.transform.position).eulerAngles.y, 0.0f);
    }
}
