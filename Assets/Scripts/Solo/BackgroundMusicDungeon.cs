using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicDungeon : MonoBehaviour
{
    void Awake()
    {
        FindObjectOfType<AudioManager>().Stop("background-main");
        FindObjectOfType<AudioManager>().Play("background-dungeon");
    }
}
