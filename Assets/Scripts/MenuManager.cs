using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MenuManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject panelPrincipal;
    public GameObject panelSettings;
    public GameObject panelCredits;
    public AudioSource buttonSound;
    public AudioSource backSound;
    public VideoPlayer intro;
    public GameObject videoIntro;

    private void Update()
    {
        if(intro.time >= intro.clip.length)
        {
            SceneManager.LoadScene(1);
        }
    }
    private void Start()
    {
        panelPrincipal.SetActive(true);
        panelSettings.SetActive(false);
        panelCredits.SetActive(false);
    }
    public void Play()
    {
        intro.Play();
        panelPrincipal.SetActive(false);
        videoIntro.SetActive(true);
        buttonSound.Play();
    }

    public void Settings()
    {
        panelPrincipal.SetActive(false);
        panelSettings.SetActive(true);
        buttonSound.Play();
    }

    public void Credits()
    {
        panelPrincipal.SetActive(false);
        panelCredits.SetActive(true);
        buttonSound.Play();
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Back()
    {
        panelPrincipal.SetActive(true);
        panelSettings.SetActive(false);
        panelCredits.SetActive(false);
        backSound.Play();
    }
}
