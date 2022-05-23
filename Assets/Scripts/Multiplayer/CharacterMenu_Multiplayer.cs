using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterMenu_Multiplayer : MonoBehaviour
{
    public Text levelText, hitpointText, pesosText, upgradeCostText, xpText;
    public Image weaponSprite;
    public RectTransform xpBar;

    // Weapon Upgrade

    public void OnUpgradeClick()
    {
        if (GameManager_Multiplayer.instance.TryUpgradeWeapon())
            UpdateMenu();
    }

    public void UpdateMenu()
    {
        hitpointText.text = GameManager_Multiplayer.instance.player.currentHealth.ToString();
        pesosText.text = GameManager_Multiplayer.instance.player.pesos.ToString();
        levelText.text = GameManager_Multiplayer.instance.GetCurrentLevel().ToString();
        weaponSprite.sprite = GameManager_Multiplayer.instance.weaponSprites[GameManager_Multiplayer.instance.weapon.weaponLevel];
        if (GameManager_Multiplayer.instance.weapon.weaponLevel == GameManager_Multiplayer.instance.weaponPrices.Count)
            upgradeCostText.text = "MAX";
        else
            upgradeCostText.text = GameManager_Multiplayer.instance.weaponPrices[GameManager_Multiplayer.instance.weapon.weaponLevel].ToString();

        int currLevel = GameManager_Multiplayer.instance.GetCurrentLevel();
        if(currLevel == GameManager_Multiplayer.instance.xpTable.Count)
        {
            xpText.text = GameManager_Multiplayer.instance.player.experience.ToString() + " total experience points";
            xpBar.localScale = Vector3.one;
        }
        else
        {
            int previousLevelXp = GameManager_Multiplayer.instance.GetXpToLevel(currLevel-1);
            int currLevelXp = GameManager_Multiplayer.instance.GetXpToLevel(currLevel);
            int diff = currLevelXp - previousLevelXp;
            int currXpIntoLevel = GameManager_Multiplayer.instance.player.experience - previousLevelXp;

            float completionRatio = (float)currXpIntoLevel / (float)diff;
            xpBar.localScale = new Vector3(completionRatio, 1, 1);
            xpText.text = currXpIntoLevel.ToString() + " / " + diff;
        }
    }

    
}
