using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;         //for background music
    public AudioSource sfxSource;           //for sound effects

    [Header("Music Tracks")]
    public AudioClip[] musicTracks;         //list of background music tracks
    public float transitionTime = 1f;       //time to transition between tracks

    private int currentTrackIndex = 0;

    [Range (0, 1)] public float masterVolume = 1f;
    [Range (0, 1)] public float musicVolume = 1f;
    [Range (0, 1)] public float sfxVolume = 1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //start playing the first track
        PlayMusicTrack(currentTrackIndex);
        ApplyVolume();
    }

    //function to play a specific sound effect on fire
    public void PlaySFX(AudioClip sfx)
    {
        sfxSource.PlayOneShot(sfx);
    }

    //function to play the next background music track in order from index
    public void PlayNextMusicTrack()
    {
        currentTrackIndex = (currentTrackIndex + 1) % musicTracks.Length;
        StartCoroutine(SmoothTransitionToTrack(currentTrackIndex));
    }

    //functio to play a specific background music track
    public void PlayMusicTrack(int index)
    {
        if (index >= 0 && index < musicTracks.Length)
        {
            musicSource.clip = musicTracks[index];
            musicSource.Play();
        }
    }

    //the function for smooth transition between music tracks
    private IEnumerator SmoothTransitionToTrack(int nextTrackIndex)
    {
        //fade out current track
        float startVolume = musicSource.volume;
        for (float t = 0; t < transitionTime; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0, t / transitionTime);
            yield return null;
        }

        //switch to the next track
        musicSource.clip = musicTracks[nextTrackIndex];
        musicSource.Play();

        //fade in the new track
        for (float t = 0; t < transitionTime; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(0, startVolume, t / transitionTime);
            yield return null;
        }

        musicSource.volume = startVolume;
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        ApplyVolume();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        ApplyVolume();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        ApplyVolume();
    }

    public void ApplyVolume()
    {
        musicSource.volume = musicVolume * masterVolume;
        sfxSource.volume = sfxVolume * masterVolume;
    }
}
