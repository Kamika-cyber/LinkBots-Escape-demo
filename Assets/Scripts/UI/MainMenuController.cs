using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] public GameObject mainMenuPanel;
    [SerializeField] public GameObject levelSelectPanel;
    [SerializeField] public GameObject settingsPanel;

    [Header("First Level Scene Name")]
    [SerializeField] private string firstLevelSceneName = "Level_01";

    private void Start()
    {
        ShowMainMenu();
    }

    public void OpenLevelSelect()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (levelSelectPanel != null) levelSelectPanel.SetActive(true);

        LevelButton[] buttons = levelSelectPanel.GetComponentsInChildren<LevelButton>(true);
        foreach (LevelButton levelButton in buttons)
        {
            levelButton.RefreshState();
        }
    }

    public void OpenSettings()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (levelSelectPanel != null) levelSelectPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        ShowMainMenu();
    }

    public void StartFirstLevel()
    {
        SceneManager.LoadScene(firstLevelSceneName);
    }

    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed");
    }

    private void ShowMainMenu()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (levelSelectPanel != null) levelSelectPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }
}