using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastPortal_Multiplayer : Collidable
{
    // Start is called before the first frame update
    public string[] sceneNames;
    public GameObject fade;
    protected override void OnCollide(Collider2D coll)
    {
        if (GameManager_Multiplayer.instance.isTransitioning)
            return;
        if (coll.name.Contains("Player"))
        {
            GameManager_Multiplayer.instance.isTransitioning = true;
            fade = GameObject.Find("LevelLoader");
            string sceneName = sceneNames[Random.Range(0, sceneNames.Length)];
            Destroy(GameObject.Find("HUD"));
            Destroy(GameManager_Multiplayer.instance.player);
            Destroy(GameManager_Multiplayer.instance.player2);
            Destroy(GameObject.Find("GameManager"));
            Destroy(GameObject.Find("InGameMenu"));
            Destroy(GameObject.Find("FloatingTextManager"));
            Destroy(GameObject.Find("UserStatisticsMenu"));
            fade.GetComponent<LevelLoader_Multiplayer>().LoadNextLevel(sceneName);
        }
    }
}
