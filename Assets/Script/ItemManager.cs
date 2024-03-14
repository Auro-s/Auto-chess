using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public Image itemTalisman;
    public Image itemSword;
    public Image itemShield;
    public Image itemPotion;
    public Image itemGuinsoo;

    public List<Button> buttons = new();
    public Transform itemSpawn; 
    public Transform ItemDisplay;


    public static ItemManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void SpawnRandomButtons(int count)
    {
        // Make sure the count doesn't exceed the number of buttons in the list
        count = Mathf.Min(count, buttons.Count);

        // Shuffle the list of buttons using Fisher-Yates shuffle algorithm
        for (int i = 0; i < buttons.Count - 1; i++)
        {
            int randomIndex = Random.Range(i, buttons.Count);
            (buttons[i], buttons[randomIndex]) = (buttons[randomIndex], buttons[i]);
        }

        // Spawn the first 'count' buttons
        for (int i = 0; i < count; i++)
        {
            // Instantiate the button prefab
            Instantiate(buttons[i], itemSpawn);
        }
    }
    public void SpawnTalisman()
    {
        // Instantiate the item prefab at the specified position
        Instantiate(itemTalisman, ItemDisplay);
        // Destroy all child objects (buttons) of the itemSpawn
        foreach (Transform child in itemSpawn)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < buttons.Count; i++)
        {
            // Check if the current button's name matches the specified name
            if (buttons[i].name == "TalismanButton")
            {
                // Remove the button from the list
                buttons.RemoveAt(i);
                // Exit the loop since the button is removed
                break;
            }
        }
    }
    public void SpawnSword()
    {
        // Instantiate the item prefab at the specified position
        Instantiate(itemSword, ItemDisplay);
        // Destroy all child objects (buttons) of the itemSpawn
        foreach (Transform child in itemSpawn)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < buttons.Count; i++)
        {
            // Check if the current button's name matches the specified name
            if (buttons[i].name == "SwordButton")
            {
                // Remove the button from the list
                buttons.RemoveAt(i);
                // Exit the loop since the button is removed
                break;
            }
        }
    }
    public void SpawnShield()
    {
        // Instantiate the item prefab at the specified position
        Instantiate(itemShield, ItemDisplay);
        // Destroy all child objects (buttons) of the itemSpawn
        foreach (Transform child in itemSpawn)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < buttons.Count; i++)
        {
            // Check if the current button's name matches the specified name
            if (buttons[i].name == "ShieldButton")
            {
                // Remove the button from the list
                buttons.RemoveAt(i);
                // Exit the loop since the button is removed
                break;
            }
        }
    }
    public void SpawnPotion()
    {
        // Instantiate the item prefab at the specified position
        Instantiate(itemPotion, ItemDisplay);
        // Destroy all child objects (buttons) of the itemSpawn
        foreach (Transform child in itemSpawn)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < buttons.Count; i++)
        {
            // Check if the current button's name matches the specified name
            if (buttons[i].name == "PotionButton")
            {
                // Remove the button from the list
                buttons.RemoveAt(i);
                // Exit the loop since the button is removed
                break;
            }
        }
    }
    public void SpawnGuinsoo()
    {
        // Instantiate the item prefab at the specified position
        Instantiate(itemGuinsoo, ItemDisplay);
        // Destroy all child objects (buttons) of the itemSpawn
        foreach (Transform child in itemSpawn)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < buttons.Count; i++)
        {
            // Check if the current button's name matches the specified name
            if (buttons[i].name == "GuinsooButton")
            {
                // Remove the button from the list
                buttons.RemoveAt(i);
                // Exit the loop since the button is removed
                break;
            }
        }
    }
}

