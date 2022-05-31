using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Weapon_Multiplayer : Collidable
{
    // Damage
    public int[] damagePoint = { 1, 2, 3, 4, 5, 6, 7, 8 };
    public float[] pushForce = { 2.0f, 2.2f, 2.4f, 2.6f, 2.8f, 3.0f, 3.4f, 4.0f };

    //upgrade
    public int weaponLevel = 0;
    public SpriteRenderer spriteRenderer;

    //Swing
    private Animator anim;
    private float cooldown = 0.5f;
    private float lastSwing;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        
    }

    protected override void Update()
    {
        base.Update();
        if (GameManager_Multiplayer.instance.isAttackButtonDown == true && this.gameObject.GetComponentInParent<Player_Multiplayer>().view.IsMine)
            if (Time.time - lastSwing > cooldown)
            {
                lastSwing = Time.time;
                Swing();
            }

        if(GameManager_Multiplayer.instance.player2isAttackButtonDown == true && !this.gameObject.GetComponentInParent<Player_Multiplayer>().view.IsMine)
        {
            Swingp2();
            GameManager_Multiplayer.instance.player2isAttackButtonDown = false;
        }
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.tag == "Fighter")
        {
            if (coll.name.Contains("Player"))
                return;

            Damage dmg = new Damage
            {
                damageAmount = damagePoint[weaponLevel],
                origin = transform.position,
                pushForce = pushForce[weaponLevel]
            };

            coll.SendMessage("ReceiveDamage", dmg);
        }
    }
    private void Swing()
    {
        GameManager_Multiplayer.instance.player.view.RPC("SwingRPC", RpcTarget.Others);
        anim.SetTrigger("Swing");
        FindObjectOfType<AudioManager>().Play("swing1");
    }

    private void Swingp2()
    {
        anim.SetTrigger("Swing");
    }

    public void UpgradeWeapon()
    {
        weaponLevel++;
        GameManager_Multiplayer.instance.weaponLevele = weaponLevel;
        spriteRenderer.sprite = GameManager_Multiplayer.instance.weaponSprites[weaponLevel];
        GameManager_Multiplayer.instance.player.view.RPC("UpgradePlayer2Weapon", RpcTarget.All, weaponLevel);
        FindObjectOfType<AudioManager>().Play("weaponUpgrade");
    }

    public void SetWeaponLevel(int level)
    {
        weaponLevel = level;
        spriteRenderer.sprite = GameManager_Multiplayer.instance.weaponSprites[weaponLevel];
    }
}
