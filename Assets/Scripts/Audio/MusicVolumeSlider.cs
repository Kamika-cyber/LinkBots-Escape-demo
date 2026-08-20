using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        if (volumeSlider == null)
            volumeSlider = GetComponent<Slider>();

        if (volumeSlider == null) return;

        if (MusicManager.Instance != null)
        {
            volumeSlider.value = MusicManager.Instance.GetVolume();
        }
        else
        {
            volumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        }

        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    private void OnDestroy()
    {
        if (volumeSlider != null)
            volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
    }

    public void OnVolumeChanged(float value)
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.SetVolume(value);
        }
        else
        {
            PlayerPrefs.SetFloat("MusicVolume", value);
            PlayerPrefs.Save();
        }
    }
}