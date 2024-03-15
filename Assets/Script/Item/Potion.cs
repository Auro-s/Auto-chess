using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public float healAmount = 100f; // Amount of health to heal
    public float healInterval = 3f; // Time interval between each healing
    public bool isHealing = false; // Flag to control the healing process

    static public Potion Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void StartContinuousHealing()
    {
        if (!isHealing && gameObject.activeInHierarchy)
        {
            Debug.Log("Mao");
            isHealing = true;
            StartCoroutine(ContinuousHealing());
        }
    }
    public void StopContinuousHealing()
    {
        isHealing = false; // Set the flag to stop healing
    }
    IEnumerator ContinuousHealing()
    {
        // Continue healing as long as the game is not paused and the object is active
        while (isHealing && gameObject.activeInHierarchy)
        {
            Debug.Log("heal");
            // Find all ally units in the scene
            Unit[] allyUnits = FindObjectsOfType<Unit>();

            // Iterate through each ally unit and heal their health
            foreach (Unit unit in allyUnits)
            {
                if (unit.CompareTag("Ally") && unit.health < unit.maxHealth)
                {
                    unit.health += healAmount;
                }
            }
            // Wait for the specified interval before the next healing
            yield return new WaitForSeconds(healInterval);
        }
    }
}
