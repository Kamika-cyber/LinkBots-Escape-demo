using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    [Header("Level Info")]
    [SerializeField] private int levelNumber = 1;
    [SerializeField] private string sceneName = "Level_01";
    [SerializeField] private bool alwaysUnlocked = false;

    [Header("UI")]
    [SerializeField] private Button button;
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private TMP_Text levelText;

    private void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();
    }

    private void Start()
    {
        RefreshState();
    }

    public void RefreshState()
    {
        bool unlocked = IsUnlocked();

        if (button != null)
            button.interactable = unlocked;

        if (lockIcon != null)
            lockIcon.SetActive(!unlocked);

        if (levelText != null)
            levelText.alpha = unlocked ? 1f : 0.5f;
    }

    public void LoadLevel()
    {
        if (!IsUnlocked())
            return;

        SceneManager.LoadScene(sceneName);
    }

    private bool IsUnlocked()
    {
        if (alwaysUnlocked)
            return true;

        if (LevelProgressManager.Instance != null)
            return LevelProgressManager.Instance.IsLevelUnlocked(levelNumber);

        return true;
    }
}