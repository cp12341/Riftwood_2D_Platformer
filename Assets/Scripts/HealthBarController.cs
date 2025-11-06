using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
   public Slider healthBarSlider; // Reference to the slider
    private Damageable damageable; // Reference to the Damageable component of the demon

    void Start()
    {
        // Find the Damageable component on the parent object
        damageable = GetComponentInParent<Damageable>();

        if (damageable != null)
        {
            // Initialize the health bar
            healthBarSlider.maxValue = damageable.MaxHealth;
            healthBarSlider.value = damageable.Health;
        }
    }

    void Update()
    {
        // Update the health bar based on the demon's current health
        if (damageable != null)
        {
            healthBarSlider.value = damageable.Health;
        }
    }
}
