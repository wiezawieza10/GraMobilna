using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Maur : MonoBehaviour
{
    private void OnDestroy()
    {
        Destroy(GameObject.Find("HUD"));
        Destroy(GameManager_Multiplayer.instance.player);
        Destroy(GameManager_Multiplayer.instance.player2);
        Destroy(GameObject.Find("GameManager"));
        Destroy(GameObject.Find("InGameMenu"));
        Destroy(GameObject.Find("FloatingTextManager"));
        Destroy(GameObject.Find("UserStatisticsMenu"));
    }
}
