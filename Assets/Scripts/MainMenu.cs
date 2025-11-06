using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Awake()
    {
        if (!PlayerPrefs.HasKey("FirstLaunch"))
        {
            PlayerPrefs.DeleteAll(); // Reset all data
            PlayerPrefs.SetInt("FirstLaunch", 1); // Mark that the reset is done
            PlayerPrefs.Save();
            Debug.Log("PlayerPrefs reset on first launch for testing.");
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        Application.Quit();
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
