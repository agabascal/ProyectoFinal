using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Singleton

    public static GameManager Instance { get; private set; }

    //Pause Menu
    public GameObject pausePanel;
    private bool isPaused;

    //Level 1
    public GameObject[] parts;
    public int partsCollected;

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
    
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
