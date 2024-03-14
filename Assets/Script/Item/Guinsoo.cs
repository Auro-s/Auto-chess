using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guinsoo : MonoBehaviour
{
    public float guinsoo = 0.01f;
 
    public static Guinsoo Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void Buff()
    {
        Debug.Log("applied");
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

