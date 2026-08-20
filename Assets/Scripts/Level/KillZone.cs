using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2") || other.CompareTag("Player3"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
                playerHealth.TakeHit();
            else if (GameOverUI.Instance != null)
                GameOverUI.Instance.ShowGameOver();
            else
                SceneLoader.ReloadCurrentLevel();
        }
    }
}