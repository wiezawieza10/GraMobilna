using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class HealingFountain_Multiplayer : Collidable
{
    public int healingAmount = 1;
    private float healCooldown = 1.0f;
    private float lastHeal;
    PhotonView view;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.tag == "Fighter" && coll.name.Contains("Player") && coll.gameObject == GameManager_Multiplayer.instance.currentPlayer)
            if (Time.time - lastHeal > healCooldown)
            {
                lastHeal = Time.time;
                GameManager_Multiplayer.instance.Heal(healingAmount);
            }
    }
}
