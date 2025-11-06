using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public Transform destination;
    GameObject player1;
    GameObject player2;

    private void Awake()
    {
        player1 = GameObject.FindGameObjectWithTag("Player1");
        player2 = GameObject.FindGameObjectWithTag("Player2");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1")||collision.CompareTag("Player2"))
        {
            if (Vector2.Distance(player1.transform.position, transform.position) > 0.8f||Vector2.Distance(player2.transform.position, transform.position) > 0.8f)
            {
                player1.transform.position = destination.transform.position;
                player2.transform.position = destination.transform.position;
            }
        }
    }
}
