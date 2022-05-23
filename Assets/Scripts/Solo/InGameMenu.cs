using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
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

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("StartMenu", LoadSceneMode.Single);
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
