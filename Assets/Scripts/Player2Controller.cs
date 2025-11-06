using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class Player2Controller : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public float airWalkSpeed = 10f;
    public float jumpImpulse = 15f;
    public float gravityScale = 3f; // Base gravity multiplier
    public float lowJumpMultiplier = 2.5f; // Gravity multiplier for short jumps
    public float fallMultiplier = 3f; // Gravity multiplier for falling
    public float jumpDistanceMultiplier = 1.5f; 
    
    private int attackCount = 0; // Tracks the number of consecutive attacks
    [SerializeField] private int maxAttackCount = 5; // Maximum allowed attacks before cooldown
    [SerializeField] private float cooldownTime = 5f; // Cooldown duration after max attacks

    // Item collection for unlocking actions
    [SerializeField] private int requiredAttackItems = 1; // Number of items to unlock attack
    [SerializeField] private int requiredShrinkItems = 1; // Number of items to unlock shrinking

    private int attackItemCount = 0; // Tracks collected items for attack
    private int shrinkItemCount = 0; // Tracks collected items for shrinking

    private bool canAttack = false; // Track attack unlock status
    private bool canShrink = false; // Track shrink unlock status

    private bool isOnCooldown = false; // Tracks if the player is in cooldown
    Vector2 moveInput;
    TouchingDirections touchingDirections;

    Damageable damageable;

    public CoinManager cm;

    AudioController audioController;


    public float CurrentMoveSpeed
    {
        get
        {
            if(CanMove)
            {
                if (IsMoving && !touchingDirections.IsOnWall)
                {
                    if (touchingDirections.IsGrounded)
                    {
                        if (IsRunning)
                        {
                            return runSpeed;
                        }
                        else
                        {
                            return walkSpeed;
                        }

                    }
                    else
                    {
                        return airWalkSpeed;
                    }

                }
                else
                {
                    return 0; // Idle speed = 0
                }

            } else
            {
                return 0;
            }

            
        }
    }

   private void Start()
    {
        // Ensure abilities start locked
        canAttack = false;
        canShrink = false;
        audioController = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioController>();

    }


    [SerializeField]
    private bool _isMoving = false;

    public bool IsMoving
    {
        get { return _isMoving; }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }

    [SerializeField]
    private bool _isRunning = false;

    public bool IsRunning
    {
        get { return _isRunning; }
        set
        {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }

    public bool _isFacingRight = true;

    public bool IsFacingRight
    {
        get { return _isFacingRight; }
        private set
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }
    }

    public bool CanMove {
        get{
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public bool IsAlive{
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }

    Rigidbody2D rb;
    Animator animator;

    // Added for shrinking
    private Vector3 originalScale;
    private Vector2 originalColliderSize;
    private Vector2 originalColliderOffset;
    private BoxCollider2D playerCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();

        // Save original scale and collider dimensions
        originalScale = transform.localScale;
        playerCollider = GetComponent<BoxCollider2D>();
        if (playerCollider != null)
        {
            originalColliderSize = playerCollider.size;
            originalColliderOffset = playerCollider.offset;
        }
    }

    void FixedUpdate()
    {
            // Maintain horizontal speed during jump
        if (!touchingDirections.IsGrounded)
        {
            float horizontalSpeed = IsRunning ? runSpeed : walkSpeed;
            horizontalSpeed *= jumpDistanceMultiplier;
            rb.velocity = new Vector2(moveInput.x * horizontalSpeed, rb.velocity.y);
        }
        else
        {
           if(!damageable.LockVelocity)
                // Standard movement on the ground
                rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        }

        // Smooth gravity for the jump
        if (rb.velocity.y > 0 && !Input.GetButton("Jump")) // Rising and button released
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y < 0) // Falling
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if(IsAlive)
        {
        IsMoving = moveInput != Vector2.zero;
        SetFacingDirection(moveInput);
        }
        else
        {
            IsMoving=false;
        }

    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true; // Face the right
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false; // Face the left
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
            if (context.started && touchingDirections.IsGrounded && CanMove) // Ensure the player is grounded
        {
            animator.SetTrigger(AnimationStrings.jump);

            // Determine horizontal speed and apply multiplier for further jumps
            float horizontalSpeed = IsRunning ? runSpeed : walkSpeed;
            horizontalSpeed *= jumpDistanceMultiplier;

            // Set the velocity for jump with horizontal distance
            rb.velocity = new Vector2(moveInput.x * horizontalSpeed, jumpImpulse);
        }

    }

     public bool IsActive { get; set; } = false;
    // Shrinking functionality
    public void OnShrink(InputAction.CallbackContext context)
    {
        if (!GameManager.Instance.isShrinkAbilityUnlocked)
        {
            Debug.Log("Shrink ability is locked!");
            return; // Exit if the ability is not unlocked
        }


        if (context.started&& IsActive)
        {
            ToggleShrink();
            audioController.PlaySfx(audioController.Magic, 1.0f);
        }
    }

    public bool isShrunken = false; // Track the current state


    // public void ShrinkCharacter()
    // {
    //     float xScale = Mathf.Abs(originalScale.x) * (IsFacingRight ? 1 : -1); // Preserve facing direction
    //     transform.localScale = new Vector3(xScale * 0.5f, originalScale.y * 0.5f, originalScale.z); // Shrinks to half size

    //     if (playerCollider != null)
    //     {
    //         playerCollider.size = originalColliderSize * 0.9f;
    //         playerCollider.offset = originalColliderOffset * 0.9f;
    //     }
    // }

    public void ShrinkCharacter()
    {
        // Temporarily detach from parent
        Transform originalParent = transform.parent;
        transform.parent = null; // Detach from any parent

        float xScale = Mathf.Abs(originalScale.x) * (IsFacingRight ? 1 : -1); // Preserve facing direction
        transform.localScale = new Vector3(xScale * 0.5f, originalScale.y * 0.5f, originalScale.z); // Shrinks to half size

        if (playerCollider != null)
        {
            playerCollider.size = originalColliderSize * 0.9f;
            playerCollider.offset = originalColliderOffset * 0.9f;
        }

        // Reattach to original parent
        transform.parent = originalParent;
    }

    public void ResetCharacterSize()
    {
        // Temporarily detach from parent
        Transform originalParent = transform.parent;
        transform.parent = null; // Detach from any parent

        float xScale = Mathf.Abs(originalScale.x) * (IsFacingRight ? 1 : -1); // Preserve facing direction
        transform.localScale = new Vector3(xScale, originalScale.y, originalScale.z); // Resets to original size

        if (playerCollider != null)
        {
            playerCollider.size = originalColliderSize;
            playerCollider.offset = originalColliderOffset;

            // Perform a small upward shift to avoid overlap
            transform.position += Vector3.up * 3f; // Adjust the value if needed
        }

        // Reattach to original parent
        transform.parent = originalParent;
    }

    // public void ResetCharacterSize()
    // {
    //     // Temporarily disable the collider
    //     if (playerCollider != null)
    //     {
    //         playerCollider.enabled = false;
    //     }

    //     float xScale = Mathf.Abs(originalScale.x) * (IsFacingRight ? 1 : -1); // Preserve facing direction
    //     transform.localScale = new Vector3(xScale, originalScale.y, originalScale.z); // Resets to original size

    //     if (playerCollider != null)
    //     {
    //         playerCollider.size = originalColliderSize;
    //         playerCollider.offset = originalColliderOffset;

    //         // Perform a small upward shift to avoid overlap
    //         transform.position += Vector3.up * 3f;

    //         // Re-enable the collider after resizing
    //         playerCollider.enabled = true;

    //         // Check for immediate collision
    //         Collider2D overlap = Physics2D.OverlapBox(transform.position, playerCollider.size, 0);
    //         if (overlap != null && overlap.gameObject != this.gameObject)
    //         {
    //             // Push the character slightly away from the collision
    //             transform.position += Vector3.up * 0.2f; // Adjust upward displacement as needed
    //         }
    //     }

    // }

    // public void ToggleShrink()
    // {
    //     if (isShrunken)
    //     {
    //         ResetCharacterSize(); // Revert to original size
    //         isShrunken = false;
    //     }
    //     else
    //     {
    //         ShrinkCharacter(); // Shrink the character
    //         isShrunken = true;
    //     }
    // }

    public void ToggleShrink()
    {
        Transform originalParent = transform.parent; // Save the current parent
        if (originalParent != null && originalParent.CompareTag("MovingPlatform"))
        {
            transform.SetParent(null); // Temporarily detach from the platform
        }

        if (isShrunken)
        {
            ResetCharacterSize(); // Revert to original size
            isShrunken = false;
        }
        else
        {
            ShrinkCharacter(); // Shrink the character
            isShrunken = true;
        }

        if (originalParent != null && originalParent.CompareTag("MovingPlatform"))
        {
            transform.SetParent(originalParent); // Reattach to the platform
        }
    }

    // public void OnAttack(InputAction.CallbackContext context)
    // {
    //     if(context.started)
    //     {
    //         animator.SetTrigger(AnimationStrings.attack);
    //     }
    // }

    public void OnAttack(InputAction.CallbackContext context)
    {
       if (!GameManager.Instance.isAttack2AbilityUnlocked)
        {
            Debug.Log("Attack ability is locked!");
            return; // Exit if the ability is not unlocked
        }

        if (context.started && !isOnCooldown) // Allow attack only if not in cooldown
        {
            attackCount++;

            // Trigger attack animation
            animator.SetTrigger(AnimationStrings.attack);
            Debug.Log($"Attack {attackCount}/{maxAttackCount}");

            if (attackCount > maxAttackCount)
            {
                StartCoroutine(StartCooldown());
            }

            audioController.PlaySfx(audioController.FemaleAttack, 1.0f);
        }
        else if (isOnCooldown)
        {
            Debug.Log("Cannot attack: on cooldown.");
        }
    }

    private IEnumerator StartCooldown()
    {
        Debug.Log("Cooldown started.");
        isOnCooldown = true;

        // Stop the attack animation
        animator.ResetTrigger(AnimationStrings.attack);

       
        // Wait for the cooldown to finish
        yield return new WaitForSeconds(cooldownTime);

        // Reset attack count and cooldown status
        attackCount = 0;
        isOnCooldown = false;


        Debug.Log("Cooldown ended. Attack is available again.");
    }


    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
        audioController.PlaySfx(audioController.Femalehurt, 1.0f);

    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            cm.coinCount++;
        }

                // Collect AttackItem
        if (other.gameObject.CompareTag("AttackItem2"))
        {
            Destroy(other.gameObject);
            attackItemCount++;
            Debug.Log($"Collected Attack Item {attackItemCount}/{requiredAttackItems}");

            // Check if attack ability should be unlocked
            if (attackItemCount >= requiredAttackItems && !canAttack)
            {
                canAttack = true;
                GameManager.Instance.isAttack2AbilityUnlocked = true;
                Debug.Log("Attack ability unlocked!");
            }
        }

        // Collect ShrinkItem
        if (other.gameObject.CompareTag("ShrinkItem"))
        {
            Destroy(other.gameObject);
            shrinkItemCount++;
            Debug.Log($"Collected Shrink Item {shrinkItemCount}/{requiredShrinkItems}");

            // Check if shrink ability should be unlocked
            if (shrinkItemCount >= requiredShrinkItems && !canShrink)
            {
                canShrink = true;
                GameManager.Instance.isShrinkAbilityUnlocked = true;
                Debug.Log("Shrink ability unlocked!");
            }
        }
    }


}
