using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InGameUI : MonoBehaviour
{
    public GhostSensors sensors;

    UIDocument document;
    VisualElement root;
    VisualElement progressBarContainer;
    VisualElement progressBar;

    bool uiLoaded = false;

    // Start is called before the first frame update
    void Start()
    {
        document = GetComponent<UIDocument>();
        root = document.rootVisualElement;
        progressBarContainer = root.Q("progress-bar-container");
        progressBar = root.Q("progress-bar");
        root.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
    }

    // Update is called once per frame
    void Update()
    {
        if (uiLoaded)
        {
            progressBar.style.height = progressBarContainer.resolvedStyle.height * sensors.temperature / 100.0f;
            progressBar.style.top = progressBarContainer.resolvedStyle.height * (100.0f - sensors.temperature) / 100.0f;

            float temperatureRatio = sensors.temperature / sensors.baseTemperature;
            progressBar.style.backgroundColor = new(Color.HSVToRGB(2.0f / 3.0f - temperatureRatio * 2.0f / 3.0f, 1.0f, 1.0f));
        }
    }

    void OnGeometryChanged(GeometryChangedEvent evt)
    {
        uiLoaded = true;
        float width = root.resolvedStyle.width;
        progressBarContainer.style.top = width * 0.05f;
        progressBarContainer.style.left = width * 0.05f;
    }
}
