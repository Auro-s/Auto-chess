using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public SpriteRenderer healthBarSpriteRenderer;
    public Unit unit;

    void Start()
    {
        // Assuming you have a reference to the Unit script on the same GameObject
        unit = GetComponent<Unit>();

        // Assuming you have a SpriteRenderer component on the same GameObject
        healthBarSpriteRenderer = GetComponent<SpriteRenderer>();

        if (healthBarSpriteRenderer == null || unit == null)
        {
            Debug.LogError("HealthBar or Unit component not assigned properly!");
            return;
        }

        // Set initial health bar size
        UpdateHealthBar();
    }

    void Update()
    {
        // Update health bar during runtime
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        if (unit != null && healthBarSpriteRenderer != null)
        {
            // Calculate the health ratio
            float healthRatio = unit.health / unit.maxHealth;

            // Scale the health bar sprite based on the health ratio
            Vector3 newScale = new Vector3(healthRatio, 1f, 1f);
            healthBarSpriteRenderer.transform.localScale = newScale;

            // Reset the unit's scale to its original scale
            unit.transform.localScale = Vector3.one;
        }
    }
}


