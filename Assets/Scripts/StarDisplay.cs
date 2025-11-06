using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarDisplay : MonoBehaviour
{
    [SerializeField] private GameObject levelEndPanel;  // UI Panel for level-end display
    [SerializeField] private Text collectedCoinsText;   // Text to display collected coins
    [SerializeField] private Image[] starImages;        // Array of star images
    [SerializeField] private Sprite filledStarSprite;   // Sprite for filled star
    [SerializeField] private Sprite emptyStarSprite;    // Sprite for empty star
    [SerializeField] private int totalCoins;            // Total coins in the level

    private int collectedCoins;                         // Coins collected by the player

    public void SetTotalCoins(int total)
    {
        totalCoins = total; // Set total coins at the start of the level
    }

    public void UpdateCollectedCoins(int coins)
    {
        collectedCoins = coins; // Update the current coin count
    }

    public void DisplayStars()
    {
        // Activate the level-end panel
        levelEndPanel.SetActive(true);

        // Update the collected coins text
        collectedCoinsText.text = $"Coins Collected: {collectedCoins}/{totalCoins}";

        // Calculate the star rating
        float percentage = (float)collectedCoins / totalCoins;

        for (int i = 0; i < starImages.Length; i++)
        {
            if (percentage >= (i + 1) / 3f) // Each star represents 1/3 of total coins
            {
                starImages[i].sprite = filledStarSprite;
            }
            else
            {
                starImages[i].sprite = emptyStarSprite;
            }
        }
    }
}
