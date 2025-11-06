using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),typeof(TouchingDirections))]
public class Mushroom : MonoBehaviour
{
    public float walkSpeed = 3f;
    //public LayerMask groundLayer; 
    public Transform edgeCheckPoint; // Assign a position near the feet of the enemy
    public float edgeCheckDistance = 0.1f;

    Rigidbody2D rb;
    TouchingDirections touchingDirections;

    public enum WalkableDirection {Right,Left}

    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector=Vector2.right;

    public WalkableDirection WalkDirection
    {
        get{return _walkDirection;}
        set {
            if(_walkDirection !=value)
            {
                    gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);

                    if(value==WalkableDirection.Right)
                    {
                        walkDirectionVector=Vector2.right;
                    }else if(value==WalkableDirection.Left)
                    {
                        walkDirectionVector = Vector2.left;
                    }
            }
            _walkDirection = value;}
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
    }

    private void FixedUpdate()
    {
        // Check if the enemy is on the ground and facing a wall or near the edge
        if ((touchingDirections.IsGrounded && touchingDirections.IsOnWall) || !IsEdgeAhead())
        {
            FlipDirection();
        }

        // Apply movement
        rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);
    }

        private bool IsEdgeAhead()
    {
        // Cast a ray downward from the edge check point to see if ground exists
        RaycastHit2D hit = Physics2D.Raycast(edgeCheckPoint.position, Vector2.down, edgeCheckDistance);
        return hit.collider != null; // Returns true if ground is detected
    }


    private void FlipDirection()
    {
        if(WalkDirection==WalkableDirection.Right)
        {
            WalkDirection=WalkableDirection.Left;
        } else if (WalkDirection==WalkableDirection.Left)
        {
            WalkDirection=WalkableDirection.Right;
        }else
        {
            Debug.LogError("Current walkable direction is not set to legal values of right or left");
        }
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
