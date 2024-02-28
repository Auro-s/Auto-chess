using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float checkInterval = 0.5f; // How often to check for nearby units
    public List<Unit> allUnits = new List<Unit>();// Reference to all units in the game
    public Button endButton;


    private bool isPaused = true;
    

    void Start()
    {
        // Populate the 'allUnits' list with references to all Unit components in the scene
        allUnits.AddRange(FindObjectsOfType<Unit>());

        // Start the automatic attack routine
        InvokeRepeating("AutoAttack", 0f, checkInterval);
    }

    public void StartFight()
    {
        isPaused = false;
        gameObject.SetActive(false);
    }
    public void DisableDrag()
    {
        GameObject[] allyGameObjects = GameObject.FindGameObjectsWithTag("Ally");

        foreach (GameObject allyGameObject in allyGameObjects)
        {
            Draggable draggableComponent = allyGameObject.GetComponent<Draggable>();

            if (draggableComponent != null)
            {
                draggableComponent.enabled = false;
            }
        }
    }
    void AutoAttack()
    {
        if (!isPaused)
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
        CheckGameEnd();
    }

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

    void CheckGameEnd()
    {
        // Check if all ally units are dead
        bool allAllyUnitsDead = !GameObject.FindGameObjectsWithTag("Ally").Any();

        // Check if all enemy units are dead
        bool allEnemyUnitsDead = !GameObject.FindGameObjectsWithTag("Enemy").Any();

        // Activate the end button if all ally or enemy units are dead
        if (allAllyUnitsDead || allEnemyUnitsDead)
        {
            endButton.gameObject.SetActive(true);
            isPaused = true; // Pause the game when the end button is active
        }
    }
}