using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;  // Add this to access Cinemachine classes

public class ChangeCharacter : MonoBehaviour
{
    private int selectedCharacter = 1;
    private int totalCharacters = 2;

    public CinemachineVirtualCamera virtualCamera; // Reference to the Cinemachine Virtual Camera
    private PowerLauncher powerLauncher; // Reference to PowerLauncher

    // AudioController audioController;

    private void Start()
    {
        powerLauncher = FindObjectOfType<PowerLauncher>();
        // audioController = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Store the position and velocity of the currently active character
            string currentCharacterName = "Player" + selectedCharacter;
            GameObject currentCharacter = GameObject.Find(currentCharacterName);
            
            if (currentCharacter == null) return;

            float currentXPosition = currentCharacter.transform.position.x;
            float currentYPosition = currentCharacter.transform.position.y;
            Rigidbody2D currentRb = currentCharacter.GetComponent<Rigidbody2D>();
            Vector2 currentVelocity = currentRb != null ? currentRb.velocity : Vector2.zero;

            // Cycle through characters
            selectedCharacter = (selectedCharacter < totalCharacters) ? selectedCharacter + 1 : 1;

            // Get the next character
            string nextCharacterName = "Player" + selectedCharacter;
            GameObject nextCharacter = GameObject.Find(nextCharacterName);

            if (nextCharacter != null)
            {
                // Add a small upward offset to avoid ground overlap
                float upwardOffset = 0.8f; // Adjust this value as needed
                nextCharacter.transform.position = new Vector3(
                currentXPosition,
                currentYPosition + upwardOffset,
                nextCharacter.transform.position.z);
                Rigidbody2D nextRb = nextCharacter.GetComponent<Rigidbody2D>();

                if (nextRb != null)
                {
                    nextRb.velocity = currentVelocity;
                }

                // Update the camera's Follow target
                virtualCamera.Follow = nextCharacter.transform;

                // Update visibility of characters
                UpdateCharacterVisibility();

                if(powerLauncher != null)
                {
                    powerLauncher.SetActivePlayer(nextCharacter);
                }
            }
        }

        // audioController.PlaySfx(audioController.Transform);
    }

    public void UpdateCharacterVisibility()
    {
        for (int i = 1; i <= totalCharacters; i++)
        {
            string characterName = "Player" + i;
            GameObject character = GameObject.Find(characterName);
            bool isActive = (i == selectedCharacter);

            if (character != null)
            {
                // Enable the selected character and disable others
                //bool isActive = (i == selectedCharacter);
                character.GetComponent<SpriteRenderer>().enabled = isActive;
                Rigidbody2D rb = character.GetComponent<Rigidbody2D>();

                if (rb != null)
                {
                    rb.simulated = isActive; // Enable physics only for the active character
                }

                // Notify player controllers about active status
                Player2Controller playerController = character.GetComponent<Player2Controller>();
                if (playerController != null)
                {
                    playerController.IsActive = isActive;
                }

                
            }
        }
    }

    public int GetSelectedCharacter()
    {
        return selectedCharacter;
    }

}
