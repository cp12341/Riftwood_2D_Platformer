using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level5UIController : MonoBehaviour
{
    public GameObject messagePanel;
    public TextMeshProUGUI welcomeMessage;
    public TextMeshProUGUI BossDialogueMessage;

    public GameObject nextButton;

    private bool isNextClicked = false;
    
    // Flags to check if the dialogue has already been shown
    private bool hasShownBossDialogue = false;
   

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

    private IEnumerator ShowBossDialogue()
    {
        if (!hasShownBossDialogue)
        {
            messagePanel.SetActive(true);
            BossDialogueMessage.gameObject.SetActive(true);
            nextButton.SetActive(true);

            yield return WaitForPlayerInput();
            BossDialogueMessage.gameObject.SetActive(false);

            messagePanel.SetActive(false);
            nextButton.SetActive(false);

            hasShownBossDialogue = true;  // Mark as shown
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
            if (CompareTag("Area1") && !hasShownBossDialogue)  // Check if dialogue hasn't been shown
            {
                StartCoroutine(ShowBossDialogue());
            }

        }
    }
 
}
