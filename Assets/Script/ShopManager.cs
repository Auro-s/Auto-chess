using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    // Reference to the player's money text
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI unitCountText;
    public TextMeshProUGUI messageText;
    public Button sellButton;
    public int playerMoney = 10;
    public int currentUnitCount = 0;
    public int maxUnitCount = 5; // Maximum allowed units
    public int unitCountIncrementCost = 5; // Cost to increment max unit count

    private readonly float messageDuration = 2f;
    private readonly int maxRaycastDistance = 100;
    private bool sellMode = false;

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
        if (sellMode)
        {
            SellUnit();
            Image buttonImage = sellButton.GetComponent<Image>();
            buttonImage.color = Color.red;
        }
        if (!sellMode)
        {
            SellUnit();
            Image buttonImage = sellButton.GetComponent<Image>();
            buttonImage.color = Color.white;
        }
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
        if (currentUnitCount < maxUnitCount)
        {
            unitCountText.color = Color.white;
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

    // Function to increment max unit count by paying a certain amount of money
    public void IncrementMaxUnitCount()
    {
        if (playerMoney >= unitCountIncrementCost)
        {
            playerMoney -= unitCountIncrementCost;
            maxUnitCount++;
            UpdateMoneyText();
            UpdateUnitCountText();
        }
        else
        {
            DisplayMessage("Not enough money!");
        }
    }
    public void EnterSellMode()
    {
        sellMode = true;
    }
    private void SellUnit()
    {
        if (sellMode && Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the mouse position to the world
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Check if the ray hits a unit
            if (Physics2D.Raycast(ray.origin, ray.direction, maxRaycastDistance))
            {
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, maxRaycastDistance);
                GameObject hitObject = hit.collider.gameObject;

                // Check if the hit object has the Unit script
                if (hitObject.CompareTag("Ally") && hitObject.TryGetComponent<Unit>(out var unit))
                {
                    // Increase player money by the unit cost
                    playerMoney += unit.unitCost - 1;
                    UpdateMoneyText(); // Update the money display

                    // Destroy the unit
                    Destroy(hitObject);
                }
            }
            // Exit sell mode after attempting to sell a unit
            sellMode = false;
        }
    }
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