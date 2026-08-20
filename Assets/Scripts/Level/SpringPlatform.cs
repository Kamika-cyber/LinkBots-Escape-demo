using UnityEngine;
using System.Collections;

public class SpringPlatform : MonoBehaviour
{
    [Header("Bounce Settings")]
    [SerializeField] private float bounceForce = 18f;
    [SerializeField] private bool onlyFromAbove = true;

    [Header("Animation")]
    [SerializeField] private Transform visualToSquash;
    [SerializeField] private float squashY = 0.7f;
    [SerializeField] private float squashX = 1.15f;
    [SerializeField] private float squashDuration = 0.08f;

    [SerializeField] private AudioSource audioSource;

    private Vector3 originalScale;
    private bool isAnimating = false;

    private void Awake()
    {
        if (visualToSquash == null)
            visualToSquash = transform;
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        originalScale = visualToSquash.localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player1") &&
            !collision.gameObject.CompareTag("Player2") &&
            !collision.gameObject.CompareTag("Player"))
            return;

        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        if (onlyFromAbove)
        {
            bool hitFromAbove = false;

            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y < -0.5f)
                {
                    hitFromAbove = true;
                    break;
                }
            }

            if (!hitFromAbove)
                return;
        }

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);

        if (audioSource != null)
        {
            audioSource.Play();
        }

        if (!isAnimating)
            StartCoroutine(PlaySquashAnimation());
    }

    private IEnumerator PlaySquashAnimation()
    {
        isAnimating = true;

        Vector3 squashedScale = new Vector3(
            originalScale.x * squashX,
            originalScale.y * squashY,
            originalScale.z
        );

        float halfDuration = squashDuration * 0.5f;
        float timer = 0f;

        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            float t = timer / halfDuration;
            visualToSquash.localScale = Vector3.Lerp(originalScale, squashedScale, t);
            yield return null;
        }

        timer = 0f;

        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            float t = timer / halfDuration;
            visualToSquash.localScale = Vector3.Lerp(squashedScale, originalScale, t);
            yield return null;
        }

        visualToSquash.localScale = originalScale;
        isAnimating = false;
    }
}