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