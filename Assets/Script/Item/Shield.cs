using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float defenseBoost = 150f;
    public static Shield Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void ActiveShield()
    {
        // Find all ally units in the scene
        Unit[] allyUnits = FindObjectsOfType<Unit>();

        // Iterate through each ally unit and boost their health
        foreach (Unit unit in allyUnits)
        {
            if (unit.CompareTag("Ally"))
            {
            unit.defense += defenseBoost;
            }
        }
    }
}
