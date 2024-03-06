using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Button loseButton;
    public Button winButton;
    public Button startButton;
    public TextMeshProUGUI messageText;
    public GameObject[] secondFight;
    public GameObject[] thirdFight;
    public bool isPaused = true;
    public bool isFirstFight = true;

    private readonly float messageDuration = 2f;
    private readonly int maxRaycastDistance = 100;
    private readonly Dictionary<string, Vector3> allyPositions = new();

    public static GameManager Instance;
    public bool IsPaused()
    {
        return isPaused;
    }
    private void Awake()
    {
        // Singleton pattern to ensure there is only one instance
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        // Check if all ally units are dead
        bool allAllyUnitsDead = !GameObject.FindGameObjectsWithTag("Ally").Any();

        // Check if all enemy units are dead
        bool allEnemyUnitsDead = !GameObject.FindGameObjectsWithTag("Enemy").Any();

        // Activate the end button if all allies are dead
        if (!isPaused && allAllyUnitsDead)
        {
            loseButton.gameObject.SetActive(true);
            isPaused = true; // Pause the game when the end button is active
        }
        // Activate the win button if all enemies are dead
        if (!isPaused && allEnemyUnitsDead)
        {
            winButton.gameObject.SetActive(true);
            isPaused = true;
            ShopManager.Instance.playerMoney += 10;
            ShopManager.Instance.UpdateMoneyText();
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
            // unpause the game and enable unit movement
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
            startButton.gameObject.SetActive(false);
        }
        else
        {
            DisplayMessage("No units on field");
        }
    }
    public void StoreAllyPositions()
    {
        GameObject[] allyUnits = GameObject.FindGameObjectsWithTag("Ally");

        foreach (GameObject allyUnit in allyUnits)
        {
            if (!allyPositions.ContainsKey(allyUnit.name))
            {
                allyPositions.Add(allyUnit.name, allyUnit.transform.position);
            }
        }
    }
    public void ReplaceAllyPositions()
    {
        GameObject[] allyUnits = GameObject.FindGameObjectsWithTag("Ally");

        foreach (GameObject allyUnit in allyUnits)
        {
            if (allyPositions.ContainsKey(allyUnit.name))
            {
                // Set the position of the ally unit to the stored position
                allyUnit.transform.position = allyPositions[allyUnit.name];
            }
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
    public void AbleDrag()
    {
        GameObject[] allyGameObjects = GameObject.FindGameObjectsWithTag("Ally");

        foreach (GameObject allyGameObject in allyGameObjects)
        {
            if (allyGameObject.TryGetComponent<Draggable>(out var draggableComponent))
            {
                draggableComponent.enabled = true;
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

                    }
                    else if (unit.upgradeLevel >= unit.maxUpgradeLevel)
                    {
                        DisplayMessage("Unit has reached the maximum upgrade level!");
                    }
                    else
                    {
                        DisplayMessage("Not enough money to upgrade!");
                    }
                }
            }
        }
    }
    public void NextFight() //This only work if the fight are 3 and no more.
    {
        if (isFirstFight)
        {
            foreach (GameObject unit in secondFight)
            {
                if (unit != null)
                {
                    unit.SetActive(true);
                }
            }
            winButton.gameObject.SetActive(false);
            startButton.gameObject.SetActive(true);
            isFirstFight = false;
            AbleDrag();
        }
        else if (!isFirstFight)
        {
            foreach (GameObject unit in thirdFight)
            {
                if (unit != null)
                {
                    unit.SetActive(true);
                }
            }
            winButton.gameObject.SetActive(false);
            AbleDrag();
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
    public void MainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}