using UnityEngine;

public class AudioService : OnBehaviour, IService
{
    public static AudioService Instance { get; private set; }

    [Header("Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Clips")]
    [SerializeField] private AudioClip mainTrack;

    protected override void OnInitialize()
    {
        // ЖЁСТКИЙ singleton
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
        }
    }

    private void Start()
    {
        PlayMusic();
    }

    private void PlayMusic()
    {
        if (musicSource == null || mainTrack == null) return;
        if (musicSource.isPlaying) return;

        musicSource.clip = mainTrack;
        musicSource.loop = true;
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