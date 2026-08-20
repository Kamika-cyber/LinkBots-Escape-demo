using UnityEngine;
using TMPro;

public class MoneyText : MonoBehaviour
{
    [SerializeField] private string prefix = "";

    private TMP_Text textComponent;

    private void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (textComponent == null) return;

        int coins = 0;

        if (CoinBank.Instance != null)
            coins = CoinBank.Instance.GetCoins();
        else
            coins = PlayerPrefs.GetInt("TotalCoins", 0);

        textComponent.text = prefix + coins.ToString();
    }
}