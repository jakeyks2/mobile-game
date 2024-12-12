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
    VisualElement emfContainer;
    List<VisualElement> emfLevels = new();

    bool uiLoaded = false;

    float flashTimer = 0.0f;
    float maxFlashTimer = 0.2f;
    bool isFlashActive = false;
    bool didHitGhost = false;

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

        emfContainer = root.Q("emf-container");

        for (int i = 0; i < 5; i++)
        {
            VisualElement emfLevel = new VisualElement();
            emfLevel.style.backgroundColor = Color.HSVToRGB((float)i / 12.0f, 1.0f, 1.0f);
            emfLevel.style.position = Position.Relative;
            emfLevel.style.flexGrow = 1.0f;
            emfContainer.Add(emfLevel);
            emfLevels.Add(emfLevel);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (uiLoaded)
        {
            progressBar.style.height = progressBarContainer.resolvedStyle.height * (sensors.temperature + 20.0f) / 100.0f;
            progressBar.style.top = progressBarContainer.resolvedStyle.height * (80.0f - sensors.temperature) / 100.0f;

            float temperatureRatio = sensors.temperature / sensors.baseTemperature;
            progressBar.style.backgroundColor = Color.HSVToRGB(2.0f / 3.0f - temperatureRatio * 2.0f / 3.0f, 1.0f, 1.0f);

            temperatureLabel.text = sensors.temperature.ToString("F0") + "°C";

            for (int i = 0; i < emfLevels.Count; i++)
            {
                emfLevels[i].style.backgroundColor = sensors.emf >= i * 25.0f ? Color.HSVToRGB((float)i / 12.0f, 1.0f, 1.0f) : Color.clear;
            }
        }

        if (isFlashActive)
        {
            flashTimer += Time.deltaTime;
            if (flashTimer >= maxFlashTimer)
            {
                flashTimer = 0.0f;
                flash.style.backgroundColor = Color.clear;
                isFlashActive = false;
                if (didHitGhost)
                {
                    didHitGhost = false;
                    sensors.ghostSpawner.ghost.GetComponent<Renderer>().enabled = false;
                    sensors.ghostSpawner.DamageGhost(1);
                    sensors.ghostSpawner.MoveGhost();
                }
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
        temperatureLabel.style.left = width * 0.1f + progressBarContainer.resolvedStyle.width;
        temperatureLabel.style.top = width * 0.05f;
        temperatureLabel.style.fontSize = progressBarContainer.resolvedStyle.width;
        emfContainer.style.width = width * 0.5f;
        emfContainer.style.height = width * 0.1f;
        emfContainer.style.top = progressBarContainer.resolvedStyle.height / 2.0f;
        emfContainer.style.right = width * 0.05f;
    }

    void Torch()
    {
        flash.style.backgroundColor = new Color(1.0f, 1.0f, 0.8f, 0.2f);
        isFlashActive = true;
        if (sensors.ghostDistance <= 1.0f && sensors.emf >= 100.0f && sensors.ghostSpawner.ghost != null)
        {
            didHitGhost = true;
            sensors.ghostSpawner.ghost.GetComponent<Renderer>().enabled = true;
        }
    }
}
