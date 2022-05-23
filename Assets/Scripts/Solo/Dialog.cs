using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    private int index;
    public float typingSpeed;
    public GameObject continueButton;
    public Animator textDisplayAnim;
    public GameObject DialogueBox;
    public GameObject HUD;
    public GameObject Player;
    public GameObject Weapon;
    private void Start()
    {
        StartCoroutine(Type());
        HUD = GameObject.Find("HUD");
        Player = GameObject.Find("Player");
        HUD.SetActive(false);
    }

    private void Update()
    {
        if (textDisplay.text == sentences[index])
        {
            continueButton.SetActive(true);
        }
    }
    IEnumerator Type()
    {
        foreach(char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
            
    }
    
    public void NextSentence()
    {
        textDisplayAnim.SetTrigger("Change");
        continueButton.SetActive(false);
        if(index < sentences.Length - 1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        } else
        {
            textDisplay.text = "";
            continueButton.SetActive(false);
            DialogueBox.SetActive(false);
            HUD.SetActive(true);
            Player.GetComponent<SpriteRenderer>().flipX = false;
            Weapon.SetActive(true);
        }
    }
}
