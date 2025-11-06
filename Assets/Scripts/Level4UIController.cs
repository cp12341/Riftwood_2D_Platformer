using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level4UIController : MonoBehaviour
{
 
    public GameObject messagePanel;
    public TextMeshProUGUI welcomeMessage;

    public TextMeshProUGUI PurpleDialogueMessage;
    public TextMeshProUGUI OrangeDialogueMessage;

    public GameObject nextButton;

    private bool isNextClicked = false;
    
    // Flags to check if the dialogue has already been shown
    private bool hasShownPurpleDialogue = false;
    private bool hasShownOrangeDialogue = false;

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


        messagePanel.SetActive(false);
        nextButton.SetActive(false);

        // Add any additional flow here
    }

    private IEnumerator ShowPurpleDialogue()
    {
        if (!hasShownPurpleDialogue)
        {
            messagePanel.SetActive(true);
            PurpleDialogueMessage.gameObject.SetActive(true);
            nextButton.SetActive(true);

            yield return WaitForPlayerInput();
            PurpleDialogueMessage.gameObject.SetActive(false);

            messagePanel.SetActive(false);
            nextButton.SetActive(false);

            hasShownPurpleDialogue = true;  // Mark as shown
        }
    }

    private IEnumerator ShowOrangeDialogue()
    {
        if (!hasShownOrangeDialogue)
        {
            messagePanel.SetActive(true);
            OrangeDialogueMessage.gameObject.SetActive(true);
            nextButton.SetActive(true);

            yield return WaitForPlayerInput();
            OrangeDialogueMessage.gameObject.SetActive(false);

            messagePanel.SetActive(false);
            nextButton.SetActive(false);

            hasShownOrangeDialogue = true;  // Mark as shown
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
            if (CompareTag("Area1") && !hasShownPurpleDialogue)  // Check if dialogue hasn't been shown
            {
                StartCoroutine(ShowPurpleDialogue());
            }
            else if (CompareTag("Area2") && !hasShownOrangeDialogue)  // Check if dialogue hasn't been shown
            {
                StartCoroutine(ShowOrangeDialogue());
            }
        }
    }
 

}
