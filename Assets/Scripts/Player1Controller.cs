using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class Player1Controller : MonoBehaviour
{
    public float walkSpeed=5f;
    public float runSpeed=9f;
    public float airWalkSpeed=10f;
    public float jumpImpulse=15f;
    public float gravityScale = 3f; // Base gravity multiplier
    public float lowJumpMultiplier = 2.5f; // Gravity multiplier for short jumps
    public float fallMultiplier = 3f; // Gravity multiplier for falling
    public float jumpDistanceMultiplier = 1.5f; 

    private int attackCount = 0; // Tracks the number of consecutive attacks
    [SerializeField] private int maxAttackCount = 1; // Maximum allowed attacks before cooldown
    [SerializeField] private float cooldownTime = 5f; // Cooldown duration after max attacks
    [SerializeField] private int requiredAttackItems = 1; // Number of items to unlock attack
    private int attackItemCount = 0; // Tracks collected items for attack
    private bool canAttack = false; // Track attack unlock status


    private bool isOnCooldown = false; // Tracks if the player is in cooldown
    Vector2 moveInput;
    TouchingDirections touchingDirections;
    Damageable damageable;

    public CoinManager cm;

    AudioController audioController;

        public bool IsOnCooldown  // Public property to access cooldown status
    {
        get { return isOnCooldown; }
    }

    public float CurrentMoveSpeed{get
        {
            if(CanMove)
            {
                if(IsMoving && !touchingDirections.IsOnWall)
                {
                    if (touchingDirections.IsGrounded)
                    {
                        if(IsRunning)
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
                    return 0;//Idle speed =0
                }
            }
            else
            {
                return 0;   //Movement locked
            }
            
        }
    }

    [SerializeField]
    private bool _isMoving = false;

    public bool IsMoving{get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving=value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    
    }

    [SerializeField]
    private bool _isRunning =false;

    public bool IsRunning
    {
        get
        {
            return _isRunning;
        }
        set
        {
            _isRunning=value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }

    public bool _isFacingRight=true;

    public bool IsFacingRight{get
        { return _isFacingRight; }
        private set
        {
            if(_isFacingRight!=value)
            {
                transform.localScale*=new Vector2(-1,1);
            }
            _isFacingRight=value;
        }
    }

    public bool CanMove { get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public bool IsAlive {
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }

    Rigidbody2D rb;
    Animator animator;

    private void Awake()
    {
        rb=GetComponent<Rigidbody2D>(); 
        animator = GetComponent<Animator>();
        touchingDirections= GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
    }
    // Start is called before the first frame update
    void Start()
    {
          canAttack = false;
          audioController = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
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
        moveInput=context.ReadValue<Vector2>();

        if(IsAlive)
        {
            IsMoving=moveInput!=Vector2.zero;
            SetFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if(moveInput.x>0 && !IsFacingRight)
        {
           IsFacingRight=true;//Face the right
        }
        else if(moveInput.x<0 && IsFacingRight)
        {
            IsFacingRight=false;//Face the left
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            IsRunning=true;
        }else if(context.canceled)
        {
            IsRunning=false;
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

    // public void OnAttack(InputAction.CallbackContext context)
    // {
    //     if (context.started)
    //     {
    //         animator.SetTrigger(AnimationStrings.attack);
    //     }
    // }

     public void TriggerCooldown()
    {
        if (!isOnCooldown)
        {
            StartCoroutine(StartCooldown());
        }
    }

    private IEnumerator StartCooldown()
    {
        Debug.Log("Cooldown started.");
        isOnCooldown = true;
        animator.ResetTrigger(AnimationStrings.attack);

        yield return new WaitForSeconds(cooldownTime);

        attackCount = 0;
        isOnCooldown = false;
        Debug.Log("Cooldown ended. Attack is available again.");
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!GameManager.Instance.isAttack1AbilityUnlocked)
        {
            Debug.Log("Attack ability is locked!");
            return; // Exit if the ability is not unlocked
        }
        if (context.started && !isOnCooldown)
        {
            attackCount++;
            animator.SetTrigger(AnimationStrings.attack);
            Debug.Log($"Attack {attackCount}/{maxAttackCount}");

            if (attackCount > maxAttackCount)
            {
                TriggerCooldown();
            }

            audioController.PlaySfx(audioController.MaleAttack, 1.0f);
        }
        else if (isOnCooldown)
        {
            Debug.Log("Cannot attack: on cooldown.");
        }
    }


    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
        audioController.PlaySfx(audioController.Malehurt,0.8f);
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            cm.coinCount++;
        }

                       // Collect AttackItem
        if (other.gameObject.CompareTag("AttackItem1"))
        {
            Destroy(other.gameObject);
            attackItemCount++;
            Debug.Log($"Collected Attack Item {attackItemCount}/{requiredAttackItems}");

            // Check if attack ability should be unlocked
            if (attackItemCount >= requiredAttackItems && !canAttack)
            {
                canAttack = true;
                GameManager.Instance.isAttack1AbilityUnlocked = true;
                Debug.Log("Attack ability unlocked!");
            }
        }

        // Collect PlatformItem
        if (other.gameObject.CompareTag("PlatformItem"))
        {
            Destroy(other.gameObject);
            Debug.Log("Collected Platform Item!");

            // Unlock platform ability
            GameManager.Instance.isPlatformAbilityUnlocked = true;
            Debug.Log("Platform ability unlocked!");
        }
    }
}
