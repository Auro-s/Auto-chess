using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    // Reference to the player's money text
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI unitCountText;

    public int playerMoney = 10;
    public int currentUnitCount = 0;
    public int maxUnitCount = 5; // Maximum allowed units

    private static ShopManager _instance;
    public static ShopManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ShopManager>();
            }
            return _instance;
        }
    }
    void Start()
    {
        // Set up the initial money
        UpdateMoneyText();
    }
    void Update()
    {
    UpdateUnitCountText();
    }
    public void UpdateMoneyText()
    {
        moneyText.text = "" + playerMoney;
    }
    // Function to update the unit count text
    public void UpdateUnitCountText()
    {
        currentUnitCount = GetTotalUnitCount();
        unitCountText.text = "Units: " + currentUnitCount + "/" + maxUnitCount;
        // change text color to red if the limit is reached
        if (currentUnitCount == maxUnitCount)
        {
            unitCountText.color = Color.red;
        }
    }

    // Function to check if more units can be spawned
    public bool CanSpawnMoreUnits()
    {
        int currentUnitCount = GetTotalUnitCount();
        return currentUnitCount < maxUnitCount;
    }

    // Function to get the total count of spawned units
    private int GetTotalUnitCount()
    {
        GameObject[] allyUnits = GameObject.FindGameObjectsWithTag("Ally");
        return allyUnits.Length;
    }
}