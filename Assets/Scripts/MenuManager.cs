using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject panelPrincipal;
    public GameObject panelSettings;
    public GameObject panelCredits;

    private void Start()
    {
        panelPrincipal.SetActive(true);
        panelSettings.SetActive(false);
        panelCredits.SetActive(false);
    }
    public void Play()
    {
        SceneManager.LoadScene("Program");
    }

    public void Settings()
    {
        panelPrincipal.SetActive(false);
        panelSettings.SetActive(true);
    }

    public void Credits()
    {
        panelPrincipal.SetActive(false);
        panelCredits.SetActive(true);
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
    }
}
