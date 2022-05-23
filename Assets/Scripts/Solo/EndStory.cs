using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndStory : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(GameObject.Find("HUD"));
        Destroy(GameObject.Find("Player"));
        Destroy(GameObject.Find("GameManager"));
        Destroy(GameObject.Find("InGameMenu"));
        Destroy(GameObject.Find("FloatingTextManager"));
        Destroy(GameObject.Find("UserStatisticsMenu"));
    }
}
