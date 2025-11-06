using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    // Notify all Damageable components of health changes
    public delegate void HealthChangedDelegate(int currentHealth, int maxHealth);
    public event HealthChangedDelegate OnHealthChanged;


    public GameObject gameOverScreen; // Assign the Game Over Panel in the Inspector.


    public int CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            OnHealthChanged?.Invoke(currentHealth, maxHealth); // Notify listeners
            Debug.Log($"SharedHealth updated: {currentHealth}/{maxHealth}");

            if (currentHealth <= 0)
            {
                HandleDeath();
                GameOver();
            }
        }
    }

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
    }

    private void HandleDeath()
    {
        Debug.Log("SharedHealth: Players have died.");
        // Add player death logic, such as resetting the level or showing a game over screen.
    }

    private void GameOver()
    {
        // Start a coroutine to wait before showing the Game Over screen
        StartCoroutine(ShowGameOverScreenAfterDelay(1f)); // 2 seconds delay
    }

    private IEnumerator ShowGameOverScreenAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);
        
        // Show the Game Over screen
        gameOverScreen.SetActive(true); 
        Time.timeScale = 0; // Pause the game
    }

    // void GameOver()
    // {
    //     gameOverScreen.SetActive(true); // Show the Game Over screen.
    //     Time.timeScale = 0; // Pause the game.
    // }

}
