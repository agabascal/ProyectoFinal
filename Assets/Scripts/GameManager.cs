using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    //Singleton
    public static GameManager Instance { get; private set; }
    #endregion

    

    //Level 1 Gameplay
    [Header("Level 1 Gameplay")]
    public GameObject[] parts;
    public int partsCollected;
    public DialogueTrigger treeDialogue;

    [Header("UI Elements")]
    //Pause Menu
    public GameObject pausePanel;
    private bool isPaused;
    public Image fadeImage;
    public GameObject dialoguePanel;

    

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

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            StartCoroutine(FadeIn());
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&& pausePanel!=null)
        {
            PauseGame();
        }
        
    }

    public IEnumerator FadeIn()
    {
        for (float i = 1; i > 0; i -= .005f) 
        {            
            fadeImage.color = new Color(0,0,0,i);
            yield return null;
        }
        dialoguePanel.SetActive(true);
        treeDialogue.TriggerDialogue();
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
