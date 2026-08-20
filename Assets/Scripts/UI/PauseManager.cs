using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    [SerializeField] private GameObject pausePanel;

    private bool isPaused = false;

    public bool IsPaused => isPaused;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        ResumeGame();
    }

    private void Update()
    {
        if (GameOverUI.Instance != null && GameOverUI.Instance.IsGameOver)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        if (isPaused) return;

        isPaused = true;

        if (pausePanel != null)
            pausePanel.SetActive(true);

        Time.timeScale = 0f;

        AudioListener.pause = true;
    }

    public void ResumeGame()
    {
        isPaused = false;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        Time.timeScale = 1f;

        AudioListener.pause = false;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneLoader.ReloadCurrentLevel();
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneLoader.LoadSceneByName("MainMenu");
    }
}