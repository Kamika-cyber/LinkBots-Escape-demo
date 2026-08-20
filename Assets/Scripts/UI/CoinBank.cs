using UnityEngine;

public class CoinBank : MonoBehaviour
{
    public static CoinBank Instance;

    private const string CoinsKey = "TotalCoins";

    private int coins;
    public int Coins => coins;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadCoins();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadCoins()
    {
        coins = PlayerPrefs.GetInt(CoinsKey, 0);
    }

    private void SaveCoins()
    {
        PlayerPrefs.SetInt(CoinsKey, coins);
        PlayerPrefs.Save();
    }

    public void AddCoins(int amount)
    {
        if (amount <= 0) return;

        coins += amount;
        SaveCoins();
    }

    public bool SpendCoins(int amount)
    {
        if (amount <= 0) return true;
        if (coins < amount) return false;

        coins -= amount;
        SaveCoins();
        return true;
    }

    public int GetCoins()
    {
        return coins;
    }
}