using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class OptionsMenu : MonoBehaviour
{
    UIDocument ui;
    VisualElement root;
    Button soundButton;
    Button vibrateButton;
    Button backButton;

    bool soundOn = true;
    bool vibrateOn = true;

    [SerializeField]
    MainMenu mainMenu;

    void Start()
    {
        ui = GetComponent<UIDocument>();
        root = ui.rootVisualElement;
        soundButton = root.Q<Button>("sound_button");
        vibrateButton = root.Q<Button>("vibrate_button");
        backButton = root.Q<Button>("back_button");

        soundButton.clicked += Sound;
        vibrateButton.clicked += Vibrate;
        backButton.clicked += Back;

        if (!PlayerPrefs.HasKey("sound")) PlayerPrefs.SetInt("sound", 1);
        if (!PlayerPrefs.HasKey("vibrate")) PlayerPrefs.SetInt("vibrate", 1);
        soundOn = PlayerPrefs.GetInt("sound") == 1;
        vibrateOn = PlayerPrefs.GetInt("vibrate") == 1;
        SetSoundValue(soundOn);
        SetVibrateValue(vibrateOn);

        root.style.display = DisplayStyle.None;
    }

    void Sound()
    {
        soundOn ^= true;
        SetSoundValue(soundOn);
        PlayerPrefs.SetInt("sound", soundOn ? 1 : 0);
    }

    void Vibrate()
    {
        vibrateOn ^= true;
        SetVibrateValue(vibrateOn);
        PlayerPrefs.SetInt("vibrate", vibrateOn ? 1 : 0);
    }

    void Back()
    {
        PlayerPrefs.Save();
        mainMenu.Enable();
        root.style.display = DisplayStyle.None;
    }

    void SetSoundValue(bool value)
    {
        soundButton.text = value ? "Sound: On" : "Sound: Off";
    }

    void SetVibrateValue(bool value)
    {
        vibrateButton.text = value ? "Vibrate: On" : "Vibrate: Off";
    }

    public void Enable()
    {
        root.style.display = DisplayStyle.Flex;
    }
}
