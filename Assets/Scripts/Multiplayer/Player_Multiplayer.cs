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

    public void SwapSprite(int skinId)
    {
        spriteRenderer.sprite = GameManager_Multiplayer.instance.playerSprites[skinId];
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

    public void Heal(int healingAmount)
    {
        if (currentHealth == maxHealth)
            return;

        currentHealth += healingAmount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        GameManager_Multiplayer.instance.ShowText("+" + healingAmount.ToString() + "hp", 25, Color.green, transform.position, Vector3.up * 30, 1.0f);
        GameManager_Multiplayer.instance.OnHitpointChange();
    }

    public void usePotion(int healingAmount)
    {
        if (GameManager_Multiplayer.instance.potionsCount > 0 && currentHealth < maxHealth)
        {
            GameManager_Multiplayer.instance.potionsCount--;

            FindObjectOfType<AudioManager>().Play("potionDrink");
            Heal(healingAmount);

        }
    }
    [PunRPC]
    public void Respawn()
    {
        Heal(maxHealth);
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
}
