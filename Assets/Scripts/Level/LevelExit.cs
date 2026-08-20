using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    private bool player1Inside;
    private bool player2Inside;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1"))
            player1Inside = true;

        if (other.CompareTag("Player2"))
            player2Inside = true;

        if (player1Inside && player2Inside)
        {
            int completedLevel = ExtractLevelNumberFromSceneName();

            if (LevelProgressManager.Instance != null && completedLevel > 0)
                LevelProgressManager.Instance.UnlockNextLevel(completedLevel);

            SceneLoader.LoadNextLevel();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player1"))
            player1Inside = false;

        if (other.CompareTag("Player2"))
            player2Inside = false;
    }

    private int ExtractLevelNumberFromSceneName()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene.StartsWith("Level_"))
        {
            string numberPart = currentScene.Replace("Level_", "");
            if (int.TryParse(numberPart, out int levelNumber))
                return levelNumber;
        }

        return -1;
    }
}