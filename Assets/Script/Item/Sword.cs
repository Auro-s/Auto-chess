using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public float damageBoost = 100f;
    public static Sword Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void ActiveSword()   
    {
        // Find all ally units in the scene
        Unit[] allyUnits = FindObjectsOfType<Unit>();

        // Iterate through each ally unit and boost their health
        foreach (Unit unit in allyUnits)
        {
            if (unit.CompareTag("Ally"))
            {
                    unit.damage += damageBoost;
            }
        }
    }
}
