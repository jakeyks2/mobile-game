using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class GhostSensors : MonoBehaviour
{
    public float baseTemperature = 20.0f;
    public float temperature = 20.0f;
    public float baseEMF = 0.2f;
    public float emf = 0.2f;
    public GhostSpawner ghostSpawner;

    bool hasTemperatureSensor = false;
    bool hasMagneticFieldSensor = false;
    XROrigin xrOrigin;
    Camera xrCamera;

    // Start is called before the first frame update
    void Start()
    {
        if (AmbientTemperatureSensor.current != null) hasTemperatureSensor = true;
        if (MagneticFieldSensor.current != null) hasMagneticFieldSensor = true;
        if (hasTemperatureSensor) InputSystem.EnableDevice(AmbientTemperatureSensor.current);
        if (hasMagneticFieldSensor) InputSystem.EnableDevice(MagneticFieldSensor.current);
        xrOrigin = GetComponent<XROrigin>();
        xrCamera = xrOrigin.Camera;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasTemperatureSensor) baseTemperature = AmbientTemperatureSensor.current.ambientTemperature.value;
        temperature = baseTemperature;
        if (hasMagneticFieldSensor)
        {
            Vector3 magneticField = MagneticFieldSensor.current.magneticField.value;
            baseEMF = (magneticField.x + magneticField.y + magneticField.z) / 3;
        }
        emf = baseEMF;
        if (ghostSpawner.ghost != null)
        {
            float ghostDistance = Vector3.Distance(xrCamera.transform.position, ghostSpawner.ghost.transform.position);
            float ghostTempReduction = Mathf.Clamp(10.0f * ghostDistance - 30.0f, -20.0f, 0.0f);
            temperature += ghostTempReduction;
            float ghostEMFIncrease = Mathf.Clamp(150.0f - ghostDistance * -50.0f, 0.0f, 100.0f);
            emf += ghostEMFIncrease;
        }
    }
}
