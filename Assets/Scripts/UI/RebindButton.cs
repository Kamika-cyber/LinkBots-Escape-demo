using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RebindButton : MonoBehaviour
{
    public enum ActionType
    {
        P1_Left,
        P1_Right,
        P1_Jump,
        P1_Shoot,

        P2_Left,
        P2_Right,
        P2_Jump,
        P2_Shoot
    }

    [Header("Binding")]
    public ActionType action;
    public TextMeshProUGUI label;

    [Header("Button Visual")]
    [SerializeField] private Image buttonImage;
    [SerializeField] private Sprite defaultSprite;

    [Header("Arrow Sprites")]
    [SerializeField] private Sprite leftArrowSprite;
    [SerializeField] private Sprite rightArrowSprite;
    [SerializeField] private Sprite upArrowSprite;
    [SerializeField] private Sprite downArrowSprite;

    private bool waitingForKey = false;

    private void Start()
    {
        if (buttonImage == null)
            buttonImage = GetComponent<Image>();

        InputManager.Load();
        UpdateLabel();
    }

    public void StartRebind()
    {
        waitingForKey = true;

        if (label != null)
            label.text = "...";
    }

    private void Update()
    {
        if (!waitingForKey) return;

        if (Input.anyKeyDown)
        {
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    SetKey(key);
                    waitingForKey = false;
                    InputManager.Save();
                    UpdateLabel();
                    break;
                }
            }
        }
    }

    private void SetKey(KeyCode key)
    {
        switch (action)
        {
            case ActionType.P1_Left:  InputManager.Player1_Left = key; break;
            case ActionType.P1_Right: InputManager.Player1_Right = key; break;
            case ActionType.P1_Jump:  InputManager.Player1_Jump = key; break;
            case ActionType.P1_Shoot: InputManager.Player1_Shoot = key; break;

            case ActionType.P2_Left:  InputManager.Player2_Left = key; break;
            case ActionType.P2_Right: InputManager.Player2_Right = key; break;
            case ActionType.P2_Jump:  InputManager.Player2_Jump = key; break;
            case ActionType.P2_Shoot: InputManager.Player2_Shoot = key; break;
        }
    }

    private void UpdateLabel()
    {
        KeyCode key = GetCurrentKey();
        UpdateVisualForKey(key);
    }

    private KeyCode GetCurrentKey()
    {
        switch (action)
        {
            case ActionType.P1_Left:  return InputManager.Player1_Left;
            case ActionType.P1_Right: return InputManager.Player1_Right;
            case ActionType.P1_Jump:  return InputManager.Player1_Jump;
            case ActionType.P1_Shoot: return InputManager.Player1_Shoot;

            case ActionType.P2_Left:  return InputManager.Player2_Left;
            case ActionType.P2_Right: return InputManager.Player2_Right;
            case ActionType.P2_Jump:  return InputManager.Player2_Jump;
            case ActionType.P2_Shoot: return InputManager.Player2_Shoot;
        }

        return KeyCode.None;
    }

    private void UpdateVisualForKey(KeyCode key)
    {
        bool isArrowKey =
            key == KeyCode.LeftArrow ||
            key == KeyCode.RightArrow ||
            key == KeyCode.UpArrow ||
            key == KeyCode.DownArrow;

        if (buttonImage != null)
        {
            buttonImage.sprite = defaultSprite;

            if (key == KeyCode.LeftArrow && leftArrowSprite != null)
                buttonImage.sprite = leftArrowSprite;
            else if (key == KeyCode.RightArrow && rightArrowSprite != null)
                buttonImage.sprite = rightArrowSprite;
            else if (key == KeyCode.UpArrow && upArrowSprite != null)
                buttonImage.sprite = upArrowSprite;
            else if (key == KeyCode.DownArrow && downArrowSprite != null)
                buttonImage.sprite = downArrowSprite;
        }

        if (label != null)
        {
            label.text = isArrowKey ? "" : key.ToString();
        }
    }
}