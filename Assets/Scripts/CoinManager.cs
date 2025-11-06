using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public int coinCount;
    public Text coinText;
    public GameObject door;
    private bool doorDestroyed;
    private int previousCoinCount;

    public AudioSource audioSource;
    public AudioClip Sfx_Coin;

    
    
    // Start is called before the first frame update
    void Start()
    {
    
        previousCoinCount = coinCount;
    }

    // Update is called once per frame
    void Update()  
    {
        coinText.text = coinCount.ToString();

        if (coinCount == 2 && !doorDestroyed)
        {
            doorDestroyed = true;
            Destroy(door);
        }

        if (coinCount != previousCoinCount)
        {
            audioSource.PlayOneShot(Sfx_Coin, 0.2f);
            previousCoinCount = coinCount;
        }
    }
}
