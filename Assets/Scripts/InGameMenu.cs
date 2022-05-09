using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class InGameMenu : MonoBehaviour
{
    public GameObject LoginScreenUI;
    public GameObject RegisterScreenUI;
    public void ShowLoginScreen() //Back button
    {
        LoginScreenUI.SetActive(true);
        RegisterScreenUI.SetActive(false);
    }

    public void ShowRegisterScreen() //Back button
    {
        RegisterScreenUI.SetActive(true);
        LoginScreenUI.SetActive(false);
    }

    public void ReturnFromAuthForm()
    {
        LoginScreenUI.SetActive(false);
        RegisterScreenUI.SetActive(false);
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
