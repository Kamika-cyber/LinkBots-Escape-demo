using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField] private AudioSource audioSource;

    private const string MusicVolumeKey = "MusicVolume";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadVolume();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (audioSource == null || clip == null) return;

        if (audioSource.clip == clip)
            return;

        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();

        // на всякий случай еще раз применяем сохраненную громкость
        audioSource.volume = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
    }

    public void StopMusic()
    {
        if (audioSource == null) return;
        audioSource.Stop();
    }

    public void SetVolume(float volume)
    {
        if (audioSource == null) return;

        volume = Mathf.Clamp01(volume);
        audioSource.volume = volume;

        PlayerPrefs.SetFloat(MusicVolumeKey, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        if (audioSource == null) return 1f;
        return audioSource.volume;
    }

    private void LoadVolume()
    {
        float savedVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);

        if (audioSource != null)
            audioSource.volume = savedVolume;
    }
}