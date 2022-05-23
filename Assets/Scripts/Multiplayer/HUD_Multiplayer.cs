using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HUD_Multiplayer : MonoBehaviour
{
    public Text PotionsText;
    public GameObject PotionsButton;
    void LateUpdate()
    {
        PotionsText.text = GameManager_Multiplayer.instance.player.potionsCount.ToString();
        Image img = PotionsButton.GetComponent<Image>();
        if(GameManager_Multiplayer.instance.player.potionsCount <= 0)
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0.135f);
        else
            img.color = new Color(img.color.r, img.color.g, img.color.b, 1);
    }
}
