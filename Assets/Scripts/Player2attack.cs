using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2attack : MonoBehaviour
{
        public int attackDamage = 10;
    public Vector2 knockback = Vector2.zero;
    [SerializeField] private bool targetsPlayers; // Specify if this attack targets players
    [SerializeField] private bool targetsEnemies; // Specify if this attack targets enemies

    public int maxAttacks = 3; // Maximum number of attacks before cooldown
    public float cooldownTime = 5f; // Cooldown duration in seconds

    private int attackCount = 0; // Tracks the number of attacks
    private bool isOnCooldown = false; // Indicates if cooldown is active
    private Animator animator; // Reference to the Animator

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Animator not found on Player 2. Attack animation won't work.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOnCooldown || animator == null) return; // Do nothing if on cooldown or animator is missing

        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null)
        {
            Vector2 deliveredKnockback = transform.parent.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);

            bool gotHit = damageable.Hit(attackDamage, deliveredKnockback);

            if (gotHit)
            {
                attackCount++;
                Debug.Log($"Attack {attackCount}/{maxAttacks}: {collision.name} hit for {attackDamage} damage.");
                PlayAttackAnimation();

                if (attackCount >= maxAttacks)
                {
                    Debug.Log("Attack limit reached. Starting cooldown...");
                    StartCoroutine(Cooldown());
                }
            }
        }
    }

    private void PlayAttackAnimation()
    {
        // Trigger the attack animation
        animator.SetTrigger(AnimationStrings.attack);
    }

    private IEnumerator Cooldown()
    {
        isOnCooldown = true;

        // Disable attack animation during cooldown
        animator.SetBool("IsOnCooldown", true);

        yield return new WaitForSeconds(cooldownTime); // Wait for the cooldown duration

        attackCount = 0; // Reset attack count
        isOnCooldown = false;

        // Re-enable attack animation after cooldown
        animator.SetBool("IsOnCooldown", false);

        Debug.Log("Cooldown finished. Attacks are ready.");
    }
}
