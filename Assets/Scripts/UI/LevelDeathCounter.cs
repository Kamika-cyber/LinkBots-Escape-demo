using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDeathCounter : MonoBehaviour
{
    public static LevelDeathCounter Instance;

    private int deaths = 0;
    private string trackedSceneName = "";

    public int Deaths => deaths;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            trackedSceneName = SceneManager.GetActiveScene().name;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddDeath()
    {
        deaths++;
    }

    public void ResetDeaths()
    {
        deaths = 0;
    }

    public void SetTrackedScene(string sceneName)
    {
        trackedSceneName = sceneName;
    }

    public string GetTrackedScene()
    {
        return trackedSceneName;
    }
}