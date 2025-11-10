using UnityEngine;
using System.Collections;

public enum SoundType
{
    CoinPickup,
    Slowed,
    Punch,
    PortalBuy,
    PortalNotEnough,
    Nope,
    Scratch
}

public enum MusicType
{
    None,
    Race,
    GameOver
}

public class SoundManager : MonoBehaviour
{
    [Header("Sound Clips")]
    [SerializeField] private AudioClip coinPickupSound;
    [SerializeField] private AudioClip slowedSound;
    [SerializeField] private AudioClip punchSound;
    [SerializeField] private AudioClip portalBuySound;
    [SerializeField] private AudioClip portalNotEnoughSound;
    [SerializeField] private AudioClip nopeSound;
    [SerializeField] private AudioClip scratchSound;

    [Header("Music Clips")]
    [SerializeField] private AudioClip raceMusic;
    [SerializeField] private AudioClip gameOverMusic;

    [Header("Volume Settings")]
    [SerializeField] private float masterVolume = 1f;
    [SerializeField] private float sfxVolume = 1f;
    [SerializeField] private float musicVolume = 0.7f;
    
    [Header("Fade Settings")]
    [SerializeField] private float defaultFadeDuration = 1f;
    
    private AudioSource _sfxSource;
    private AudioSource _musicSource;
    private Coroutine _fadeCoroutine;

    private void Awake()
    {
        _sfxSource = GetComponent<AudioSource>();
        if (_sfxSource == null)
        {
            _sfxSource = gameObject.AddComponent<AudioSource>();
        }
        _sfxSource.playOnAwake = false;
        
        _musicSource = gameObject.AddComponent<AudioSource>();
        _musicSource.playOnAwake = false;
        _musicSource.loop = true;
        _musicSource.volume = 0f;
    }

    private void PlaySoundInternal(SoundType soundType, float volumeMultiplier = 1f)
    {
        AudioClip clip = GetClipForSoundType(soundType);
        
        if (clip != null && _sfxSource != null)
        {
            float volume = masterVolume * sfxVolume * volumeMultiplier;
            _sfxSource.PlayOneShot(clip, volume);
        }
    }

    private AudioClip GetClipForSoundType(SoundType soundType)
    {
        return soundType switch
        {
            SoundType.CoinPickup => coinPickupSound,
            SoundType.Slowed => slowedSound,
            SoundType.Punch => punchSound,
            SoundType.PortalBuy => portalBuySound,
            SoundType.PortalNotEnough => portalNotEnoughSound,
            SoundType.Nope => nopeSound,
            SoundType.Scratch => scratchSound,

            _ => null
        };
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }

    private IEnumerator PlaySoundDelayed(SoundType soundType, float delay, float volumeMultiplier)
    {
        yield return new WaitForSeconds(delay);
        PlaySoundInternal(soundType, volumeMultiplier);
    }

    public static void PlaySound(SoundType soundType, float delay = 0f, float volumeMultiplier = 1f)
    {
        SoundManager manager = FindObjectOfType<SoundManager>();
        if (manager != null)
        {
            if (delay > 0)
            {
                manager.StartCoroutine(manager.PlaySoundDelayed(soundType, delay, volumeMultiplier));
            }
            else
            {
                manager.PlaySoundInternal(soundType, volumeMultiplier);
            }
        }
    }

    #region Music Control

    private AudioClip GetMusicClip(MusicType musicType)
    {
        return musicType switch
        {
            MusicType.Race => raceMusic,
            MusicType.GameOver => gameOverMusic,
            _ => null
        };
    }

    private void PlayMusicInternal(MusicType musicType, float startTime = 0f, float fadeInDuration = 0f)
    {
        if (_musicSource == null) return;
        
        AudioClip clip = GetMusicClip(musicType);
        if (clip == null) return;
        
        _musicSource.clip = clip;
        _musicSource.time = startTime;
        _musicSource.Play();
        
        if (fadeInDuration > 0f)
        {
            FadeMusicIn(fadeInDuration);
        }
        else
        {
            _musicSource.volume = musicVolume;
        }
    }

