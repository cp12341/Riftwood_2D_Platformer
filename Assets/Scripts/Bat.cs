using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    public float flightSpeed = 2f;
    public float waypointReachedDistance = 0.1f;
    public DetectionZone biteDetectionZone;
    public Collider2D deathCollider;
    public List<Transform> waypoints;

    Animator animator;
    Rigidbody2D rb;
    Damageable damageable;

    Transform nextWaypoint;
    int waypointNum = 0;

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
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        damageable = GetComponent<Damageable>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        nextWaypoint = waypoints[waypointNum];
    }

    private void OnEnable()
    {
        //damageable.damageableDeath += OnDeath();
        damageable.damageableDeath.AddListener(OnDeath);
    }

    // Update is called once per frame
    void Update()
    {
        HasTarget = biteDetectionZone.detectedColliders.Count > 0;
    }

    private void FixedUpdate()
    {
        if(damageable.IsAlive)
        {
            if(CanMove)
            {
                Flight();
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        } 
    }

    private void Flight()
    {
        // Fly to next waypoint
        Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized;

        // Check if we have reached the waypoint already
        float distance = Vector2.Distance(nextWaypoint.position, transform.position);

        rb.velocity = directionToWaypoint * flightSpeed;
        UpdateDirection();

        // See if we need to switch waypoints
        if(distance <= waypointReachedDistance)
        {
            // Switch to next waypoint
            waypointNum++;

            if(waypointNum >= waypoints.Count)
            {
                // Loop back to original waypoint
                waypointNum = 0;
            }

            nextWaypoint = waypoints[waypointNum];
        }
    }

    private void UpdateDirection()
    {
        Vector3 locScale = transform.localScale;

        if(transform.localScale.x > 0)
        {
            // Facing the right
            if(rb.velocity.x < 0)
            {
                //Flip
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
        else 
        {
            // Facing the left
            if(rb.velocity.x > 0)
            {
                //Flip
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
    }

    public void OnDeath()
    {
        // Dead bat falls to the ground
        rb.gravityScale = 2f;
        deathCollider.enabled = true;
        StartCoroutine(FadeOutAndDestroy());
    }
    
    public float fadeStep = 0.05f;  // How much to fade each step
    public float waitTime = 0.1f;   // Delay between each fade step

    private IEnumerator FadeOutAndDestroy()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        for (float alpha = 1f; alpha > 0; alpha -= fadeStep)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            }
            yield return new WaitForSeconds(waitTime);
        }

        Destroy(gameObject); // Remove bat from the scene
    }
}
