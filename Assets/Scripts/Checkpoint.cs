using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private static Vector2 lastCheckpointPosition = Vector2.zero;  // Store the last checkpoint position
    private Animator animator;
    private bool isActivated = false;  // Track checkpoint state
    public AudioSource audioSource;  // AudioSource component to play sound effects

    public AudioClip checkpointSfx;  // AudioClip for checkpoint sound effect

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();  // Get the AudioSource component
    }

    // This function is called when the player reaches a checkpoint
    public static void SetLastCheckpoint(Vector2 checkpointPosition)
    {
        lastCheckpointPosition = checkpointPosition;
        Debug.Log("Checkpoint set at position: " + checkpointPosition);
    }

    // This function gets the last checkpoint position
    public static Vector2 GetLastCheckpoint()
    {
        return lastCheckpointPosition;
    }

    // Reset checkpoint position
    public static void ResetCheckpoint()
    {
        lastCheckpointPosition = Vector2.zero;
        Debug.Log("Checkpoint has been reset.");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActivated && (collision.CompareTag("Player1") || collision.CompareTag("Player2")))
        {
            isActivated = true;
            SetLastCheckpoint(transform.position);
            animator.SetTrigger("activateCheckpoint");
            Debug.Log($"{collision.tag} has activated a checkpoint.");
            audioSource.PlayOneShot(checkpointSfx, 1.0f);  // Play the checkpoint sound effect
           
        }


    }

    // Reset state when game restarts
    public void ResetToInitialState()
    {
        isActivated = false;
        animator.Play("Idle", 0, 0f);  // Reset to Idle animation state
    }

    // Start is called before the first frame update
    void Start()
    {
        //Checkpoint.ResetCheckpoint();  // Reset at the start of the game
        ResetToInitialState();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
