using UnityEngine;

public class FirePowerPickup : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float durationToGive = 8f;
    [SerializeField] private bool destroyOnPickup = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player1") && !other.CompareTag("Player2"))
            return;

        PlayerShooter shooter = other.GetComponent<PlayerShooter>();
        if (shooter != null)
        {
            shooter.GiveFirePower(durationToGive);

            if (destroyOnPickup)
                Destroy(gameObject);
            else
                gameObject.SetActive(false);
        }
    }
}