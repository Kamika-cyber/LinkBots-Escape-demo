using UnityEngine;

public class LevelMusic : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private AudioClip levelMusic;

    private void Start()
    {
        if (MusicManager.Instance == null)
        {
            Debug.LogWarning("MusicManager not found!");
            return;
        }

        if (levelMusic == null)
        {
            Debug.LogWarning("LevelMusic: AudioClip is not assigned!");
            return;
        }

        MusicManager.Instance.PlayMusic(levelMusic);
    }
}