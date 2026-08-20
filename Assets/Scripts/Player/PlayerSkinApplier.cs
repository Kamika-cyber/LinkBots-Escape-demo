using UnityEngine;

public class PlayerSkinApplier : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private int playerIndex = 1;

    [Header("Renderers")]
    [SerializeField] private SpriteRenderer bodyRenderer;
    [SerializeField] private SpriteRenderer leftLegRenderer;
    [SerializeField] private SpriteRenderer rightLegRenderer;

    [System.Serializable]
    public class Skin
    {
        public string skinName;

        public Sprite body;
        public Sprite leftLeg;
        public Sprite rightLeg;
    }

    [Header("Skins")]
    [SerializeField] private Skin[] skins;

    private void Start()
    {
        ApplySkin();
    }

    public void ApplySkin()
    {
        if (skins == null || skins.Length == 0) return;

        int skinIndex = playerIndex == 1
            ? PlayerPrefs.GetInt("Player1Skin", 0)
            : PlayerPrefs.GetInt("Player2Skin", 0);

        skinIndex = Mathf.Clamp(skinIndex, 0, skins.Length - 1);

        Skin skin = skins[skinIndex];

        if (bodyRenderer != null)
            bodyRenderer.sprite = skin.body;

        if (leftLegRenderer != null)
            leftLegRenderer.sprite = skin.leftLeg;

        if (rightLegRenderer != null)
            rightLegRenderer.sprite = skin.rightLeg;
    }
}