using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float health = 100f;
    public float damage = 10f;
    public float defense = 5f;
    public float attackRange = 5f;
    public float attackSpeed = 1f;
    private float lastAttackTime;

    // Example method to handle taking damage
    public void TakeDamage(float amount)
    {
        float damageTaken = amount - defense;
        health -= Mathf.Max(0, damageTaken);

        if (health <= 0)
        {
            Die();
        }
    }

    // Example method for attacking another unit
    public void TryAttack(Unit target)
    {
        // Check if the target has the "Enemy" tag
        if (target.CompareTag("Enemy"))
        {
            // Check if enough time has passed since the last attack based on attack speed
            if (Time.time - lastAttackTime >= 1f / attackSpeed)
            {
                // Perform attack logic here
                target.TakeDamage(damage);
                lastAttackTime = Time.time; // Update last attack time
            }
        }
    }

    // Example method for when the unit dies
    void Die()
    {
        // Handle death logic here
        Destroy(gameObject);
    }
}
