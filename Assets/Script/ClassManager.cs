using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ClassManager : MonoBehaviour
{
    public static ClassManager Instance; 

    public TextMeshProUGUI countTextW;
    public TextMeshProUGUI countTextA;
    public TextMeshProUGUI countTextAS;
    public TextMeshProUGUI countTextM;
    public TextMeshProUGUI countTextT;
    public float warriorHealthBonus = 100f;
    public float tankDefenseBonus = 50f;
    public float assasinDamageBonus = 50f;
    public float archeMovementBonus = 2f;
    public float mageAtksBonus = 0.2f;

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
    void Update()
    {
        // Find all units and filter by the specified faction
        Unit[] unitW = FindObjectsOfType<Unit>().Where(unit => unit.faction == "Warrior").ToArray();
        Unit[] unitA = FindObjectsOfType<Unit>().Where(unit => unit.faction == "Archer").ToArray();
        Unit[] unitAS = FindObjectsOfType<Unit>().Where(unit => unit.faction == "Assassin").ToArray();
        Unit[] unitM = FindObjectsOfType<Unit>().Where(unit => unit.faction == "Mage").ToArray();
        Unit[] unitT = FindObjectsOfType<Unit>().Where(unit => unit.faction == "Tank").ToArray();

        // Display the count of units in the UI text
        countTextW.text = "" + unitW.Length.ToString();
        countTextA.text = "" + unitA.Length.ToString();
        countTextAS.text = "" + unitAS.Length.ToString();
        countTextM.text = "" + unitM.Length.ToString();
        countTextT.text = "" + unitT.Length.ToString();
    }
    public void ClassBonuses()
    {
        ApplyClassBonuses();
    }

    void ApplyClassBonuses()
    {
        // Find every unit with the "Warrior" faction
        Unit[] warriorUnits = GameObject.FindGameObjectsWithTag("Ally").Where(go => go.GetComponent<Unit>().faction == "Warrior").Select(go => go.GetComponent<Unit>()).ToArray();
        Unit[] tankUnits = GameObject.FindGameObjectsWithTag("Ally").Where(go => go.GetComponent<Unit>().faction == "Tank").Select(go => go.GetComponent<Unit>()).ToArray();
        Unit[] archerUnits = GameObject.FindGameObjectsWithTag("Ally").Where(go => go.GetComponent<Unit>().faction == "Archer").Select(go => go.GetComponent<Unit>()).ToArray();
        Unit[] mageUnits = GameObject.FindGameObjectsWithTag("Ally").Where(go => go.GetComponent<Unit>().faction == "Mage").Select(go => go.GetComponent<Unit>()).ToArray();
        Unit[] assassinUnits = GameObject.FindGameObjectsWithTag("Ally").Where(go => go.GetComponent<Unit>().faction == "Mage").Select(go => go.GetComponent<Unit>()).ToArray();

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
            foreach (Unit archerUnit in archerUnits)
            {
                allyUnit.GetComponent<Unit>().movementSpeed += archeMovementBonus;
            }
            foreach (Unit mageUnit in mageUnits)
            {
                allyUnit.GetComponent<Unit>().attackSpeed += mageAtksBonus;
            }
            foreach (Unit assassinUnit in assassinUnits)
            {
                allyUnit.GetComponent<Unit>().damage += assasinDamageBonus;
            }
        }
    }
}


