using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public GameObject loginUI;
    public GameObject mainMenuUI;
    public GameObject CreditsUI;
    public GameObject newGameUI;

    private void Start()
    {
        FindObjectOfType<AudioManager>().Stop("background-endScene");
        FindObjectOfType<AudioManager>().Play("menu-theme");
    }
    public void ShowNewGameScreen()
    {
        newGameUI.SetActive(true);
        mainMenuUI.SetActive(false);
    }
    public void ShowLoginScreen()
    {
        loginUI.SetActive(true);
        mainMenuUI.SetActive(false);
    }

    public void ShowCreditsScreen()
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
        CreditsUI.SetActive(false);
    }
    public void ReturnFromNewGame()
    {
        newGameUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    public void StartGame()
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
