using UnityEngine;
using UnityEngine.UI;

public class UnitSpawnButton : MonoBehaviour
{
    // Reference to the unit prefab
    public GameObject unitPrefab;

    // This method will be called when the button is clicked
    public void SpawnUnit()
    {
        // Spawn the unit when the button is clicked
        Instantiate(unitPrefab, Vector3.zero, Quaternion.identity);
    }
}
