using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Player_Multiplayer : Mover_Multiplayer
{
    private SpriteRenderer spriteRenderer;
    private bool isAlive = true;
    public Joystick joystick;
    public int potionsCount;
    public int pesos;
    public int experience;
    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        joystick = FindObjectOfType<Joystick>();
        GameManager_Multiplayer.instance.SetPlayer();
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        if (!isAlive)
            return;
        base.ReceiveDamage(dmg);
        FindObjectOfType<AudioManager>().Play("player-getHit");
        GameManager_Multiplayer.instance.OnHitpointChange();
        //Debug.Log("PLAYER RECEIVE DAMAGE");
    }
    protected override void Death()
    {
        view.RPC("PlayerDeathRPC", RpcTarget.AllBuffered);
    }
    [PunRPC]
    private void PlayerDeathRPC()
    {
        isAlive = false;
        GameManager_Multiplayer.instance.deathMenu.SetTrigger("Show");
    }
    private void FixedUpdate()
    {
        if(view.IsMine)
        {
            float x = joystick.Horizontal;
            float y = joystick.Vertical;

            if (isAlive && GameManager_Multiplayer.instance.notInDialog)
                UpdateMotor(new Vector3(x, y, 0));

            if (GameManager_Multiplayer.instance.notInDialog == false)
                UpdateMotor(new Vector3(0, 0, 0));
        }
    }

    public void OnLevelUp()
    {
        maxHealth += 2;
        currentHealth = maxHealth;
    }

    public void SetLevel(int level)
    {
        for (int i = 0; i < level; i++)
            OnLevelUp();
    }

    [PunRPC]
    public void Respawn()
    {
        GameManager_Multiplayer.instance.Heal(maxHealth);
        isAlive = true;
        lastImmune = Time.time;
        pushDirection = Vector3.zero;
        transform.position = GameObject.Find("SpawnPoint").transform.position;
        //FindObjectOfType<AudioManager>().Play("respawn");
    }
    public void SetJoystick(Joystick joystickObject)
    {
        joystick = joystickObject;
    }

    [PunRPC]
    private void FlipTrue()
    {
        transform.localScale = new Vector3(-1, 1, 1);
    }

    [PunRPC]
    private void FlipFalse()
    {
        transform.localScale = Vector3.one;
    }

    [PunRPC]
    public void UpgradePlayer2Weapon(int level)
    {
        Debug.Log("UpgradePlayer2Weapon");
        GameManager_Multiplayer.instance.SetPlayer();
        GameManager_Multiplayer.instance.weapon2.spriteRenderer.sprite = GameManager_Multiplayer.instance.weaponSprites[level];
    }

    [PunRPC]
    public void SwingRPC()
    {
        GameManager_Multiplayer.instance.player2isAttackButtonDown = true;
    }
}
