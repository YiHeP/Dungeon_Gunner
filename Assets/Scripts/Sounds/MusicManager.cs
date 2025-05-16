using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class MusicManager : SingletonMonobehaviour<MusicManager>
{
    private AudioSource musicAudioSource = null;
    private AudioClip currentAudioClip = null;
    private Coroutine fadeOutMusicCoroutine;
    private Coroutine fadeInMusicCoroutine;
    public int musicVolume = 10;

    protected override void Awake()
    {
        base.Awake();

        musicAudioSource = GetComponent<AudioSource>();

        GameResources.Instance.musicOffSnapshot.TransitionTo(0);
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
            musicVolume = PlayerPrefs.GetInt("musicVolume");

        SetMusicVolume(musicVolume);
    }

    public void PlayMusic(MusicTrackSO musicTrack,float fadeOutTime = Settings.musicFadeOutTime, float fadeInTime = Settings.musicFadeInTime)
    {
        StartCoroutine(PlayMusicRoutine(musicTrack,fadeOutTime,fadeInTime));
    }

    private IEnumerator PlayMusicRoutine(MusicTrackSO musicTrack, float fadeOutTime, float fadeInTime)
    {
        if(fadeInMusicCoroutine != null)
        {
            StopCoroutine(fadeInMusicCoroutine);
        }

        if(fadeOutMusicCoroutine != null)
        {
            StopCoroutine(fadeOutMusicCoroutine);
        }

        if(musicTrack.audioClip != currentAudioClip)
        {
            currentAudioClip = musicTrack.audioClip;

            yield return fadeOutMusicCoroutine = StartCoroutine(FadeOutMusic(fadeOutTime));

            yield return fadeInMusicCoroutine = StartCoroutine(FadeInMusic(musicTrack,fadeInTime));
        }
        yield return null;
    }

    private IEnumerator FadeOutMusic(float fadeOutTime)
    {
        GameResources.Instance.musicLoweSnapshot.TransitionTo(fadeOutTime);

        yield return new WaitForSeconds(fadeOutTime);
    }

    private IEnumerator FadeInMusic(MusicTrackSO musicTrack,float fadeInTime)
    {
        musicAudioSource.clip = musicTrack.audioClip;
        musicAudioSource.volume = musicTrack.musicVolume;
        musicAudioSource.Play();

        GameResources.Instance.musicOnFullSnapshot.TransitionTo(fadeInTime);

        yield return new WaitForSeconds(fadeInTime);
    }

    public void IncreaseMusicVolume()
    {
        int maxMusicVoulme = 20;

        if(musicVolume >= maxMusicVoulme)
        {
            return;
        }

        musicVolume += 1;
        SetMusicVolume(musicVolume);
    }

    public void DecreaseMusicVolume()
    {
        if (musicVolume == 0) return;

        musicVolume -= 1;

        SetMusicVolume(musicVolume);
    }

    private void SetMusicVolume(int volume)
    {
        float muteDecibels = -80f;

        if(volume == 0)
        {
            GameResources.Instance.musicMasterMixerGroup.audioMixer.SetFloat("musicVolume", muteDecibels);
        }
        else
        {
            GameResources.Instance.musicMasterMixerGroup.audioMixer.SetFloat("musicVolume", HelpUtilities.LinearToDecibels(volume));
        }    
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("musicVolume", musicVolume);
    }
}
