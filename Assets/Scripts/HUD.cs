using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HUD : MonoBehaviour
{
    public Text PotionsText;
    public GameObject PotionsButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PotionsText.text = GameManager.instance.potionsCount.ToString();
        Image img = PotionsButton.GetComponent<Image>();
        if(GameManager.instance.potionsCount <= 0)
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0.135f);
        else
            img.color = new Color(img.color.r, img.color.g, img.color.b, 1);
    }
}
