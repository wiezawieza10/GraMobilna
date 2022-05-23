using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicEndScene : MonoBehaviour
{
    void Awake()
    {
        FindObjectOfType<AudioManager>().Stop("background-dungeon");
        FindObjectOfType<AudioManager>().Play("background-endScene");
    }
}
