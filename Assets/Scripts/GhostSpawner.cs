using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GhostSpawner : MonoBehaviour
{
    public GameObject ghostPrefab;
    public PointGenerator pointGenerator;
    public Camera xrCamera;
    public GameObject ghost;

    [SerializeField]
    string sceneToLoad;

    float ghostTimer = 0.0f;
    float enrageTimer = 0.0f;
    int ghostHealth = 10;

    [SerializeField]
    AudioClip[] randomSounds;

    AudioSource ghostAudioSource;

    [SerializeField]
    Jumpscare jumpscare;

    void Update()
    {
        ghostTimer += Time.deltaTime;
        enrageTimer += Time.deltaTime;
        if (enrageTimer >= 300.0f)
        {
            jumpscare.PlayScare();
            gameObject.SetActive(false);
        }
        if (ghostTimer >= 15.0f)
        {
            if (ghost != null && PlayerPrefs.GetInt("sound") == 1) ghostAudioSource.PlayOneShot(randomSounds[Random.Range(0, randomSounds.Length)]);
            MoveGhost();
        }
    }

    public void MoveGhost()
    {
        ghostTimer = 0.0f;
        if (ghost == null)
        {
            ghost = Instantiate(ghostPrefab);
            ghost.GetComponent<Billboard>().playerCamera = xrCamera;
            ghostAudioSource = ghost.GetComponent<AudioSource>();
        }
        ghost.transform.position = pointGenerator.GetRandomPointInRoom(xrCamera.transform.position, 3.0f);
        ghost.transform.position -= new Vector3(0.0f, 0.5f, 0.0f);
        ghost.GetComponent<Renderer>().enabled = false;
    }

    public void DamageGhost(int amount)
    {
        ghostHealth -= amount;
        if (ghostHealth <= 0)
        {
            DataStore.hasPlayedGame = true;
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
