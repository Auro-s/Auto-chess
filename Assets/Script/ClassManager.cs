using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClassManager : MonoBehaviour
{
    public static ClassManager Instance; // Singleton instance

    public float warriorHealthBonus = 10f;
    public float tankDefenseBonus = 2f;

    void Awake()
    {
        // Ensure there's only one instance of ClassManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ApplyClassBonuses();
    }

    void ApplyClassBonuses()
    {
        // Find every unit with the "Warrior" faction
        Unit[] warriorUnits = GameObject.FindGameObjectsWithTag("Ally").Where(go => go.GetComponent<Unit>().faction == "Warrior").Select(go => go.GetComponent<Unit>()).ToArray();
        Unit[] tankUnits = GameObject.FindGameObjectsWithTag("Ally").Where(go => go.GetComponent<Unit>().faction == "Tank").Select(go => go.GetComponent<Unit>()).ToArray();
       
        // Find every ally unit and apply bonus health for each warrior unit
        foreach (GameObject allyUnit in GameObject.FindGameObjectsWithTag("Ally"))
        {
            foreach (Unit warriorUnit in warriorUnits)
            {
                allyUnit.GetComponent<Unit>().health += warriorHealthBonus;
            }
            foreach (Unit tankUnit in tankUnits)
            {
                allyUnit.GetComponent<Unit>().defense += tankDefenseBonus;
            }
        }
    }
}


