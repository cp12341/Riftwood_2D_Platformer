using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level2UIController : MonoBehaviour
{
    public GameObject messagePanel;
    public TextMeshProUGUI welcomeMessage;

    public TextMeshProUGUI BlockDialogueMessage;
    public TextMeshProUGUI PotionDialogueMessage;
    public TextMeshProUGUI controlsTutorial;
    public GameObject nextButton;

    private bool isNextClicked = false;
    
    // Flags to check if the dialogue has already been shown
    private bool hasShownBlockDialogue = false;
    private bool hasShownPotionDialogue = false;

    void Start()
    {
        StartCoroutine(ShowMessages());
    }

    private IEnumerator ShowMessages()
    {
        messagePanel.SetActive(true);

        // Show Welcome Message
        welcomeMessage.gameObject.SetActive(true);
        nextButton.SetActive(true);
        yield return WaitForPlayerInput();
        welcomeMessage.gameObject.SetActive(false);

        // Show Basic Controls Tutorial
        controlsTutorial.gameObject.SetActive(true);
        yield return WaitForPlayerInput();
        controlsTutorial.gameObject.SetActive(false);

        messagePanel.SetActive(false);
        nextButton.SetActive(false);

        // Add any additional flow here
    }

    private IEnumerator ShowBlockDialogue()
    {
        if (!hasShownBlockDialogue)
        {
            messagePanel.SetActive(true);
            BlockDialogueMessage.gameObject.SetActive(true);
            nextButton.SetActive(true);

            yield return WaitForPlayerInput();
            BlockDialogueMessage.gameObject.SetActive(false);

            messagePanel.SetActive(false);
            nextButton.SetActive(false);

            hasShownBlockDialogue = true;  // Mark as shown
        }
    }

    private IEnumerator ShowPotionDialogue()
    {
        if (!hasShownPotionDialogue)
        {
            messagePanel.SetActive(true);
            PotionDialogueMessage.gameObject.SetActive(true);
            nextButton.SetActive(true);

            yield return WaitForPlayerInput();
            PotionDialogueMessage.gameObject.SetActive(false);

            messagePanel.SetActive(false);
            nextButton.SetActive(false);

            hasShownPotionDialogue = true;  // Mark as shown
        }
    }

    private IEnumerator WaitForPlayerInput()
    {
        isNextClicked = false;

        // Wait until the player clicks to continue
        while (!isNextClicked)
        {
            yield return null; // Wait for the next frame
        }
    }

    public void OnNextButtonClicked()
    {
        isNextClicked = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            Debug.Log("Player entered trigger: " + other.gameObject.name);
            if (CompareTag("Area1") && !hasShownBlockDialogue)  // Check if dialogue hasn't been shown
            {
                StartCoroutine(ShowBlockDialogue());
            }
            else if (CompareTag("Area2") && !hasShownPotionDialogue)  // Check if dialogue hasn't been shown
            {
                StartCoroutine(ShowPotionDialogue());
            }
        }
    }
}