    private void StopMusicInternal(float fadeOutDuration = 0f)
    {
        if (_musicSource == null) return;
        
        if (fadeOutDuration > 0f)
        {
            FadeMusicOut(fadeOutDuration, true);
        }
        else
        {
            _musicSource.Stop();
            _musicSource.volume = 0f;
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (_musicSource != null)
        {
            _musicSource.volume = musicVolume;
        }
    }

    #endregion

    #region Fade Effects

    public void FadeMusicIn(float duration)
    {
        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
        }
        _fadeCoroutine = StartCoroutine(FadeCoroutine(0f, musicVolume, duration, false));
    }

    public void FadeMusicOut(float duration, bool stopAfterFade = true)
    {
        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
        }
        _fadeCoroutine = StartCoroutine(FadeCoroutine(_musicSource.volume, 0f, duration, stopAfterFade));
    }

    public void CrossFadeMusic(MusicType newMusicType, float startTime = 0f, float crossFadeDuration = 1f)
    {
        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
        }
        _fadeCoroutine = StartCoroutine(CrossFadeCoroutine(newMusicType, startTime, crossFadeDuration));
    }

    private IEnumerator FadeCoroutine(float startVolume, float targetVolume, float duration, bool stopAfterFade)
    {
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            _musicSource.volume = Mathf.Lerp(startVolume, targetVolume, t);
            yield return null;
        }
        
        _musicSource.volume = targetVolume;
        
        if (stopAfterFade && targetVolume == 0f)
        {
            _musicSource.Stop();
        }
        
        _fadeCoroutine = null;
    }

    private IEnumerator CrossFadeCoroutine(MusicType newMusicType, float startTime, float duration)
    {
        float elapsed = 0f;
        float startVolume = _musicSource.volume;
        
        while (elapsed < duration / 2f)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / (duration / 2f);
            _musicSource.volume = Mathf.Lerp(startVolume, 0f, t);
            yield return null;
        }
        
        _musicSource.Stop();
        PlayMusicInternal(newMusicType, startTime, 0f);
        _musicSource.volume = 0f;
        
        elapsed = 0f;
        while (elapsed < duration / 2f)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / (duration / 2f);
            _musicSource.volume = Mathf.Lerp(0f, musicVolume, t);
            yield return null;
        }
        
        _musicSource.volume = musicVolume;
        _fadeCoroutine = null;
    }

    #endregion

    #region Static Music Methods

    public static void PlayMusic(MusicType musicType, float startTime = 0f, float fadeInDuration = 0f)
    {
        SoundManager manager = FindObjectOfType<SoundManager>();
        if (manager != null)
        {
            manager.PlayMusicInternal(musicType, startTime, fadeInDuration);
        }
    }

    public static void StopMusic(float fadeOutDuration = 0f)
    {
        SoundManager manager = FindObjectOfType<SoundManager>();
        if (manager != null)
        {
            manager.StopMusicInternal(fadeOutDuration);
        }
    }

    public static void FadeInMusic(float duration)
    {
        SoundManager manager = FindObjectOfType<SoundManager>();
        if (manager != null)
        {
            manager.FadeMusicIn(duration);
        }
    }

    public static void FadeOutMusic(float duration, bool stopAfterFade = true)
    {
        SoundManager manager = FindObjectOfType<SoundManager>();
        if (manager != null)
        {
            manager.FadeMusicOut(duration, stopAfterFade);
        }
    }

    public static void CrossFade(MusicType newMusicType, float startTime = 0f, float duration = 1f)
    {
        SoundManager manager = FindObjectOfType<SoundManager>();
        if (manager != null)
        {
            manager.CrossFadeMusic(newMusicType, startTime, duration);
        }
    }

    #endregion
}

