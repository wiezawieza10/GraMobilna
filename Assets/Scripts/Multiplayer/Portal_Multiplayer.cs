using UnityEngine;
public class Portal_Multiplayer : Collidable
{
    // Start is called before the first frame update
    public string[] sceneNames;
    public GameObject fade;
    protected override void OnCollide(Collider2D coll)
    {
        if (GameManager_Multiplayer.instance.isTransitioning)
            return;
        if(coll.name.Contains("Player"))
        {
            //Debug.Log("Portal used");
            GameManager_Multiplayer.instance.isTransitioning = true;
            fade = GameObject.Find("LevelLoader");
            string sceneName = sceneNames[Random.Range(0, sceneNames.Length)];
            fade.GetComponent<LevelLoader_Multiplayer>().LoadNextLevel(sceneName);
        }
    }
}
