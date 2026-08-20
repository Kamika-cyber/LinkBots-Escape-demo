using UnityEngine;
using System.Collections;

public class DisappearingPlatform : MonoBehaviour
{
    public float visibleTime = 8f;
    public float invisibleTime = 8f;

    private Renderer platformRenderer;
    private Collider2D platformCollider;

    void Start()
    {
       
        platformRenderer = GetComponent<Renderer>();
        platformCollider = GetComponent<Collider2D>();

       
        StartCoroutine(PlatformLoop());
    }

    IEnumerator PlatformLoop()
    {
        while (true)
        {
            // visible
            platformRenderer.enabled = true;
            platformCollider.enabled = true;

            yield return new WaitForSeconds(visibleTime);

            // invisible
            platformRenderer.enabled = false;
            platformCollider.enabled = false;

            yield return new WaitForSeconds(invisibleTime);
        }
    }
}