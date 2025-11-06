using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public static Game Instance; // Singleton instance

    [SerializeField] private GameObject gameOverScreen; // Reference to the Game Over UI

    private void Awake()
    {
        // Ensure a single instance of the GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowGameOverScreen()
    {
        gameOverScreen.SetActive(true); // Display Game Over UI
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload current level
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit the game
        Debug.Log("Game Quit!");
    }
}
