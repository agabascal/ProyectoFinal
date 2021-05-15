using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

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
    private GameObject player;
    public Vector3 playerStartPos,playerStartRot;
    public Vector3 cameraStartPos,cameraStartRot;
    private bool lastDialogue;


    [Header("UI Elements")]
    //Pause Menu
    public GameObject pausePanel;
    private bool isPaused;
    public Image fadeImage;
    public Image blackFadeImage,whiteFadeImage;
    public GameObject dialoguePanel;
    public AudioSource musicEnvaironment;
    public GameObject panelSettings;

    

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

        player = FindObjectOfType<PlayerController>().gameObject;
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            fadeImage = blackFadeImage;
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
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, i);
            yield return null;
        }

        if (treeDialogue.index==0)
        {
            dialoguePanel.SetActive(true);
            treeDialogue.TriggerDialogue(treeDialogue.dialogue[treeDialogue.index]);            
        }

        if (treeDialogue.index == 1 && partsCollected==4)
        {
            dialoguePanel.SetActive(true);
            lastDialogue = true;
            treeDialogue.TriggerDialogue(treeDialogue.dialogue[treeDialogue.index]);
            
        }

    }

    public IEnumerator FadeOut()
    {
        Camera.main.GetComponent<CinemachineBrain>().enabled = false;
        player.GetComponent<PlayerController>().canMove = false;

        for (float i = 0; i < 1; i += .005f)
        {
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.r, fadeImage.color.r, i);
            yield return null;
        }
        if (treeDialogue.index==1)
        {
            if (!lastDialogue)
            {
                StartCoroutine(FadeIn());
            }
            else
            {
                //load next scene
            }          
            Camera.main.transform.localPosition = cameraStartPos;
            Camera.main.transform.localEulerAngles = cameraStartRot;
            player.transform.localPosition = playerStartPos;
            player.transform.eulerAngles = playerStartRot;
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
            musicEnvaironment.Stop();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            pausePanel.SetActive(false);
            isPaused = false;
            Time.timeScale = 1f;
            musicEnvaironment.Play();
        }
        
    }

    public void Settings()
    {
        panelSettings.SetActive(true);
        pausePanel.SetActive(false);
    }

    public void Back()
    {
        panelSettings.SetActive(false);
        pausePanel.SetActive(true);
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
        if (partsCollected== 4)
        {
            fadeImage = whiteFadeImage;
            StartCoroutine(FadeOut());
        }
    }
}
