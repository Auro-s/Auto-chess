using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public static Gem Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep the Gem object alive across scene changes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates in subsequent scenes
        }
    }
    public void Active()
    {
        // Check if the SpriteRenderer component exists
        if (TryGetComponent(out spriteRenderer))
        {
            // Activate the SpriteRenderer to make it visible
            spriteRenderer.enabled = true;
        }
    }
}
