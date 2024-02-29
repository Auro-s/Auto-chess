using UnityEngine;
using TMPro;
using System.Collections;
public class UnitSpawnButton : MonoBehaviour
{
    // Reference to the unit prefab
    public GameObject unitPrefab;
    public TextMeshProUGUI messageText;

    private float messageDuration = 2f;
    public void SpawnUnit()
    {
        // Access the UnitCost from the Unit script
        int unitCost = unitPrefab.GetComponent<Unit>().UnitCost;

        // Reference to the ShopManager script
        ShopManager shopManager = FindObjectOfType<ShopManager>();

        // Check if the player has enough money
        if (shopManager != null && shopManager.playerMoney >= unitCost)
        {
            // Deduct the unit cost from the player's money
            shopManager.playerMoney -= unitCost;
            shopManager.UpdateMoneyText(); // Update the money display

            // Spawn the unit at Vector3.zero with no rotation
            Instantiate(unitPrefab, Vector3.zero, Quaternion.identity);
        }
        else
        {
            // Display an error message
            DisplayMessage("Not enough money!");
        }
    }

    // Function to display a message on the UI text
    private void DisplayMessage(string message)
    {
        messageText.text = message;
        StartCoroutine(HideMessage());
    }

    // Coroutine to hide the message after a certain duration
    private IEnumerator HideMessage()
    {
        yield return new WaitForSeconds(messageDuration);
        messageText.text = ""; // Clear the message
    }
}
