using System.Collections;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public float healAmount = 100f;
    public float healInterval = 5f;
    public static Potion Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void Heal()
    {
        StartCoroutine(Healing());
    }
    IEnumerator Healing()
    {
        // Continue healing as long as the object is active
        while (!GameManager.Instance.isPaused && gameObject.activeSelf)
        {
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