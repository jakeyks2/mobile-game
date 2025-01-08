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

    void Update()
    {
        ghostTimer += Time.deltaTime;
        enrageTimer += Time.deltaTime;
        if (enrageTimer >= 300.0f)
        {
            if (PlayerPrefs.GetInt("vibrate") == 1) Vibrator.Vibrate(1000);
            Debug.Log("loss");
            SceneManager.LoadScene(sceneToLoad);
        }
        if (ghostTimer >= 15.0f)
        {
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
        }
        ghost.transform.position = pointGenerator.GetRandomPointInRoom(xrCamera.transform.position, 3.0f);
        ghost.GetComponent<Renderer>().enabled = false;
    }

    public void DamageGhost(int amount)
    {
        ghostHealth -= amount;
        if (ghostHealth <= 0)
        {
            Debug.Log("win");
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
