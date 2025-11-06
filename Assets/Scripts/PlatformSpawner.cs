using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab;    // Assign your platform prefab in the Unity Inspector
    public float blockOffsetY = 5f;     // Distance below the player to spawn the platform
    public float platformLifespan = 5f; // Time in seconds before the platform disappears
    private float cooldownTime = 1f;    // Cooldown time in seconds
    private float lastSpawnTime;

    private Rigidbody2D rb; // Reference to the Rigidbody2D for jump detection

    public ChangeCharacter changeCharacter; // Reference to the ChangeCharacter script

    AudioController audioController;

    void Start()
    {
        // Cache the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        audioController = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioController>();
    }

    void Update()
    {
        HandleBlockSpawning();
    }

    void HandleBlockSpawning()
    {
        // Check if platform ability is unlocked before allowing spawning
        if (changeCharacter.GetSelectedCharacter() == 1 && Input.GetKeyDown(KeyCode.E) && IsPlayerJumping() && Time.time >= lastSpawnTime + cooldownTime)
        {
            if (GameManager.Instance.isPlatformAbilityUnlocked)  // Ensure platform ability is unlocked
            {
                SpawnPlatformBelow();
                lastSpawnTime = Time.time; // Update the cooldown timer
            }
            else
            {
                Debug.Log("Platform ability is locked!");
            }
        }
    }

    void SpawnPlatformBelow()
    {
        // Get the player's current position
        Vector3 playerPosition = transform.position;

        // Calculate the spawn position directly below the player
        Vector3 spawnPosition = new Vector3(
            playerPosition.x,               // Same X as the player
            playerPosition.y - blockOffsetY, // Offset below the player
            playerPosition.z                // Same Z as the player (if in 3D)
        );

        // Instantiate the platform prefab at the calculated position
        GameObject newPlatform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);

        // Schedule the platform to be destroyed after the specified lifespan
        Destroy(newPlatform, platformLifespan);

        Debug.Log($"Platform spawned below at {spawnPosition}");

        audioController.PlaySfx(audioController.Magic);
    }

    bool IsPlayerJumping()
    {
        // Detect if the player is jumping based on vertical velocity
        return Mathf.Abs(rb.velocity.y) > 0.1f;
    }
}
