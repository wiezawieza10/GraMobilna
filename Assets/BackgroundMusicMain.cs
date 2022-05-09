using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicMain : MonoBehaviour
{
    void Awake()
    {
        FindObjectOfType<AudioManager>().Stop("menu-theme");
        FindObjectOfType<AudioManager>().Play("background-main");
    }

}
