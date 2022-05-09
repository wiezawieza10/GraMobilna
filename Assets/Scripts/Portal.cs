using UnityEngine;
public class Portal : Collidable
{
    // Start is called before the first frame update
    public string[] sceneNames;
    public GameObject fade;
    bool alreadyCollided = false; 
    protected override void OnCollide(Collider2D coll)
    {

        if (GameManager.instance.isTransitioning)
            return;
        if(coll.name == "Player")
        {
            GameManager.instance.isTransitioning = true;
            GameManager.instance.SaveState();
            fade = GameObject.Find("LevelLoader");
            string sceneName = sceneNames[Random.Range(0, sceneNames.Length)];
            fade.GetComponent<LevelLoader>().LoadNextLevel(sceneName);
        }
    }
}
