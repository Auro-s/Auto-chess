using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Button endButton;
    public Button startButton;
    public TextMeshProUGUI messageText;

    public bool isPaused = true;

    private readonly float messageDuration = 2f;

    private readonly int maxRaycastDistance = 100;
    public bool IsPaused()
    {
        return isPaused;
    }
    
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }
    void Update()
    {
        // Check if all ally units are dead
        bool allAllyUnitsDead = !GameObject.FindGameObjectsWithTag("Ally").Any();

        // Check if all enemy units are dead
        bool allEnemyUnitsDead = !GameObject.FindGameObjectsWithTag("Enemy").Any();

        // Activate the end button if all ally or enemy units are dead
        if (!isPaused && allAllyUnitsDead || allEnemyUnitsDead)
        {
            endButton.gameObject.SetActive(true);
            isPaused = true; // Pause the game when the end button is active

        }
        if (isPaused)
        {
            UpgradeUnit();
            Unit[] units = FindObjectsOfType<Unit>();

            foreach (Unit unit in units)
            {

                if (unit.TryGetComponent<Rigidbody2D>(out var unitRb))
                {
                    unitRb.bodyType = RigidbodyType2D.Kinematic;
                }
            }
        }
    }

    public void StartFight()
    {
        // Find all units in the scene
        Unit[] units = FindObjectsOfType<Unit>();

        // Check if there are any units with the "Ally" tag
        if (units.Any(unit => unit.CompareTag("Ally")))
        {
            // If there are, unpause the game and enable unit movement
            isPaused = false;

            // Enable Rigidbody2D for ally units
            foreach (Unit unit in units)
            {
                if (unit.CompareTag("Ally") && unit.TryGetComponent<Rigidbody2D>(out var unitRb))
                {
                    unitRb.bodyType = RigidbodyType2D.Dynamic;
                }
            }
            // Disable drag for ally units
            DisableDrag();
            if (startButton != null)
            {
                startButton.gameObject.SetActive(false);
            }
        }
        else
        {
            DisplayMessage("No units on field");
        }
    }
    public void DisableDrag()
    {
        GameObject[] allyGameObjects = GameObject.FindGameObjectsWithTag("Ally");

        foreach (GameObject allyGameObject in allyGameObjects)
        {
            
            if (allyGameObject.TryGetComponent<Draggable>(out var draggableComponent))
            {
                draggableComponent.enabled = false;
            }
        }
    }
    public void UpgradeUnit()
    {
        // Check if the right mouse button is pressed
        if (Input.GetMouseButtonDown(1))
        {
            // Cast a ray from the mouse position to the world
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Check if the ray hits a unit
            if (Physics2D.Raycast(ray.origin, ray.direction, maxRaycastDistance))
            {
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, maxRaycastDistance);
                GameObject hitObject = hit.collider.gameObject;

                // Check if the hit object has the Unit script
                if (hitObject.TryGetComponent<Unit>(out Unit unit) && unit.CompareTag("Ally"))
                {
                    // Check if the player has enough money to upgrade and the upgrade level is below the maximum
                    if (ShopManager.Instance.playerMoney >= unit.upgradeCost && unit.upgradeLevel < unit.maxUpgradeLevel)
                    {
                        // Deduct the upgrade cost from the player's money
                        ShopManager.Instance.playerMoney -= unit.upgradeCost;
                        ShopManager.Instance.UpdateMoneyText(); // Update the money display

                        // Increase unit statistics
                        unit.maxHealth *= unit.upgradeMultiplier;
                        unit.damage *= unit.upgradeMultiplier;
                        unit.defense *= unit.upgradeMultiplier;
                        unit.unitCost += 3;

                        // Update the health text after the upgrade
                        unit.UpdateHealth();

                        // Increment the upgrade level for the specific unit
                        unit.upgradeLevel++;

                        Debug.Log("Unit upgraded to level " + unit.upgradeLevel);
                    }
                    else if (unit.upgradeLevel >= unit.maxUpgradeLevel)
                    {
                        Debug.Log("Unit has reached the maximum upgrade level!");
                    }
                    else
                    {
                        Debug.Log("Not enough money to upgrade!");
                    }
                }
            }
        }
    }
    public void MainMenu()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void QuitGame()
    {
        Application.Quit();
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