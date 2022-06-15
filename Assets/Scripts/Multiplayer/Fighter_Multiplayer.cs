using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter_Multiplayer : MonoBehaviour
{
    public int currentHealth = 9;
    public int maxHealth = 10;
    public float pushRecoverySpeed = 0.2f;

    protected float immuneTime = 1.0f;
    protected float lastImmune;

    protected Vector3 pushDirection;

    protected virtual void ReceiveDamage(Damage dmg)
    {
        if (Time.time - lastImmune > immuneTime)
        {
            Debug.Log("Damage = " + dmg);
            Debug.Log("currBefore = " + currentHealth);
            lastImmune = Time.time;
            currentHealth -= dmg.damageAmount;
            Debug.Log("currAfter = " + currentHealth);
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;
        
        GameManager_Multiplayer.instance.ShowText(dmg.damageAmount.ToString(), 25, Color.red, transform.position, Vector3.zero, 0.5f);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Death();
        }
        }
    }

    protected virtual void Death()
    {

    }

}
