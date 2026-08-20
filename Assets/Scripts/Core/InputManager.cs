using UnityEngine;

public static class InputManager
{
    public static KeyCode Player1_Left;
    public static KeyCode Player1_Right;
    public static KeyCode Player1_Jump;
    public static KeyCode Player1_Shoot;

    public static KeyCode Player2_Left;
    public static KeyCode Player2_Right;
    public static KeyCode Player2_Jump;
    public static KeyCode Player2_Shoot;

    public static void Load()
    {
        Player1_Left  = ParseKey(PlayerPrefs.GetString("P1_Left", "A"), KeyCode.A);
        Player1_Right = ParseKey(PlayerPrefs.GetString("P1_Right", "D"), KeyCode.D);
        Player1_Jump  = ParseKey(PlayerPrefs.GetString("P1_Jump", "W"), KeyCode.W);
        Player1_Shoot = ParseKey(PlayerPrefs.GetString("P1_Shoot", "S"), KeyCode.Space);

        Player2_Left  = ParseKey(PlayerPrefs.GetString("P2_Left", "LeftArrow"), KeyCode.LeftArrow);
        Player2_Right = ParseKey(PlayerPrefs.GetString("P2_Right", "RightArrow"), KeyCode.RightArrow);
        Player2_Jump  = ParseKey(PlayerPrefs.GetString("P2_Jump", "UpArrow"), KeyCode.UpArrow);
        Player2_Shoot = ParseKey(PlayerPrefs.GetString("P2_Shoot", "DownArrow"), KeyCode.RightControl);
    }

    public static void Save()
    {
        PlayerPrefs.SetString("P1_Left", Player1_Left.ToString());
        PlayerPrefs.SetString("P1_Right", Player1_Right.ToString());
        PlayerPrefs.SetString("P1_Jump", Player1_Jump.ToString());
        PlayerPrefs.SetString("P1_Shoot", Player1_Shoot.ToString());

        PlayerPrefs.SetString("P2_Left", Player2_Left.ToString());
        PlayerPrefs.SetString("P2_Right", Player2_Right.ToString());
        PlayerPrefs.SetString("P2_Jump", Player2_Jump.ToString());
        PlayerPrefs.SetString("P2_Shoot", Player2_Shoot.ToString());

        PlayerPrefs.Save();
    }

    private static KeyCode ParseKey(string keyName, KeyCode fallback)
    {
        if (System.Enum.TryParse(keyName, out KeyCode parsedKey))
            return parsedKey;

        return fallback;
    }
}