using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameObject player1;
    private GameObject player2;
    private Player1Controller player1Controller;
    private Player2Controller player2Controller;
    private PlayerInput player1Input;
    private PlayerInput player2Input;
    private Collider2D player1Collider;
    private Collider2D player2Collider;
    private Rigidbody2D player1Rb;
    private Rigidbody2D player2Rb;

    private Checkpoint[] checkpoints; // Array to store all checkpoints

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Register callback for scene loading
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unregister callback
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializePlayers(); // Reinitialize players when a new scene loads
        InitializeCheckpoints(); // Reset checkpoints
    }

    private void Start()
    {
        InitializePlayers();
        InitializeCheckpoints();
    }

    private void InitializePlayers()
    {
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");

        if (player1 != null && player2 != null)
        {
            player1Controller = player1.GetComponent<Player1Controller>();
            player2Controller = player2.GetComponent<Player2Controller>();
            player1Input = player1.GetComponent<PlayerInput>();
            player2Input = player2.GetComponent<PlayerInput>();
            player1Collider = player1.GetComponent<Collider2D>();
            player2Collider = player2.GetComponent<Collider2D>();
            player1Rb = player1.GetComponent<Rigidbody2D>();
            player2Rb = player2.GetComponent<Rigidbody2D>();

            // Enable Player1 and disable Player2 at start
            SetPlayerActive(player1, true);
            SetPlayerActive(player2, false);
        }
    }

    private void InitializeCheckpoints()
    {
        checkpoints = FindObjectsOfType<Checkpoint>();
        foreach (Checkpoint checkpoint in checkpoints)
        {
            checkpoint.ResetToInitialState();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchCharacter();
        }
    }

    private void SwitchCharacter()
    {
        if (player1 == null || player2 == null)
        {
            Debug.LogWarning("Players not initialized. Cannot switch characters.");
            return;
        }

        if (player1Controller.enabled)
        {
            SetPlayerActive(player1, false);
            SetPlayerActive(player2, true);
        }
        else
        {
            SetPlayerActive(player2, false);
            SetPlayerActive(player1, true);
        }
    }

    private void SetPlayerActive(GameObject player, bool isActive)
    {
        if (player != null)
        {
            // Handle either Player1Controller or Player2Controller
            var player1Controller = player.GetComponent<Player1Controller>();
            var player2Controller = player.GetComponent<Player2Controller>();
            var input = player.GetComponent<PlayerInput>();
            var collider = player.GetComponent<Collider2D>();
            var rb = player.GetComponent<Rigidbody2D>();

            // Enable or disable the appropriate controller
            if (player1Controller != null)
                player1Controller.enabled = isActive;

            if (player2Controller != null)
                player2Controller.enabled = isActive;

            // Enable or disable input, collider, and physics simulation
            if (input != null) input.enabled = isActive;
            if (collider != null) collider.enabled = isActive;
            if (rb != null) rb.simulated = isActive;
        }
    }

    // Tracks whether the attack ability is unlocked
    public bool isAttack1AbilityUnlocked = false;
    public bool isAttack2AbilityUnlocked = false;
    public bool isShrinkAbilityUnlocked = false;
    public bool isPlatformAbilityUnlocked = false;  // Track if platform ability is unlocked
}
