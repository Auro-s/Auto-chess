using UnityEngine;

public class Draggable : MonoBehaviour
{
    private Vector2 initialPosition;
    private Collider2D unitCollider;
    private bool isDragging = false;
    public float boundaryXMin, boundaryXMax, boundaryYMin, boundaryYMax;

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

            // Clamp the position within the boundaries
            float clampedX = Mathf.Clamp(mousePosition.x, boundaryXMin, boundaryXMax);
            float clampedY = Mathf.Clamp(mousePosition.y, boundaryYMin, boundaryYMax);

            transform.position = new Vector2(clampedX, clampedY);
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
        // Clamp the final position within the boundaries
        float clampedX = Mathf.Clamp(transform.position.x, boundaryXMin, boundaryXMax);
        float clampedY = Mathf.Clamp(transform.position.y, boundaryYMin, boundaryYMax);

        // Update the final position within the boundaries
        transform.position = new Vector2(clampedX, clampedY);

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
