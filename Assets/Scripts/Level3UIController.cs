using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level3UIController : MonoBehaviour
{

    public GameObject messagePanel;
    public TextMeshProUGUI welcomeMessage;
   
    public GameObject nextButton;

    private bool isNextClicked = false;

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
}
