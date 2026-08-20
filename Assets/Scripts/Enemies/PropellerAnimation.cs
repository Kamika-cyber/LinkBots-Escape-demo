using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PropellerAnimation : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite blurSprite;
    [SerializeField] private Sprite fastSprite;

    [Header("Animation")]
    [SerializeField] private float frameDuration = 0.08f;

    private SpriteRenderer spriteRenderer;
    private float timer;
    private int frameIndex;

    private Sprite[] frames;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        frames = new Sprite[]
        {
            normalSprite,
            blurSprite,
            fastSprite,
            blurSprite,
            normalSprite
        };
    }

    private void Start()
    {
        if (frames[0] != null)
        {
            spriteRenderer.sprite = frames[0];
        }
    }

    private void Update()
    {
        if (frames == null || frames.Length == 0)
            return;

        timer += Time.deltaTime;

        if (timer >= frameDuration)
        {
            timer = 0f;
            frameIndex++;

            if (frameIndex >= frames.Length)
                frameIndex = 0;

            if (frames[frameIndex] != null)
            {
                spriteRenderer.sprite = frames[frameIndex];
            }
        }
    }
}