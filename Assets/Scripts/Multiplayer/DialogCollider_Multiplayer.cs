using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DialogCollider_Multiplayer : Collidable
{
    public GameObject DialogueBox;
    public GameObject DialogManager;
    public GameObject DialogColliderObject;
    PhotonView view;
    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }
    protected override void OnCollide(Collider2D coll)
    {
        if (coll.tag == "Fighter" && coll.name.Contains("Player") && coll.gameObject == GameManager_Multiplayer.instance.currentPlayer)
        {
            Debug.Log("DIALOG");
            DialogColliderObject.SetActive(false);
            DialogueBox.SetActive(true);
            DialogManager.SetActive(true);
        }
    }
}
