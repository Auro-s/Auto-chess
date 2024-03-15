using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talisman : MonoBehaviour
{
    public float healthBoostAmount = 500f; // Amount to increase health by

    public void ApplyHealthBoost()
    {
        // Check if this GameObject is active in the scene
        if (gameObject.activeInHierarchy)
        {
            // Find all ally units in the scene
            Unit[] allyUnits = FindObjectsOfType<Unit>();

            // Iterate through each ally unit and boost their health
            foreach (Unit unit in allyUnits)
            {
                if (unit.CompareTag("Ally"))
                {
                    unit.health += healthBoostAmount;
                }
            }
        }
    }
}
