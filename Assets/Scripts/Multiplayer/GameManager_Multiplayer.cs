using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
public class GameManager_Multiplayer : MonoBehaviour
{
    public static GameManager_Multiplayer instance;
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;

    public Player_Multiplayer player;
    public Player_Multiplayer player2;
    public Weapon_Multiplayer weapon;
    public Weapon_Multiplayer weapon2;
    public LevelLoader_Multiplayer levelLoader;
    public FloatingTextManager floatingTextManager;
    public RectTransform hitpointBar;
    public GameObject hud;
    public GameObject menu;
    public GameObject inGameMenu;
    public Animator deathMenu;
    public bool isAttackButtonDown;
    public bool player2isAttackButtonDown;
    public bool notInDialog;
    public string currMap;
    public string savedMap;
    public bool wasGameLoaded;
    public int weaponLevele;
    public bool isTransitioning;
    public GameObject joystickHandle;
    public GameObject currentPlayer;
    private void Awake()
    {
        //Debug.Log("GameManager awake");
        if (instance != null)
        {
            Debug.Log("Other GameManager instance found");
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(floatingTextManager.gameObject);
            Destroy(hud);
            Destroy(menu);
            Destroy(inGameMenu);
            Destroy(levelLoader);
        }

        if (SceneManager.GetActiveScene().name == "StartMenu")
        {
            
            Destroy(player.gameObject);
            Destroy(floatingTextManager.gameObject);
            Destroy(hud);
            Destroy(menu);
            Destroy(inGameMenu);
            Destroy(levelLoader);
            Destroy(gameObject);
        }
        instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
        notInDialog = true;

    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name != "Main_Multiplayer")
            OnHitpointChange();
    }
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    public bool TryUpgradeWeapon()
    {
        if (weaponPrices.Count <= weapon.weaponLevel)
            return false;

        if(player.pesos >= weaponPrices[weapon.weaponLevel])
        {
            player.pesos -= weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();
            return true;
        }

        return false;
    }

    public void OnHitpointChange()
    {
        float ratio = (float)player.currentHealth / (float)player.maxHealth;
        hitpointBar.localScale = new Vector3(1, ratio, 1);
    }

    public int GetXpToLevel(int level)
    {
        int r = 0;
        int xp = 0;

        while (r < level)
        {
            xp += xpTable[r];
            r++;
        }

        return xp;
    }
    public int GetCurrentLevel()
    {
        int r = 0;
        int add = 0;

        while(player.experience >= add)
        {
            add += xpTable[r];
            r++;

            if (r == xpTable.Count)
                return r;
        }

        return r;
    }

    public void GrantXp(int xp)
    {
        int currLevel = GetCurrentLevel();
        player.experience += xp;
        player.potionsCount++;
        if (currLevel < GetCurrentLevel())
            OnLevelUp();
    }

    public void OnLevelUp()
    {
        Debug.Log("Level up");
        player.OnLevelUp();
        OnHitpointChange();
    }    

    public void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "StartMenu")
        {
            Destroy(player.gameObject);
            Destroy(floatingTextManager.gameObject);
            Destroy(hud);
            Destroy(menu);
            Destroy(inGameMenu);
            Destroy(levelLoader);
            Destroy(gameObject);
            return;
        }

        var players = FindObjectsOfType<Player_Multiplayer>();
        if (players.Length > 0 )
            for (var i =0; i < players.Length; ++i)
                players[i].transform.position = GameObject.Find("SpawnPoint").transform.position;
        currMap = SceneManager.GetActiveScene().name;
        if(joystickHandle != null && currMap != "Main_Multiplayer")
            joystickHandle.transform.localPosition = new Vector3(0, 0, 0);
    }

    
    public void Respawn()
    {
        deathMenu.SetTrigger("Hide");
        PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().name);
        player.GetComponent<PhotonView>().RPC("Respawn", RpcTarget.AllBuffered);
    }
    public void ColorPlayer()
    {
        var sprite = player2.GetComponent<SpriteRenderer>();
        sprite.color = new Color(255, 153, 250, 255);
    }

        public void SetPlayer()
    {
        
        var players = FindObjectsOfType<Player_Multiplayer>();
        if (players.Length > 1)
            for (var i = 0; i < players.Length; ++i)
            {
                if (players[i].GetComponent<PhotonView>().IsMine)
                {

                    player = players[i];
                    
                }
                else
                    player2 = players[i];
            }
        else
            player = FindObjectOfType<Player_Multiplayer>();


        var weapons = FindObjectsOfType<Weapon_Multiplayer>();
        if (weapons.Length > 1)
            for (var i = 0; i < weapons.Length; ++i)
            {
                if (weapons[i].GetComponentInParent<PhotonView>().IsMine)
                {
                    weapon = weapons[i];
                }
                else
                    weapon2 = weapons[i];


            }
        else
            weapon = FindObjectOfType<Weapon_Multiplayer>();
    }
    public void Heal(int healingAmount)
    {
        if (player.currentHealth == player.maxHealth)
            return;

        player.currentHealth += healingAmount;
        if (player.currentHealth > player.maxHealth)
            player.currentHealth = player.maxHealth;
        ShowText("+" + healingAmount.ToString() + "hp", 25, Color.green, player.transform.position, Vector3.up * 30, 1.0f);
        OnHitpointChange();
        player.view.RPC("HealPlayer2", RpcTarget.OthersBuffered, healingAmount);
    }
    public void HealPlayer2(int healingAmount)
    {
        if (player2.currentHealth == player2.maxHealth)
            return;

        player2.currentHealth += healingAmount;
        if (player2.currentHealth > player2.maxHealth)
            player2.currentHealth = player2.maxHealth;
        ShowText("+" + healingAmount.ToString() + "hp", 25, Color.green, player2.transform.position, Vector3.up * 30, 1.0f);
    }
    public void usePotion(int healingAmount)
    {
        if (player.potionsCount > 0 && player.currentHealth < player.maxHealth)
        {
            player.potionsCount--;

            FindObjectOfType<AudioManager>().Play("potionDrink");
            Heal(healingAmount);

        }
    }

}
