using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DialogButcher : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    private int index;
    public float typingSpeed;
    public GameObject yesButton;
    public GameObject noButton;
    public GameObject continueButton;
    public GameObject endButton;
    public Animator textDisplayAnim;
    public GameObject DialogueBox;
    public GameObject HUD;
    public GameObject Player;
    private void Start()
    {
        StartCoroutine(Type());
        HUD = GameObject.Find("HUD");
        Player = GameObject.Find("Player");
        HUD.SetActive(false);
        GameManager.instance.notInDialog = false;
        FindObjectOfType<AudioManager>().Play("HowCanIHelpYou");
    }

    private void Update()
    {
        if (textDisplay.text == sentences[1])
        {
            yesButton.SetActive(true);
            noButton.SetActive(true);
        }

        if (textDisplay.text == sentences[index] && index != 1)
        {
            continueButton.SetActive(true);
        }
    }
    IEnumerator Type()
    {
        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

    }

    public void NoButtonClicked()
    {
        textDisplayAnim.SetTrigger("Change");
        textDisplay.text = "";
        yesButton.SetActive(false);
        noButton.SetActive(false);
        continueButton.SetActive(false);
        DialogueBox.SetActive(false);
        HUD.SetActive(true);
        GameManager.instance.notInDialog = true;
    }

    public void NextSentenceAfterYes()
    {
        textDisplayAnim.SetTrigger("Change");
        yesButton.SetActive(false);
        noButton.SetActive(false);
        continueButton.SetActive(false);
        Debug.Log("CHECK2");
        if (index < sentences.Length - 1 && index!=1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
            Debug.Log("CHECK3");
        }
        else
        {
            Debug.Log("CHECK4");
            if (GameManager.instance.pesos >= 70)
            {
                Debug.Log("CHECK5");
                GameManager.instance.pesos -= 70;
                GameManager.instance.potionsCount += 3;
            }
            else if (GameManager.instance.pesos < 70)
            {
                textDisplay.text = "Brakuje Ci monet!";
                yesButton.SetActive(false);
                noButton.SetActive(false);
                continueButton.SetActive(false);
                endButton.SetActive(true);
                return;
            }
            textDisplay.text = "";
            yesButton.SetActive(false);
            noButton.SetActive(false);
            continueButton.SetActive(false);
            DialogueBox.SetActive(false);
            HUD.SetActive(true);
            GameManager.instance.notInDialog = true;
        }
    }

    public void Continue()
    {
        textDisplayAnim.SetTrigger("Change");
        continueButton.SetActive(false);
        if (index < sentences.Length - 1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        }
        else
        {
            textDisplay.text = "";
            continueButton.SetActive(false);
            DialogueBox.SetActive(false);
            HUD.SetActive(true);
            GameManager.instance.notInDialog = true;
        }
    }
    public void EndButton()
    {
        textDisplayAnim.SetTrigger("Change");
        textDisplay.text = "";
        yesButton.SetActive(false);
        noButton.SetActive(false);
        continueButton.SetActive(false);
        endButton.SetActive(false);
        DialogueBox.SetActive(false);
        HUD.SetActive(true);
        GameManager.instance.notInDialog = true;
    }
}