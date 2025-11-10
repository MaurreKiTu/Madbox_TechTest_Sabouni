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


    [Header("Volume Settings")]
    [SerializeField] private float masterVolume = 1f;
    [SerializeField] private float sfxVolume = 1f;
    
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        _audioSource.playOnAwake = false;
    }

    private void PlaySoundInternal(SoundType soundType, float volumeMultiplier = 1f)
    {
        AudioClip clip = GetClipForSoundType(soundType);
        
        if (clip != null && _audioSource != null)
        {
            float volume = masterVolume * sfxVolume * volumeMultiplier;
            _audioSource.PlayOneShot(clip, volume);
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
}

