using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    //Singleton
    public static GameManager Instance { get; private set; }
    #endregion

    //Pause Menu
    public GameObject pausePanel;
    private bool isPaused;

    //Level 1
    public GameObject[] parts;
    public int partsCollected;

    //Use Awake to manage Singleton's existance 
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);        
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if (!isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            pausePanel.SetActive(true);
            isPaused = true;
            Time.timeScale = 0f;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            pausePanel.SetActive(false);
            isPaused = false;
            Time.timeScale = 1f;
        }
        
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void UpdateCollected(int id)
    {
        parts[id].SetActive(true);
        partsCollected++;
    }
}
