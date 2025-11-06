using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private SharedHealth sharedHealth; // Reference to SharedHealth
    [SerializeField] private GameObject heartPrefab;    // Prefab for a single heart
    [SerializeField] private Transform heartContainer;  // Parent object for hearts
    [SerializeField] private Sprite fullHeartSprite;    // Full heart sprite
    [SerializeField] private Sprite halfHeartSprite;    // Half heart sprite
    [SerializeField] private Sprite emptyHeartSprite;   // Empty heart sprite

    private List<Image> hearts = new List<Image>();

    private void Start()
    {
        if (sharedHealth != null)
        {
            sharedHealth.OnHealthChanged += UpdateHearts;
            InitializeHearts(sharedHealth.maxHealth);
        }
    }

    private void OnDestroy()
    {
        if (sharedHealth != null)
        {
            sharedHealth.OnHealthChanged -= UpdateHearts;
        }
    }

    private void InitializeHearts(int maxHealth)
    {
        int heartCount = Mathf.CeilToInt(maxHealth / 20f); // Each heart represents 20 health points
        for (int i = 0; i < heartCount; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartContainer);
            hearts.Add(heart.GetComponent<Image>());
        }
    }

    private void UpdateHearts(int currentHealth, int maxHealth)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            int heartHealth = (i + 1) * 20; // Each heart's threshold

            if (currentHealth >= heartHealth)
            {
                hearts[i].sprite = fullHeartSprite;
            }
            else if (currentHealth >= heartHealth - 10)
            {
                hearts[i].sprite = halfHeartSprite;
            }
            else
            {
                hearts[i].sprite = emptyHeartSprite;
            }
        }
    }
}
