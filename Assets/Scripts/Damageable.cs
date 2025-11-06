using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{

    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent damageableDeath;

    Animator animator;

    [SerializeField]
    private int _health = 100;

    [SerializeField]
    private int _maxHealth = 100;

    [SerializeField]
    private float healthMultiplier = 1.0f; 

    public int Health
    {
        get { return _health; }
        set
        {
            _health = value;
            if (_health <= 0)
            {
                IsAlive = false;
            }
        }
    }

        public int MaxHealth
    {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }


    [SerializeField]
    private bool _isAlive = true;
    [SerializeField]
    private bool isInvincible = false;

    private float timeSinceHit;
    public float invincibilityTime = 0.25f;

    public bool IsAlive
    {
        get 
        {
            return _isAlive; 
        }
        set
        {
            _isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
            Debug.Log($"IsAlive set to {value}");

            if(value == false)
            {
                damageableDeath.Invoke();
            }
        }
    }

    public bool LockVelocity
    {
        get { return animator.GetBool(AnimationStrings.lockVelocity); }
        set { animator.SetBool(AnimationStrings.lockVelocity, value); }
    }

    // Reference to SharedHealth
    [SerializeField]
    private SharedHealth sharedHealth;

    [SerializeField]
    private bool isPlayer; // Set this in the Inspector
    public bool IsPlayer => isPlayer;

    private void Awake()
    {
        // animator = GetComponent<Animator>();

        // if (isPlayer && sharedHealth == null)
        // {
        //     Debug.LogError($"Player {gameObject.name} needs a reference to SharedHealth!");
        // }

        //         // Adjust health based on whether the object is an enemy
        // if (!isPlayer)
        // {
        //     AdjustEnemyHealth();
        // }
        animator = GetComponent<Animator>();

        if (isPlayer && sharedHealth == null)
        {
            Debug.LogError($"Player {gameObject.name} needs a reference to SharedHealth!");
        }
        else if (isPlayer && sharedHealth != null)
        {
            SyncHealth();
        }
        else
        {
            AdjustEnemyHealth();
        }
    }

    private void Start()
    {
        if (isPlayer && sharedHealth != null)
        {
            SyncHealth(); // Synchronize health after all objects are initialized
        }
    }
    
    private void AdjustEnemyHealth()
    {
        MaxHealth = Mathf.RoundToInt(MaxHealth * healthMultiplier);
        Health = MaxHealth;
        Debug.Log($"{gameObject.name} health adjusted to {Health}/{MaxHealth}");
    }

    private void OnEnable()
    {
        if (isPlayer && sharedHealth != null)
        {
            sharedHealth.OnHealthChanged += UpdateHealthFromShared;
            SyncHealth(); // Ensure health is synchronized when switching players
        }
    }

    private void OnDisable()
    {
        if (isPlayer && sharedHealth != null)
        {
            sharedHealth.OnHealthChanged -= UpdateHealthFromShared;
        }
    }

    private void SyncHealth()
    {
        if (sharedHealth != null)
        {
            UpdateHealthFromShared(sharedHealth.CurrentHealth, sharedHealth.maxHealth);
        }
    }

    private void UpdateHealthFromShared(int currentHealth, int maxHealth)
    {
        Health = currentHealth; // Sync health with shared health
        Debug.Log($"{gameObject.name} health updated to {currentHealth}/{maxHealth}");
    }

    private void Update()
    {
        if (isInvincible)
        {
            timeSinceHit += Time.deltaTime;
            if (timeSinceHit > invincibilityTime)
            {
                isInvincible = false;
                timeSinceHit = 0;
            }
           
        }
    }

    public bool Hit(int damage, Vector2 knockback)
    {
        if (IsAlive && !isInvincible)
        {
            if (isPlayer && sharedHealth != null) // 处理玩家
            {
                sharedHealth.TakeDamage(damage);
            }
            else
            {
                Health -= damage; // For enemies
            }

            // Trigger animations and invincibility logic
            isInvincible = true;
            animator.SetTrigger(AnimationStrings.hitTrigger);
            LockVelocity = true;
            damageableHit?.Invoke(damage, knockback);

            return true;
        }

        return false;
    }

}

