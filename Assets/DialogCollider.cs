using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogCollider : Collidable
{
    public GameObject DialogueBox;
    public GameObject DialogManager;
    public GameObject DialogColliderObject;
    protected override void OnCollide(Collider2D coll)
    {
        if (coll.tag == "Fighter" && coll.name == "Player")
        {
            Debug.Log("DIALOG");
            DialogColliderObject.SetActive(false);
            DialogueBox.SetActive(true);
            DialogManager.SetActive(true);
        }
    }
}
