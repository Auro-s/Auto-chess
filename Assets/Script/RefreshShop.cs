using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class RefreshShop : MonoBehaviour
{
    public GameObject[] targetButton;
    public int refreshCost = 2;
    public TextMeshProUGUI messageText;
    private readonly float messageDuration = 2f;

    // Method to toggle the activation status of the button
    public void ToggleButton()
    {
        ShopManager shopManager = FindObjectOfType<ShopManager>();
        
        if (AllButtonsActive())
        {
            // Display a message indicating that buttons are already active
            DisplayMessage("No need to refresh!");
        }
        else if (shopManager != null && shopManager.playerMoney >= refreshCost)
        {
            // Deduct the cost from the player's money
            shopManager.playerMoney -= refreshCost;
            shopManager.UpdateMoneyText(); // Update the money display

            foreach (GameObject target in targetButton)
            {
                if (target != null && !target.activeSelf)
                {
                    target.SetActive(true);
                }

            }
        }
        else
        {
            // Display an error message
            DisplayMessage("Not enough money!");
        }
        
    }
    
    // Function to check if all buttons are already active
    private bool AllButtonsActive()
    {
        foreach (GameObject target in targetButton)
        {
            if (target != null && !target.activeSelf)
            {
                return false; // At least one button is not active
            }
        }
        return true; // All buttons are already active
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
