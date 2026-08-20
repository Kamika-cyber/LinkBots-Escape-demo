using UnityEngine;

public class PlayerSkinManager : MonoBehaviour
{
    public static PlayerSkinManager Instance;

    private const string Player1SkinKey = "Player1Skin";
    private const string Player2SkinKey = "Player2Skin";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPlayer1Skin(int skinIndex)
    {
        PlayerPrefs.SetInt(Player1SkinKey, skinIndex);
        PlayerPrefs.Save();
    }

    public void SetPlayer2Skin(int skinIndex)
    {
        PlayerPrefs.SetInt(Player2SkinKey, skinIndex);
        PlayerPrefs.Save();
    }

    public int GetPlayer1Skin()
    {
        return PlayerPrefs.GetInt(Player1SkinKey, 0);
    }

    public int GetPlayer2Skin()
    {
        return PlayerPrefs.GetInt(Player2SkinKey, 0);
    }
}