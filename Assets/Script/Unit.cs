using System.Linq;
using System.Collections;
using UnityEngine;
using TMPro;

public class Unit : MonoBehaviour
{
    public float maxHealth;
    public float health;
    public float baseDamage;
    public float damage;
    public float baseDefense;
    public float defense;
    public float attackRange;
    public float baseAtks; //base stats needed for reset the bonus of items
    public float attackSpeed;
    public float movementSpeed;
    public float critHitChance = 0.2f;
    public int unitCost = 3;
    public int upgradeCost;
    public int Level = 1;
    public int maxLevel = 3;
    public string faction;
    public TextMeshPro healthbar;
    public AudioSource audioSource;
    public GameObject attackAnimation;
    
    private float lastAttackTime;
    private string targetTag; // The tag to identify the target (either "Ally" or "Enemy")

    public static Unit Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
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
                if (TryGetComponent<Rigidbody2D>(out var rb2D))
                {
                    rb2D.bodyType = RigidbodyType2D.Static; //block the collision with other units
                }
            }
        }
        UpdateHealth();
        upgradeCost = Level * 4;
    }
    public void Reset()
    {
        health = maxHealth;
        damage = baseDamage;
        defense = baseDefense;
        attackSpeed = baseAtks;
    }
    //set the health of units
    public void UpdateHealth()
    {
        if (GameManager.Instance.isPaused)
        {
            health = maxHealth;
        }
        healthbar.text = health + "/" + Level;
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
        // attacking when in range
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

                audioSource.Play();
                attackAnimation.SetActive(true); 
                StartCoroutine(DeactivateAfterDelay(0.2f));
                  
                if(Guinsoo.Instance != null)
                {
                    Guinsoo.Instance.ApplyGuinsoo(); 
                }
            }
        }
    }
    IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Deactivate the slash effect
        if (attackAnimation != null)
        {
            attackAnimation.SetActive(false);
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
