using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Chest_Multiplayer : Collectable
{
    public Sprite emptyChest;
    public int pesosAmount = 10;
    protected override void OnCollect()
    {
        if (!collected)
        {
            collected = true;

            GetComponent<SpriteRenderer>().sprite = emptyChest;
            GameManager_Multiplayer.instance.player.pesos += pesosAmount;
            GameManager_Multiplayer.instance.ShowText("+" + pesosAmount + " coins", 25, Color.yellow, transform.position, Vector3.up * 50, 1.0f);
            FindObjectOfType<AudioManager>().Play("coinChest");
        }
    }
}
