using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public GameObject loginUI;
    public GameObject mainMenuUI;

    public void ShowLoginScreen() //Back button
    {
        loginUI.SetActive(true);
        mainMenuUI.SetActive(false);
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
