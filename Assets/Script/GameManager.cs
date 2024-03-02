using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Button endButton;
    public bool isPaused = true;

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
        if (allAllyUnitsDead || allEnemyUnitsDead)
        {
            endButton.gameObject.SetActive(true);
            isPaused = true; // Pause the game when the end button is active
            Unit[] units = FindObjectsOfType<Unit>();

            foreach (Unit unit in units)
            {
                Rigidbody2D unitRb = unit.GetComponent<Rigidbody2D>();

                if (unitRb != null)
                {
                    unitRb.bodyType = RigidbodyType2D.Kinematic;
                }
            }
        }
    }
    
    public void StartFight()
    {
        isPaused = false;
        Unit[] units = FindObjectsOfType<Unit>();

        foreach (Unit unit in units)
        {
            Rigidbody2D unitRb = unit.GetComponent<Rigidbody2D>();

            if (unitRb != null)
            {
                unitRb.bodyType = RigidbodyType2D.Dynamic;
            }
        }
        DisableDrag();
    }
    public void DisableDrag()
    {
        GameObject[] allyGameObjects = GameObject.FindGameObjectsWithTag("Ally");

        foreach (GameObject allyGameObject in allyGameObjects)
        {
            Draggable draggableComponent = allyGameObject.GetComponent<Draggable>();

            if (draggableComponent != null)
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
}