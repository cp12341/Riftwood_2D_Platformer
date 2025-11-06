using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapOnCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1")
        {
            Debug.Log("Trap hit Player");
        }
    }
}
