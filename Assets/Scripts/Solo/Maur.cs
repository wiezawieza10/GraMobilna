using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Maur : MonoBehaviour
{
    private void OnDestroy()
    {
        Destroy(GameObject.Find("HUD"));
        Destroy(GameObject.Find("Player"));
        Destroy(GameObject.Find("GameManager"));
        Destroy(GameObject.Find("InGameMenu"));
        Destroy(GameObject.Find("FloatingTextManager"));
        Destroy(GameObject.Find("UserStatisticsMenu"));
        SceneManager.LoadScene("EndScene", LoadSceneMode.Single);
    }
}
