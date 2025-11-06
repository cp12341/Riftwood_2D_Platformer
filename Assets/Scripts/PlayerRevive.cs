using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRevive : MonoBehaviour
{
    public float fallThresholdY = -10f;  // Y-position threshold for falling
    public int fallDamage = 20;          // Damage taken when falling

    [SerializeField] private SharedHealth sharedHealth;  // Reference to SharedHealth

    private void Start()
    {
        // Auto-assign SharedHealth if not set in the Inspector
        if (sharedHealth == null)
        {
            sharedHealth = FindObjectOfType<SharedHealth>();
            if (sharedHealth == null)
            {
                Debug.LogError("SharedHealth reference is missing in PlayerRevive.");
            }
        }
    }

    private void Update()
    {
        if (transform.position.y < fallThresholdY)
        {
            HandleFall();
        }
    }

    private void HandleFall()
    {
        if (sharedHealth != null)
        {
            sharedHealth.TakeDamage(fallDamage);

            if (sharedHealth.CurrentHealth > 0)
            {
                Vector2 respawnPosition = Checkpoint.GetLastCheckpoint();
                if (respawnPosition == Vector2.zero)
                {
                    respawnPosition = new Vector2(0, 0);  // Default starting position
                }

                transform.position = respawnPosition;
                Debug.Log($"Player revived at: {respawnPosition}");
            }
            else
            {
                Debug.Log("Player died from fall damage. Game Over.");
                HandleGameOver();
            }
        }
    }

    private void HandleGameOver()
    {
        // Implement game-over logic here, such as restarting the level or showing a game-over UI.
        Debug.Log("Game Over: Restart or show Game Over UI.");
        Checkpoint.ResetCheckpoint();  // Reset checkpoint on game over
        Debug.Log("Game Over: Checkpoints reset.");
    }

}
