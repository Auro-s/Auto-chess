using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AutoBattleManager : MonoBehaviour
{
    public float checkInterval = 0.5f; // How often to check for nearby enemies, maybe place that in the Update?

    // Reference to all units in the game
    public Unit[] allUnits;

    void Start()
    {
        // Populate the 'allUnits' array with references to all Unit components in the scene
        allUnits = FindObjectsOfType<Unit>();

        // Start the automatic attack routine
        InvokeRepeating("AutoAttack", 0f, checkInterval);
    }

    void AutoAttack()
    {
        // Iterate through all units
        foreach (Unit unit in allUnits)
        {
            // Check if the unit has the "Ally" tag and find nearby enemy units
            if (unit != null && unit.CompareTag("Ally"))
            {
                Unit[] nearbyEnemies = FindEnemiesInRange(unit);

                // Attack the first enemy in range (you can modify this logic based on your requirements)
                if (nearbyEnemies.Length > 0)
                {
                    unit.TryAttack(nearbyEnemies[0]);
                }
            }
        }
    }

    // Example method to find enemy units in range
    Unit[] FindEnemiesInRange(Unit unit)
    {
        // Check if the unit is null before attempting to access its transform
        if (unit == null)
        {
            return new Unit[0]; // Return an empty array if the unit is null
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(unit.transform.position, unit.attackRange);

        // Filter colliders to get only enemy units
        return colliders
            .Where(collider => collider.CompareTag("Enemy"))
            .Select(collider => collider.GetComponent<Unit>())
            .ToArray();
    }
}

