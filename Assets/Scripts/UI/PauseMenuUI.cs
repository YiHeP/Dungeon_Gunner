using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; 

public class PauseMenuUI : MonoBehaviour
{
    #region Tooltip
    [Tooltip("填入音乐音量等级")]
    #endregion
    [SerializeField] private TextMeshProUGUI musicLevelText;

    #region Tooltip
    [Tooltip("填入音效音量等级")]
    #endregion
    [SerializeField] private TextMeshProUGUI soundLevelText;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator InitializeUI()
    {
        yield return null;

        soundLevelText.SetText(SoundEffectManager.Instance.soundsVolume.ToString());
        musicLevelText.SetText(MusicManager.Instance.musicVolume.ToString());
    }

    private void OnEnable()
    {
        Time.timeScale = 0f;

        StartCoroutine(InitializeUI());
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

    public void IncreaseMusicVolume()
    {
        MusicManager.Instance.IncreaseMusicVolume();

        musicLevelText.SetText(MusicManager.Instance.musicVolume.ToString());
    }

    public void DecreaseMusicVolume()
    {
        MusicManager.Instance.DecreaseMusicVolume();

        musicLevelText.SetText(MusicManager.Instance.musicVolume.ToString());
    }

    public void IncreaseSoundVolume()
    {
        SoundEffectManager.Instance.IncreaseSoundVolume();

        soundLevelText.SetText(SoundEffectManager.Instance.soundsVolume.ToString());
    }

    public void DecreaseSoundVolume()
    {
        SoundEffectManager.Instance.DecreaseSoundVolume();

        soundLevelText.SetText(SoundEffectManager.Instance.soundsVolume.ToString());
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelpUtilities.ValidateCheckNullValues(this, nameof(musicLevelText), musicLevelText);
        HelpUtilities.ValidateCheckNullValues(this, nameof(soundLevelText), soundLevelText);
    }
#endif
    #endregion
}
