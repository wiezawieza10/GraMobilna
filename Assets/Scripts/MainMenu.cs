using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public GameObject loginUI;
    public GameObject mainMenuUI;
    public GameObject CreditsUI;

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
        loginUI.SetActive(false);
        CreditsUI.SetActive(false);
    }

    public void StartGame() //Back button
    {
        SceneManager.LoadScene("MainStory", LoadSceneMode.Single);
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
