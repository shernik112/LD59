using UnityEngine;

public class AudioService : OnBehaviour, IService
{
    public static AudioService Instance { get; private set; }

    [Header("Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Clips")]
    [SerializeField] private AudioClip mainTrack;

    [Header("Music Speed Settings")]
    [SerializeField] private float maxPitch = 1.4f;
    [SerializeField] private float pitchIncreasePerSecond = 0.01f;

    private float _currentPitch = 1f;

    protected override void OnInitialize()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (musicSource != null)
        {
            musicSource.clip = mainTrack;
            musicSource.loop = true;
            musicSource.pitch = 1f;
        }

        _currentPitch = 1f;
    }

    private void Start()
    {
        PlayMusic();
    }

    protected override void OnUpdate()
    {
        UpdateMusicSpeed();
    }

    private void UpdateMusicSpeed()
    {
        if (musicSource == null || !musicSource.isPlaying)
            return;

        _currentPitch += pitchIncreasePerSecond * Time.deltaTime;
        _currentPitch = Mathf.Clamp(_currentPitch, 1f, maxPitch);

        musicSource.pitch = _currentPitch;
    }

    private void PlayMusic()
    {
        if (musicSource == null || mainTrack == null) return;
        if (musicSource.isPlaying) return;

        musicSource.clip = mainTrack;
        musicSource.loop = true;
        musicSource.pitch = 1f;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource == null) return;

        musicSource.Stop();
    }

    public void PlaySFX(AudioClip sfx)
    {
        if (sfxSource == null || sfx == null) return;

        sfxSource.PlayOneShot(sfx, 1f);
    }
}