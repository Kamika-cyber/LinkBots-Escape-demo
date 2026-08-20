using UnityEngine;

public class ShieldPickup : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool destroyOnPickup = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();

            if (player != null)
            {
                player.GiveShield();

                if (destroyOnPickup)
                    Destroy(gameObject);
                else
                    gameObject.SetActive(false);
            }
        }
    }
}