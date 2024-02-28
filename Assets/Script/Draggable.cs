using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    private Vector2 initialPosition;
    private Collider2D unitCollider;
    private bool isDragging = false;
    void Start()
    {
        initialPosition = transform.position;
        unitCollider = GetComponent<Collider2D>();
    }
         void Update()
    {
        if (isDragging)
        {
            // Update the unit's position based on mouse drag
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition;
        }
    }
    void OnMouseDown()
    {
        // Store the initial position when the unit is clicked
        initialPosition = transform.position;
        isDragging = true;
    }

    void OnMouseUp()
    {
        isDragging = false;
        // Check for valid placement and return to the initial position if needed
        CheckValidPlacement();
    }
    void CheckValidPlacement()
    {
        Collider2D[] overlappingColliders = Physics2D.OverlapCircleAll(transform.position, unitCollider.bounds.size.x / 2);

        foreach (Collider2D collider in overlappingColliders)
        {
            if (collider != unitCollider)
            {
                // Check for collisions with other units
                if (collider.CompareTag("Ally") || collider.CompareTag("Enemy"))
                {
                    // If colliding with another unit, return to the initial position
                    transform.position = initialPosition;
                    return;
                }
            }
        }
    }
}

