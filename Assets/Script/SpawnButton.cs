using UnityEngine;
using TMPro;
using System.Collections;
public class SpawnButton : MonoBehaviour
{
    // Reference to the unit prefab
    public GameObject unitPrefab;
    public TextMeshProUGUI messageText;

    private readonly float messageDuration = 2f;
    public void SpawnUnit()
    {
        ShopManager shopManager = FindObjectOfType<ShopManager>();

        if (shopManager != null && ShopManager.Instance.currentUnitCount == ShopManager.Instance.maxUnitCount)
        {
            // Show an error message or handle it in a way that fits your game logic
            DisplayMessage("Max unit count reached!");
            return; // Exit the function to prevent further execution
        }
        // Access the UnitCost from the Unit script
        int unitCost = unitPrefab.GetComponent<Unit>().unitCost;

        // Check if the player has enough money
        if (shopManager != null && shopManager.playerMoney >= unitCost)
        {
            // Deduct the unit cost from the player's money
            shopManager.playerMoney -= unitCost;
            shopManager.UpdateMoneyText(); // Update the money display

            float randomX = Random.Range(-5f, -1f); 
            float randomY = Random.Range(-2f, 2f);

            // Spawn the unit at random Vector3
            Instantiate(unitPrefab, new Vector3(randomX, randomY, 0f), Quaternion.identity);
            gameObject.SetActive(false);
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
