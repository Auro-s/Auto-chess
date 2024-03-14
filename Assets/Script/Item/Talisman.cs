using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talisman : MonoBehaviour
{
    public float healthBoost = 500f;
    public static Talisman Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void ActiveTalisman()
    {
        // Find all ally units in the scene
        Unit[] allyUnits = FindObjectsOfType<Unit>();

        // Iterate through each ally unit and boost their health
        foreach (Unit unit in allyUnits)
        {
            if (unit.CompareTag("Ally"))
            {
                unit.health += healthBoost;
            }
        }
    }
}
