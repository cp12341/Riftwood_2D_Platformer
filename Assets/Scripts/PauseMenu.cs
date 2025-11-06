using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;    // Reference to the Pause Menu
    // [SerializeField] GameObject heartCanvas; // Reference to the Heart Canvas
    // [SerializeField] GameObject coinCanvas;  // Reference to the Coin Canvas
    // [SerializeField] GameObject timeCanvas;  // Reference to the Coin Canvas

    public void Pause()
    {
        pauseMenu.SetActive(true);
        // heartCanvas.SetActive(false); // Disable Heart Canvas
        // coinCanvas.SetActive(false);  // Disable Coin Canvas
        // timeCanvas.SetActive(false);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        // heartCanvas.SetActive(true);  // Re-enable Heart Canvas
        // coinCanvas.SetActive(true);   // Re-enable Coin Canvas
        // timeCanvas.SetActive(true);
        Time.timeScale = 1;
    }

    public void Home()
    {
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1;
    }

    // public void Restart()
    // {
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //     Time.timeScale = 1;
    // }

    public void Restart()
    {
        // Reset the checkpoint when the level restarts
        Checkpoint.ResetCheckpoint();

        // if (SceneManager.GetActiveScene().buildIndex == 2)  // Lock for level 2
        // {
        //     GameManager.Instance.isPlatformAbilityUnlocked = false;
        //     GameManager.Instance.isShrinkAbilityUnlocked = false;
        // }

        // // Reset the platform ability to locked
        // GameManager.Instance.isPlatformAbilityUnlocked = false;

        // GameManager.Instance.isShrinkAbilityUnlocked = false;

        // if (SceneManager.GetActiveScene().buildIndex == 4)  // Lock for level 4
        // {
        //     GameManager.Instance.isAttack1AbilityUnlocked = false;
        //     GameManager.Instance.isAttack2AbilityUnlocked = false;
        // }
        // GameManager.Instance.isAttack1AbilityUnlocked = false;
        // GameManager.Instance.isAttack2AbilityUnlocked = false;

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Ensure time is running normally after restart
        Time.timeScale = 1;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
