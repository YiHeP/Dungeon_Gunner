using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class SoundEffectManager : SingletonMonobehaviour<SoundEffectManager>
{
    public int soundsVolume = 8;

    private void Start()
    {
        SetSoundVolume(soundsVolume);
    }

    public void PlaySoundEffect(SoundEffectSO soundEffect)
    {
        SoundEffect sound = (SoundEffect)PoolManager.Instance.ReuseComponent(soundEffect.soundEffectPrefab,Vector3.zero,Quaternion.identity);
        sound.SetSound(soundEffect);
        sound.gameObject.SetActive(true);
        StartCoroutine(DisableSound(sound, soundEffect.soundEffectClip.length));
    }

    private IEnumerator DisableSound(SoundEffect soundEffect, float soundDuration)
    {
        yield return new WaitForSeconds(soundDuration);

        soundEffect.gameObject.SetActive(false);
    }

    private void SetSoundVolume(int volume)
    {
        float muteDecibels = -80f;

        if(volume == 0)
        {
            GameResources.Instance.soundMasterMixrGroup.audioMixer.SetFloat("soundsVolume", muteDecibels);
        }
        else
        {
            GameResources.Instance.soundMasterMixrGroup.audioMixer.SetFloat("soundsVolume",HelpUtilities.LinearToDecibels(volume));
        }
    }

    public void IncreaseSoundVolume()
    {
        int maxSoundVoulme = 20;

        if (soundsVolume >= maxSoundVoulme)
        {
            return;
        }

        soundsVolume += 1;
        SetSoundVolume(soundsVolume);
    }

    public void DecreaseSoundVolume()
    {
        if (soundsVolume == 0) return;

        soundsVolume -= 1;

        SetSoundVolume(soundsVolume);
    }

}
