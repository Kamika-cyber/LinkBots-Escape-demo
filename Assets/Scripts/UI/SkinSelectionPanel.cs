using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinSelectionPanel : MonoBehaviour
{
    [System.Serializable]
    public class SkinData
    {
        public Sprite sprite;
        public int price;
        public bool unlockedByDefault;
        public string saveKey;
    }

    [Header("Player 1")]
    [SerializeField] private Image player1Preview;
    [SerializeField] private TMP_Text player1Label;
    [SerializeField] private TMP_Text player1PriceText;
    [SerializeField] private Button player1BuyButton;
    [SerializeField] private Button player1UseButton;
    [SerializeField] private SkinData[] player1Skins;

    [Header("Player 2")]
    [SerializeField] private Image player2Preview;
    [SerializeField] private TMP_Text player2Label;
    [SerializeField] private TMP_Text player2PriceText;
    [SerializeField] private Button player2BuyButton;
    [SerializeField] private Button player2UseButton;
    [SerializeField] private SkinData[] player2Skins;

    private int player1Index;
    private int player2Index;

    private void Start()
    {
        InitializeDefaultUnlocks();

        player1Index = Mathf.Clamp(PlayerPrefs.GetInt("Player1Skin", 0), 0, Mathf.Max(0, player1Skins.Length - 1));
        player2Index = Mathf.Clamp(PlayerPrefs.GetInt("Player2Skin", 0), 0, Mathf.Max(0, player2Skins.Length - 1));

        RefreshUI();
    }

    private void InitializeDefaultUnlocks()
    {
        InitializeSkinArray(player1Skins);
        InitializeSkinArray(player2Skins);
    }

    private void InitializeSkinArray(SkinData[] skins)
    {
        if (skins == null) return;

        foreach (var skin in skins)
        {
            if (skin == null || string.IsNullOrEmpty(skin.saveKey)) continue;

            if (skin.unlockedByDefault && !PlayerPrefs.HasKey(skin.saveKey))
            {
                PlayerPrefs.SetInt(skin.saveKey, 1);
            }
        }

        PlayerPrefs.Save();
    }

    private bool IsUnlocked(SkinData skin)
    {
        if (skin == null || string.IsNullOrEmpty(skin.saveKey))
            return false;

        return PlayerPrefs.GetInt(skin.saveKey, 0) == 1;
    }

    private void UnlockSkin(SkinData skin)
    {
        if (skin == null || string.IsNullOrEmpty(skin.saveKey))
            return;

        PlayerPrefs.SetInt(skin.saveKey, 1);
        PlayerPrefs.Save();
    }

    public void NextPlayer1Skin()
    {
        if (player1Skins == null || player1Skins.Length == 0) return;

        player1Index = (player1Index + 1) % player1Skins.Length;
        RefreshUI();
    }

    public void PrevPlayer1Skin()
    {
        if (player1Skins == null || player1Skins.Length == 0) return;

        player1Index--;
        if (player1Index < 0)
            player1Index = player1Skins.Length - 1;

        RefreshUI();
    }

    public void NextPlayer2Skin()
    {
        if (player2Skins == null || player2Skins.Length == 0) return;

        player2Index = (player2Index + 1) % player2Skins.Length;
        RefreshUI();
    }

    public void PrevPlayer2Skin()
    {
        if (player2Skins == null || player2Skins.Length == 0) return;

        player2Index--;
        if (player2Index < 0)
            player2Index = player2Skins.Length - 1;

        RefreshUI();
    }

    public void BuyPlayer1Skin()
    {
        BuySkin(player1Skins, player1Index);
        RefreshUI();
    }

    public void BuyPlayer2Skin()
    {
        BuySkin(player2Skins, player2Index);
        RefreshUI();
    }

    private void BuySkin(SkinData[] skins, int index)
    {
        if (skins == null || skins.Length == 0) return;
        if (index < 0 || index >= skins.Length) return;

        SkinData skin = skins[index];
        if (skin == null) return;
        if (IsUnlocked(skin)) return;

        if (CoinBank.Instance == null) return;

        if (CoinBank.Instance.SpendCoins(skin.price))
        {
            UnlockSkin(skin);
        }
    }

    public void UsePlayer1Skin()
    {
        if (player1Skins == null || player1Skins.Length == 0) return;

        SkinData skin = player1Skins[player1Index];
        if (skin == null || !IsUnlocked(skin)) return;

        PlayerPrefs.SetInt("Player1Skin", player1Index);
        PlayerPrefs.Save();

        RefreshUI();
    }

    public void UsePlayer2Skin()
    {
        if (player2Skins == null || player2Skins.Length == 0) return;

        SkinData skin = player2Skins[player2Index];
        if (skin == null || !IsUnlocked(skin)) return;

        PlayerPrefs.SetInt("Player2Skin", player2Index);
        PlayerPrefs.Save();

        RefreshUI();
    }

    private void RefreshUI()
    {
        RefreshPlayer1UI();
        RefreshPlayer2UI();
    }

    private void RefreshPlayer1UI()
    {
        if (player1Skins == null || player1Skins.Length == 0) return;

        SkinData skin = player1Skins[player1Index];
        bool unlocked = IsUnlocked(skin);
        bool selected = PlayerPrefs.GetInt("Player1Skin", 0) == player1Index;

        if (player1Preview != null)
            player1Preview.sprite = skin.sprite;

        if (player1Label != null)
            player1Label.text = "Skin " + (player1Index + 1);

        if (player1PriceText != null)
            player1PriceText.text = unlocked ? (selected ? "Selected" : "Unlocked") : ("Price: " + skin.price);

        if (player1BuyButton != null)
            player1BuyButton.gameObject.SetActive(!unlocked);

        if (player1UseButton != null)
            player1UseButton.gameObject.SetActive(unlocked && !selected);
    }

    private void RefreshPlayer2UI()
    {
        if (player2Skins == null || player2Skins.Length == 0) return;

        SkinData skin = player2Skins[player2Index];
        bool unlocked = IsUnlocked(skin);
        bool selected = PlayerPrefs.GetInt("Player2Skin", 0) == player2Index;

        if (player2Preview != null)
            player2Preview.sprite = skin.sprite;

        if (player2Label != null)
            player2Label.text = "Skin " + (player2Index + 1);

        if (player2PriceText != null)
            player2PriceText.text = unlocked ? (selected ? "Selected" : "Unlocked") : ("Price: " + skin.price);

        if (player2BuyButton != null)
            player2BuyButton.gameObject.SetActive(!unlocked);

        if (player2UseButton != null)
            player2UseButton.gameObject.SetActive(unlocked && !selected);
    }
}