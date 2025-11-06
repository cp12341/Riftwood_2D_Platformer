using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; // Import TextMeshPro namespace

public class LevelMenu : MonoBehaviour
{
    public Button[] buttons;
    public GameObject levelButtons;
    public Sprite lockedImage; // Image for locked state

    private void Awake()
    {
        ButtonsToArray();
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        for (int i = 0; i < buttons.Length; i++)
        {
            // Get the button's Image component
            Image buttonImage = buttons[i].GetComponent<Image>();

            // Get the TextMeshProUGUI component on the button (assumes it is a child)
            TextMeshProUGUI buttonText = buttons[i].GetComponentInChildren<TextMeshProUGUI>();

            if (i < unlockedLevel)
            {
                // Level is unlocked
                buttons[i].interactable = true; // Keep the unlocked image and text as they are
                if (buttonText != null)
                {
                    buttonText.enabled = true; // Ensure text is visible
                }
            }
            else
            {
                // Level is locked
                buttonImage.sprite = lockedImage; // Change to locked image
                buttons[i].interactable = false; // Disable the button
                if (buttonText != null)
                {
                    buttonText.enabled = false; // Hide the text
                }
            }
        }
    }

    public void OpenLevel(int levelId)
    {
        SceneManager.LoadScene(levelId);
    }

    void ButtonsToArray()
    {
        int childCount = levelButtons.transform.childCount;
        buttons = new Button[childCount];
        for (int i = 0; i < childCount; i++)
        {
            buttons[i] = levelButtons.transform.GetChild(i).gameObject.GetComponent<Button>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
