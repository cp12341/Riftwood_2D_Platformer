using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyCollector : MonoBehaviour
{
    public GameObject levelCompleteUI; // Reference to the Level Completed UI

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("key"))
        {
            Destroy(collision.gameObject); // Remove the key from the scene
            Debug.Log("Key Collected!");
            UnlockNewLevel();
            ShowLevelCompleteUI();
        }
    }

    private void ShowLevelCompleteUI()
    {
        levelCompleteUI.SetActive(true); // Show the Level Completed UI
        Time.timeScale = 0f; // Pause the game
    }

    void UnlockNewLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex>=PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();
        }
    }
}
