using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Rigidbody2D),typeof(TouchingDirections), typeof(Damageable))]
public class Demon : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float walkStopRate = 0.00f;
    public DetectionZone attackZone;
    //public LayerMask groundLayer; 
    public Transform edgeCheckPoint; // Assign a position near the feet of the enemy
    public float edgeCheckDistance = 0.1f;

    public GameObject keyPrefab; // Key prefab to spawn
    public Transform keySpawnPoint; // Spawn point for the key

    public GameObject healthBarSliderPrefab; // Assign the HealthBarSlider prefab in the Inspector
    private Slider healthBarSlider; // Instance of the slider

    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    Animator animator;

    Damageable damageable;

    public enum WalkableDirection {Right,Left}

    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector=Vector2.left;

    public WalkableDirection WalkDirection
    {
        get{return _walkDirection;}
        set {
            if(_walkDirection !=value)
            {
                    gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);

                    if(value==WalkableDirection.Left)
                    {
                        walkDirectionVector=Vector2.left;
                    }else if(value==WalkableDirection.Right)
                    {
                        walkDirectionVector = Vector2.right;
                    }
            }
            _walkDirection = value;}
    }

    public bool _hasTarget=false;

    public bool HasTarget{
        get
        {
            return _hasTarget;
            }
        private set
        {
            _hasTarget=value;
            animator.SetBool(AnimationStrings.hasTarget,value);
           }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator=GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
          // Subscribe to the death event to spawn the key
        damageable.damageableDeath.AddListener(OnBossDeath);
    }

        // Update is called once per frame
    void Update()
    {
        HasTarget=attackZone.detectedColliders.Count>0;
             // Update the health bar's position and value
        if (healthBarSlider != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 1.5f);
            healthBarSlider.transform.position = screenPos;

            healthBarSlider.value = damageable.Health;
        }
    }

    private void FixedUpdate()
    {
        // Check if the enemy is on the ground and facing a wall or near the edge
        if ((touchingDirections.IsGrounded && touchingDirections.IsOnWall) || !IsEdgeAhead())
        {
            FlipDirection();
        }

        if(!damageable.LockVelocity)
        {
            if(CanMove && touchingDirections.IsGrounded)
           { rb.velocity=new Vector2(walkSpeed*walkDirectionVector.x,rb.velocity.y);}
            else
           { rb.velocity=new Vector2(Mathf.Lerp(rb.velocity.x,0,walkStopRate),rb.velocity.y);}

        }
        
    }

      private void OnDestroy()
    {
        // Unsubscribe from the event to prevent memory leaks
        if (damageable != null)
        {
            damageable.damageableDeath.RemoveListener(OnBossDeath);
        }

            // Destroy the health bar
        if (healthBarSlider != null)
        {
            Destroy(healthBarSlider.gameObject);
        }
    }


        private bool IsEdgeAhead()
    {
        // Cast a ray downward from the edge check point to see if ground exists
        RaycastHit2D hit = Physics2D.Raycast(edgeCheckPoint.position, Vector2.down, edgeCheckDistance);
        return hit.collider != null; // Returns true if ground is detected
    }

    private void OnBossDeath()
    {
        Debug.Log("Boss defeated! Spawning key...");

        // Spawn the key at the specified location
        if (keyPrefab != null && keySpawnPoint != null)
        {
            Instantiate(keyPrefab, keySpawnPoint.position, Quaternion.identity);
            Debug.Log("Key spawned!");
        }

        // Destroy the boss object
        Destroy(gameObject);
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

      public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }


    // Start is called before the first frame update
    void Start()
    {
         WalkDirection = WalkableDirection.Left; 

        // Instantiate and position the health bar
        if (healthBarSliderPrefab != null)
        {
            GameObject sliderInstance = Instantiate(healthBarSliderPrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity);
            sliderInstance.transform.SetParent(GameObject.Find("Canvas").transform, false);

            healthBarSlider = sliderInstance.GetComponent<Slider>();
            if (healthBarSlider != null)
            {
                healthBarSlider.maxValue = damageable.MaxHealth;
                healthBarSlider.value = damageable.Health;
            }
        }
    }


}
