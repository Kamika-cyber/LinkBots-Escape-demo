using UnityEngine;

public class LevelProgressManager : MonoBehaviour
{
    public static LevelProgressManager Instance;

    private const string HighestUnlockedLevelKey = "HighestUnlockedLevel";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeProgress();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeProgress()
    {
        if (!PlayerPrefs.HasKey(HighestUnlockedLevelKey))
        {
            PlayerPrefs.SetInt(HighestUnlockedLevelKey, 1);
            PlayerPrefs.Save();
        }
    }

    public int GetHighestUnlockedLevel()
    {
        return PlayerPrefs.GetInt(HighestUnlockedLevelKey, 1);
    }

    public bool IsLevelUnlocked(int levelNumber)
    {
        return levelNumber <= GetHighestUnlockedLevel();
    }

    public void UnlockNextLevel(int completedLevel)
    {
        int currentHighest = GetHighestUnlockedLevel();
        int nextLevel = completedLevel + 1;

        if (nextLevel > currentHighest)
        {
            PlayerPrefs.SetInt(HighestUnlockedLevelKey, nextLevel);
            PlayerPrefs.Save();
        }
    }

    public void ResetProgress()
    {
        PlayerPrefs.SetInt(HighestUnlockedLevelKey, 1);
        PlayerPrefs.Save();
    }
}