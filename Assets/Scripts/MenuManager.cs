using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Program");
    }

    public void Settings()
    {

    }

    public void Exit()
    {
        Application.Quit();
    }
}
