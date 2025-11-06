using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelButton : MonoBehaviour
{
    public string nextSceneName; // Name of the next scene

    public void LoadNextLevel()
    {
        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene(nextSceneName); // Load next scene
    }

    public void Home()
    {
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1;
    }

    public void Restart()
    {
        Checkpoint.ResetCheckpoint();
        // Reset the platform ability to locked

        // if (SceneManager.GetActiveScene().buildIndex == 2)  // Lock for level 2
        // {
        //     GameManager.Instance.isPlatformAbilityUnlocked = false;
        //     GameManager.Instance.isShrinkAbilityUnlocked = false;
        // }


        // if (SceneManager.GetActiveScene().buildIndex == 4)  // Lock for level 4
        // {
        //     GameManager.Instance.isAttack1AbilityUnlocked = false;
        //     GameManager.Instance.isAttack2AbilityUnlocked = false;
        // }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
}
