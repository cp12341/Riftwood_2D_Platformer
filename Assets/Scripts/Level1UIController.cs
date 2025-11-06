using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Level1UIController : MonoBehaviour
{
    public GameObject messagePanel;
    public TextMeshProUGUI welcomeMessage;
    public TextMeshProUGUI meetCharacter;
    public TextMeshProUGUI meetCharacter2;
    public TextMeshProUGUI controlsTutorial;
    public TextMeshProUGUI keyPrompt;
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

        // Show Meet Character Message
        meetCharacter.gameObject.SetActive(true);
        yield return WaitForPlayerInput();
        meetCharacter.gameObject.SetActive(false);
        
        meetCharacter2.gameObject.SetActive(true);
        yield return WaitForPlayerInput();
        meetCharacter2.gameObject.SetActive(false);

        // Show Basic Controls Tutorial
        controlsTutorial.gameObject.SetActive(true);
        yield return WaitForPlayerInput();
        controlsTutorial.gameObject.SetActive(false);
        
        // Show Key Prompt
        keyPrompt.gameObject.SetActive(true);
        yield return WaitForPlayerInput();
        keyPrompt.gameObject.SetActive(false);

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
