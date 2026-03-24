using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    [Header("Volume Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float _musicVolume = 0.5f;
    [Range(0f, 1f)]
    [SerializeField] private float _sfxVolume = 0.7f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (_musicSource == null)
        {
            _musicSource = gameObject.AddComponent<AudioSource>();
            _musicSource.loop = true;
        }

        if (_sfxSource == null)
        {
            _sfxSource = gameObject.AddComponent<AudioSource>();
        }

        ApplyVolumes();
    }

    private void ApplyVolumes()
    {
        if (_musicSource != null) _musicSource.volume = _musicVolume;
        if (_sfxSource != null) _sfxSource.volume = _sfxVolume;
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && _sfxSource != null)
        {
            _sfxSource.PlayOneShot(clip);
        }
    }

    public void PlayMusic(AudioClip music)
    {
        if (music != null && _musicSource != null)
        {
            if (_musicSource.clip == music && _musicSource.isPlaying) return;
            _musicSource.clip = music;
            _musicSource.Play();
        }
    }

    public void SetMusicVolume(float volume)
    {
        _musicVolume = Mathf.Clamp01(volume);
        if (_musicSource != null) _musicSource.volume = _musicVolume;
    }

    public void SetSFXVolume(float volume)
    {
        _sfxVolume = Mathf.Clamp01(volume);
        if (_sfxSource != null) _sfxSource.volume = _sfxVolume;
    }

    public float GetMusicVolume() => _musicVolume;
    public float GetSFXVolume() => _sfxVolume;
}