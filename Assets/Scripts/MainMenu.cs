using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public GameObject loginUI;
    public GameObject mainMenuUI;
    public GameObject CreditsUI;

    
    private void Start()
    {
        FindObjectOfType<AudioManager>().Stop("background-endScene");
        FindObjectOfType<AudioManager>().Play("menu-theme");
    }
    public void ShowLoginScreen() //Back button
    {
        loginUI.SetActive(true);
        mainMenuUI.SetActive(false);
    }

    public void ShowCreditsScreen() //Back button
    {
        loginUI.SetActive(false);
        mainMenuUI.SetActive(false);
        CreditsUI.SetActive(true);
    }

    public void ReturnFromLogin()
    {
        mainMenuUI.SetActive(true);
        loginUI.SetActive(false);
    }

    public void ReturnFromCredits()
    {
        mainMenuUI.SetActive(true);
        //loginUI.SetActive(false);
        CreditsUI.SetActive(false);
    }

    public void StartGame() //Back button
    {
        if(LoadedVals.instance.wasGameLoaded)
        {
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene("MainStory", LoadSceneMode.Single);
        }
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
