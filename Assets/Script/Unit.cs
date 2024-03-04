using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Unit : MonoBehaviour
{
    public float maxHealth = 1000;
    public float health;
    public float damage = 10f;
    public float defense = 5f;
    public float attackRange = 5f;
    public float attackSpeed = 1f;
    public float movementSpeed = 3f;
    public float critHitChance = 0.2f;
    public float upgradeMultiplier = 1.5f;

    public int unitCost = 3;
    public int upgradeCost = 5;
    public int upgradeLevel = 0;
    public int maxUpgradeLevel = 3;

    public string faction;

    public TextMeshPro healthbar;
    
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
        UpdateHealth();
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
                AttackNearestTarget();
            }
        }
        UpdateHealth();
    }
    public void UpdateHealth()
    {
        if (GameManager.Instance.isPaused)
        {
            health = maxHealth;
        }
        healthbar.text = health + "/" + upgradeLevel;
    }
    private void MoveTowardsNearestTarget()
    {
        // Find the nearest target unit
        Unit nearestTarget = FindNearestUnitWithTag(targetTag);

        // Move towards the target if found
        if (nearestTarget != null)
        {
            Vector3 direction = (nearestTarget.transform.position - transform.position).normalized;
            transform.Translate(movementSpeed * Time.deltaTime * direction);
        }
    }
    private void AttackNearestTarget()
    {
        // Simulate attacking when in range
        
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
    // method for attacking another unit
    public void Attack(Unit target)
    {
        // Check if the target is not null
        if (target != null)
        {
            // Check if enough time has passed since the last attack based on attack speed
            if (Time.time - lastAttackTime >= 1f / attackSpeed)
            {
                // Check for a critical hit
                bool isCritHit = Random.value <= critHitChance;

                // Calculate damage, considering critical hit
                float finalDamage = isCritHit ? damage * 1.5f : damage;

                // Perform attack logic 
                target.TakeDamage(finalDamage);
                lastAttackTime = Time.time; // Update last attack time
            }
        }
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
    void Die()
    {
        // Handle death logic
        Destroy(gameObject);
    }
}
