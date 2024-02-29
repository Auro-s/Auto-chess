using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float health = 100f;
    public float damage = 10f;
    public float defense = 5f;
    public float attackRange = 5f;
    public float attackSpeed = 1f;
    public float movementSpeed = 3f;

    private float lastAttackTime;
    private string targetTag; // The tag to identify the target (either "Ally" or "Enemy")

    void Start()
    {
        // Assign the target tag based on the unit's tag
        if (CompareTag("Ally"))
        {
            targetTag = "Enemy";
        }
        else if (CompareTag("Enemy"))
        {
            targetTag = "Ally";
        }
    }

    void Update()
    {
        // Check if the game is not paused
        if (!GameManager.Instance.IsPaused())
        {
            // If not in range of a target, move towards the nearest target
            if (!IsInRangeOfTarget())
            {
                MoveTowardsNearestTarget();
            }
            else
            {
                // Example: Simulate attacking when in range
                AttackNearestTarget();
            }
        }
    }

    private void MoveTowardsNearestTarget()
    {
        // Find the nearest target unit
        Unit nearestTarget = FindNearestUnitWithTag(targetTag);

        // Move towards the target if found
        if (nearestTarget != null)
        {
            Vector3 direction = (nearestTarget.transform.position - transform.position).normalized;
            transform.Translate(direction * movementSpeed * Time.deltaTime);
        }
    }

    private void AttackNearestTarget()
    {
        // Example: Simulate attacking when in range
        // Your attack logic goes here
        Unit nearestTarget = FindNearestUnitWithTag(targetTag);
        if (nearestTarget != null)
        {
            Attack(nearestTarget);
        }
    }

    private bool IsInRangeOfTarget()
    {
        // Check if any target is in attack range
        Unit nearestTarget = FindNearestUnitWithTag(targetTag);
        return (nearestTarget != null && Vector3.Distance(transform.position, nearestTarget.transform.position) <= attackRange);
    }
    private Unit FindNearestUnitWithTag(string tag)
    {
        Unit[] units = GameObject.FindGameObjectsWithTag(tag)
            .Select(go => go.GetComponent<Unit>())
            .Where(unit => unit != null)
            .ToArray();

        if (units.Length == 0)
        {
            return null;
        }

        Unit nearestUnit = units[0];
        float nearestDistance = Vector3.Distance(transform.position, nearestUnit.transform.position);

        for (int i = 1; i < units.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, units[i].transform.position);

            if (distance < nearestDistance)
            {
                nearestUnit = units[i];
                nearestDistance = distance;
            }
        }

        return nearestUnit;
    }

    // Handle taking damage
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
    public void Attack(Unit target)
    {
        // Check if the target is not null
        if (target != null)
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

    void Die()
    {
        // Handle death logic here
        Destroy(gameObject);
    }
}
