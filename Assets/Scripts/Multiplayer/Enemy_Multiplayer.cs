using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Enemy_Multiplayer : Mover_Multiplayer
{
    public int xpValue = 1;

    public float triggerLenght = 1;
    public float chaseLenght = 5;
    private bool chasing;
    private bool collidingWithPlayer;
    private Transform playerTransform;
    private Vector3 startingPosition;

    //hitbox
    public ContactFilter2D filter;
    private BoxCollider2D hitbox;
    private Collider2D[] hits = new Collider2D[10];
    protected override void Start()
    {
        base.Start();
        playerTransform = GameManager_Multiplayer.instance.player.transform;
        startingPosition = transform.position;
        hitbox = transform.GetChild(0).GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        //If player in range
        if (Vector3.Distance(playerTransform.position, startingPosition) < chaseLenght)
        {
            if (Vector3.Distance(playerTransform.position, startingPosition) < triggerLenght)
                chasing = true;

            if (chasing)
            {
                if (!collidingWithPlayer)
                {
                    UpdateMotor((playerTransform.position - transform.position).normalized);
                }
            }
            else
            {
                UpdateMotor(startingPosition - transform.position);
            }
        }
        else
        {
            UpdateMotor(startingPosition - transform.position);
            chasing = false;
        }

        collidingWithPlayer = false;
        boxCollider.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
                continue;
            
            if(hits[i].tag == "Fighter" && hits[i].name.Contains("Player"))
            {
                collidingWithPlayer = true;
            }


            hits[i] = null;
        }
    }
    protected override void Death()
    {
        view.RPC("EnemyDeathRPC", RpcTarget.AllBuffered);
        if(view.IsMine)
        {
            FindObjectOfType<AudioManager>().Play("enemyDeath");
            GameManager_Multiplayer.instance.GrantXp(xpValue);
        }
    }

    [PunRPC]
    private void EnemyDeathRPC()
    {
        Destroy(gameObject);
        GameManager_Multiplayer.instance.ShowText("+" + xpValue + " xp", 30, Color.magenta, transform.position, Vector3.up * 40, 1.0f);
    }

    [PunRPC]
    private void EnemyFlipTrue()
    {
        transform.localScale = new Vector3(-1, 1, 1);
    }

    [PunRPC]
    private void EnemyFlipFalse()
    {
        transform.localScale = Vector3.one;
    }
}