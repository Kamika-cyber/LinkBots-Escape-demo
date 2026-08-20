using TMPro;
using UnityEngine;

public class LevelDeathText : MonoBehaviour
{
    [SerializeField] private string prefix = "Deaths: ";

    private TMP_Text textComponent;

    private void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (textComponent == null) return;

        int deaths = 0;

        if (LevelDeathCounter.Instance != null)
            deaths = LevelDeathCounter.Instance.Deaths;

        textComponent.text = prefix + deaths;
    }
}