using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Shield")]
    [SerializeField] private bool hasShield = false;
    [SerializeField] private GameObject shieldVisual;

    public void GiveShield()
    {
        hasShield = true;

        if (shieldVisual != null)
            shieldVisual.SetActive(true);
    }

    public void TakeHit()
    {
        if (GameOverUI.Instance != null && GameOverUI.Instance.IsGameOver)
            return;

        if (hasShield)
        {
            hasShield = false;
            Debug.Log(gameObject.name + " shield broken");
            OnShieldBreak();
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " died");

        if (GameOverUI.Instance != null)
            GameOverUI.Instance.ShowGameOver();
        else
            SceneLoader.ReloadCurrentLevel();
    }

    private void OnShieldBreak()
    {
        if (shieldVisual != null)
            shieldVisual.SetActive(false);
    }

    public bool HasShield()
    {
        return hasShield;
    }
}