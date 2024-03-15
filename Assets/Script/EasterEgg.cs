using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EasterEgg : MonoBehaviour
{
    public int clickCount = 0;
    public void Counter()
    {
        clickCount++; // Increment the click count
        if (clickCount >= 4)
        {
            // Activate the button after 4 clicks
            gameObject.SetActive(true);
        }
    }
}
