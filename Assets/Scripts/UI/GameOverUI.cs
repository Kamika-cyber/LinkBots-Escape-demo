using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI Instance;

    [Header("Panels")]
    [SerializeField] private GameObject gameOverPanel;

    [Header("Scene Names")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private bool isGameOver = false;

    public bool IsGameOver => isGameOver;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        HideGameOverInstant();
    }

    public void ShowGameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        if (LevelDeathCounter.Instance != null)
            LevelDeathCounter.Instance.AddDeath();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        SceneLoader.ReloadCurrentLevel();
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        SceneLoader.LoadSceneByName(mainMenuSceneName);
    }

    public void HideGameOverInstant()
    {
        isGameOver = false;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        Time.timeScale = 1f;
    }
}