using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class GhostSensors : MonoBehaviour
{
    public float baseTemperature = 20.0f;
    public float temperature = 20.0f;
    public GhostSpawner ghostSpawner;

    bool hasTemperatureSensor = false;
    XROrigin xrOrigin;
    Camera xrCamera;

    // Start is called before the first frame update
    void Start()
    {
        if (AmbientTemperatureSensor.current != null) hasTemperatureSensor = true;
        if (hasTemperatureSensor) InputSystem.EnableDevice(AmbientTemperatureSensor.current);
        xrOrigin = GetComponent<XROrigin>();
        xrCamera = xrOrigin.Camera;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasTemperatureSensor) baseTemperature = AmbientTemperatureSensor.current.ambientTemperature.value;
        temperature = baseTemperature;
        if (ghostSpawner.ghost != null)
        {
            float ghostDistance = Vector3.Distance(xrCamera.transform.position, ghostSpawner.ghost.transform.position);
            float ghostTempReduction = Mathf.Clamp(10.0f * ghostDistance - 30.0f, -20.0f, 0.0f);
            temperature += ghostTempReduction;
        }
    }
}
