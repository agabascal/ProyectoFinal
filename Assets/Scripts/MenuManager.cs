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
    public GameObject blackPanel;

    private void Update()
    {
        
        if(intro.time > 21)
        {
            blackPanel.GetComponent<Image>().color = Color.black;
            SceneManager.LoadScene(1);
            //videoIntro.SetActive(false);
        }
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        panelPrincipal.SetActive(true);
        panelSettings.SetActive(false);
        panelCredits.SetActive(false);
    }
    public void Play()
    {
        StartCoroutine(FadeOut());                
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

    private IEnumerator FadeOut()
    {
        for (float i = 0; i < 1; i += .005f)
        {
            blackPanel.GetComponent<Image>().color = new Color(blackPanel.GetComponent<Image>().color.r, 
                                                               blackPanel.GetComponent<Image>().color.r, 
                                                               blackPanel.GetComponent<Image>().color.r, i);
            yield return null;
        }
        StartCoroutine(FadeIn());
        panelPrincipal.SetActive(false);
        intro.Play();
        videoIntro.SetActive(true);

    }
    private IEnumerator FadeIn()
    {
        for (float i = 1; i > 0; i -= .005f)
        {
            blackPanel.GetComponent<Image>().color = new Color(blackPanel.GetComponent<Image>().color.r,
                                                               blackPanel.GetComponent<Image>().color.r,
                                                               blackPanel.GetComponent<Image>().color.r, i);
            yield return null;
        }
    }
}
