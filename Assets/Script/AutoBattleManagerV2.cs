using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AutoBattleManagerV2 : MonoBehaviour
{
    public float checkInterval = 0.5f; // How often to check for nearby units

    // Reference to all units in the game
    private List<Unit> allUnits = new List<Unit>();

    void Start()
    {
        // Populate the 'allUnits' list with references to all Unit components in the scene
        allUnits.AddRange(FindObjectsOfType<Unit>());

        // Start the automatic attack routine
        InvokeRepeating("AutoAttack", 0f, checkInterval);
    }

    void AutoAttack()
    {
        // Remove destroyed units from the list
        allUnits.RemoveAll(unit => unit == null);

        // Iterate through all units
        foreach (Unit unit in allUnits)
        {
            // Check if the unit has the "Ally" or "Enemy" tag and find nearby enemy or ally units
            if (unit != null && (unit.CompareTag("Ally") || unit.CompareTag("Enemy")))
            {
                string targetTag = (unit.CompareTag("Ally")) ? "Enemy" : "Ally";
                Unit target = FindNearestUnitWithTag(unit, targetTag);

                // Attack the nearest unit if it is not null and is within the attack range
                if (target != null && target.gameObject != null && Vector3.Distance(unit.transform.position, target.transform.position) <= unit.attackRange)
                {
                    unit.Attack(target);
                }
            }
        }
    }

    // Example method to find units in range with a specified tag
    Unit FindNearestUnitWithTag(Unit unit, string tag)
    {
        // Check if the unit is null before attempting to access its transform
        if (unit == null)
        {
            return null; // Return null if the unit is null
        }

        // Filter units with the specified tag
        IEnumerable<Unit> unitsWithTag = allUnits.Where(u => u.CompareTag(tag) && u != null && u.gameObject != null);

        // Find the nearest unit
        return unitsWithTag.OrderBy(u => Vector3.Distance(unit.transform.position, u.transform.position)).FirstOrDefault();
    }
}






