using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int value = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") ||
            collision.CompareTag("Player1") ||
            collision.CompareTag("Player2"))
        {
            if (CoinBank.Instance != null)
            {
                CoinBank.Instance.AddCoins(value);
            }
            else
            {
                Debug.LogWarning("CoinBank not found!");
            }

            Destroy(gameObject);
        }
    }
}