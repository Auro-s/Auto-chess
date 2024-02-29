using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    // Reference to the player's money text
    public TextMeshProUGUI moneyText;
    public int playerMoney = 10;
    void Start()
    {
        // Set up the initial money
        UpdateMoneyText();
    }
    void UpdateMoneyText()
    {
        moneyText.text = "" + playerMoney;
    }
}