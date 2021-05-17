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
    public VideoPlayer intro;
    public GameObject videoIntro;
    public AudioSource musicIntro;

    private void Update()
    {
        if(intro.time >= intro.clip.length)
        {
            Debug.Log("Detectado");
            SceneManager.LoadScene(1);
            videoIntro.SetActive(false);
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
        AudioManager.PlayButtonAudio();
        musicIntro.Stop();
    }

    public void Settings()
    {
        panelPrincipal.SetActive(false);
        panelSettings.SetActive(true);
        AudioManager.PlayButtonAudio();
    }

    public void Credits()
    {
        panelPrincipal.SetActive(false);
        panelCredits.SetActive(true);
        AudioManager.PlayButtonAudio();
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
        AudioManager.PlayBackButtonAudio();
    }
}
