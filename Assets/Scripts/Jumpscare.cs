using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument), typeof(AudioSource))]
public class Jumpscare : MonoBehaviour
{
    UIDocument ui;
    VisualElement root;
    VisualElement ghost;

    AudioSource audioSource;

    [SerializeField]
    AudioClip jumpscareSound;

    bool isPlayingScare = false;
    float scareTimer;

    [SerializeField]
    float scareDuration;

    [SerializeField]
    string sceneToLoad;

    void Start()
    {
        ui = GetComponent<UIDocument>();
        root = ui.rootVisualElement;
        ghost = root.Q("ghost");
        root.style.display = DisplayStyle.None;

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        ghost.style.rotate = new Rotate(Mathf.Sin(Time.time * 100.0f) * 2.0f);

        if (isPlayingScare)
        {
            scareTimer -= Time.deltaTime;
            if (scareTimer <= 0.0f)
            {
                DataStore.hasPlayedGame = true;
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }

    public void PlayScare()
    {
        root.style.display = DisplayStyle.Flex;
        if (PlayerPrefs.GetInt("vibrate") == 1) Vibrator.Vibrate((long)scareDuration * 1000);
        if (PlayerPrefs.GetInt("sound") == 1) audioSource.PlayOneShot(jumpscareSound);
        isPlayingScare = true;
        scareTimer = scareDuration;
    }
}
