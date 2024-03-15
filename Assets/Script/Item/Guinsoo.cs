using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guinsoo : MonoBehaviour
{
    public float guinsoo = 0.01f; 
    static public Guinsoo Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void ApplyGuinsoo()
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
                    unit.attackSpeed += guinsoo;
                }
            }
        }
    }
}
