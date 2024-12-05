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
    Label temperatureLabel;
    Button torchButton;
    VisualElement flash;
    

    bool uiLoaded = false;

    float flashTimer = 0.0f;
    float maxFlashTimer = 0.05f;
    bool isFlashActive = false;

    // Start is called before the first frame update
    void Start()
    {
        document = GetComponent<UIDocument>();
        root = document.rootVisualElement;
        progressBarContainer = root.Q("progress-bar-container");
        progressBar = root.Q("progress-bar");
        temperatureLabel = root.Q<Label>("temperature-label");
        torchButton = root.Q<Button>("torch-button");
        flash = root.Q("flash");
        root.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);

        torchButton.clicked += Torch;
    }

    // Update is called once per frame
    void Update()
    {
        if (uiLoaded)
        {
            progressBar.style.height = progressBarContainer.resolvedStyle.height * (sensors.temperature + 20.0f) / 100.0f;
            progressBar.style.top = progressBarContainer.resolvedStyle.height * (100.0f - sensors.temperature) / 100.0f;

            float temperatureRatio = sensors.temperature / sensors.baseTemperature;
            progressBar.style.backgroundColor = new(Color.HSVToRGB(2.0f / 3.0f - temperatureRatio * 2.0f / 3.0f, 1.0f, 1.0f));
        }

        if (isFlashActive)
        {
            flashTimer += Time.deltaTime;
            if (flashTimer >= maxFlashTimer)
            {
                flashTimer = 0.0f;
                flash.style.backgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                isFlashActive = false;
            }
        }
    }

    void OnGeometryChanged(GeometryChangedEvent evt)
    {
        uiLoaded = true;
        float width = root.resolvedStyle.width;
        float height = root.resolvedStyle.height;
        progressBarContainer.style.top = width * 0.05f;
        progressBarContainer.style.left = width * 0.05f;
        torchButton.style.height = width * 0.3f;
        Debug.Log(width * 0.05f);
        Debug.Log(progressBarContainer.resolvedStyle.height);
        Debug.Log(temperatureLabel.parent);
    }

    void Torch()
    {
        flash.style.backgroundColor = new Color(1.0f, 1.0f, 0.8f, 0.2f);
        isFlashActive = true;
    }
}
