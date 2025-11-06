using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform_02 : MonoBehaviour
{
    public Transform posA, posB;
    public float speed;
    Vector3 targetPos;

    private void Start()
    {
        targetPos = posB.position;
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, posA.position) < 0.05f)
        {
            targetPos = posB.position;
        }

        if (Vector2.Distance(transform.position, posB.position) < 0.05f)
        {
            targetPos = posA.position;
        }
        
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1"))
        {
            collision.transform.parent = this.transform;
        }
        else if (collision.CompareTag("Player2"))
        {
            collision.transform.parent = this.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1"))
        {
            collision.transform.parent = null;
        }
        else if (collision.CompareTag("Player2"))
        {
            collision.transform.parent = null;
        }
    }

}
