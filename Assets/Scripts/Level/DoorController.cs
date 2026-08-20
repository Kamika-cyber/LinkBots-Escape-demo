using TMPro;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Door Visuals")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private Sprite openSprite;

    [Header("Next Level")]
    [SerializeField] private string nextSceneName = "Level_02";

    [Header("Settings")]
    [SerializeField] private bool requireBothPlayers = true;

    [Header("Keys")]
    [SerializeField] private int requiredKeys = 1;
    [SerializeField] private TMP_Text keyCounterText;

    private int collectedKeys = 0;
    private bool isOpen = false;
    private bool player1Inside = false;
    private bool player2Inside = false;

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        SetClosedVisual();
        UpdateKeyCounter();
    }

    public void RegisterCollectedKey()
    {
        if (isOpen) return;

        collectedKeys++;
        collectedKeys = Mathf.Clamp(collectedKeys, 0, requiredKeys);

        UpdateKeyCounter();

        if (collectedKeys >= requiredKeys)
        {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {
        isOpen = true;
        SetOpenVisual();
        UpdateKeyCounter();
    }

    private void UpdateKeyCounter()
    {
        if (keyCounterText == null) return;

        if (isOpen)
            keyCounterText.text = "Door Open";
        else
            keyCounterText.text = collectedKeys + " / " + requiredKeys;
    }

    private void SetClosedVisual()
    {
        if (spriteRenderer != null && closedSprite != null)
            spriteRenderer.sprite = closedSprite;
    }

    private void SetOpenVisual()
    {
        if (spriteRenderer != null && openSprite != null)
            spriteRenderer.sprite = openSprite;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1"))
            player1Inside = true;

        if (other.CompareTag("Player2"))
            player2Inside = true;

        TryLoadNextLevel();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player1"))
            player1Inside = false;

        if (other.CompareTag("Player2"))
            player2Inside = false;
    }

    private void TryLoadNextLevel()
    {
        if (!isOpen) return;

        bool canLoad = false;

        if (requireBothPlayers)
        {
            canLoad = player1Inside && player2Inside;
        }
        else
        {
            canLoad = player1Inside || player2Inside;
        }

        if (!canLoad) return;

        int completedLevel =     ExtractLevelNumberFromSceneName();
        if (LevelProgressManager.Instance != null && completedLevel > 0)
        {
            LevelProgressManager.Instance.UnlockNextLevel(completedLevel);
        }

        SceneLoader.LoadSceneByName(nextSceneName);
    }

    private int ExtractLevelNumberFromSceneName()
    {
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        if (currentScene.StartsWith("Level_"))
        {
            string numberPart = currentScene.Replace("Level_", "");
            if (int.TryParse(numberPart, out int levelNumber))
                return levelNumber;
        }

        return -1;
    }

    public bool IsOpen()
    {
        return isOpen;
    }

    public int GetCollectedKeys()
    {
        return collectedKeys;
    }

    public int GetRequiredKeys()
    {
        return requiredKeys;
    }
}