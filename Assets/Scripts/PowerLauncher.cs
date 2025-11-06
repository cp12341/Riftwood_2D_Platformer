using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerLauncher : MonoBehaviour
{
    public Transform launchPoint;
    public GameObject powerPrefab;
    public string player1Tag = "Player1"; // Set the correct tag for Player 1 in the Inspector

    private GameObject activePlayer;

    // Start is called before the first frame update
    void Start()
    {
        // Assuming Player1 is the initial active player
        activePlayer = GameObject.FindWithTag(player1Tag);
    }

    public void SetActivePlayer(GameObject player)
    {
        activePlayer = player;
    }

    public void FireProjectile()
    {
        if (activePlayer.CompareTag(player1Tag)) // Only launch if it's Player1
        {
            // Access the Player1Controller script
            Player1Controller player1Controller = activePlayer.GetComponent<Player1Controller>();

            if (player1Controller != null)
            {
                if (!player1Controller.IsOnCooldown) // Check cooldown from Player1Controller
                {
                    LaunchPower();
                    player1Controller.TriggerCooldown(); // Trigger cooldown in Player1Controller
                }
                else
                {
                    Debug.Log("Player 1's power is on cooldown!");
                }
            }
            else
            {
                Debug.LogError("Player1Controller script not found on active player!");
            }
        }
    }

    private void LaunchPower()
    {
        GameObject power = Instantiate(powerPrefab, launchPoint.position, powerPrefab.transform.rotation);
        Vector3 origScale = power.transform.localScale;

        // Adjust the direction of the power based on player direction
        power.transform.localScale = new Vector3(
            origScale.x * (transform.localScale.x > 0 ? 1 : -1),
            origScale.y,
            origScale.z
        );
    }
}
