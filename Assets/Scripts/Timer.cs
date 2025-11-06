using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;
    public GameObject gameOverScreen; // Reference to the Game Over screen

    // Update is called once per frame
    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else if (remainingTime <= 0)
        {
            remainingTime = 0;
            // Trigger the Game Over logic
            GameOver();
            timerText.color = Color.red;
        }

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
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
}
