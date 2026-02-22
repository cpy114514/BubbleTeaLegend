using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 开始界面设置：打开/关闭设置面板，可选音乐音量等。
/// 挂在 Canvas 上，并拖好设置面板、设置按钮、关闭按钮引用。
/// </summary>
public class StartMenuSettings : MonoBehaviour
{
    [Header("设置界面")]
    public GameObject settingsPanel;
    public Button openSettingsButton;
    public Button closeSettingsButton;

    [Header("音乐音量（可选）")]
    public Slider musicVolumeSlider;
    public string volumePrefKey = "MusicVolume";

    void Start()
    {
        if (openSettingsButton != null)
            openSettingsButton.onClick.AddListener(OpenSettings);
        if (closeSettingsButton != null)
            closeSettingsButton.onClick.AddListener(CloseSettings);

        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        if (musicVolumeSlider != null)
        {
            float saved = PlayerPrefs.GetFloat(volumePrefKey, 1f);
            musicVolumeSlider.value = saved;
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            ApplyMusicVolume(saved);
        }
    }

    public void OpenSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    void OnMusicVolumeChanged(float value)
    {
        ApplyMusicVolume(value);
        PlayerPrefs.SetFloat(volumePrefKey, value);
        PlayerPrefs.Save();
    }

    void ApplyMusicVolume(float value)
    {
        AudioListener.volume = value;
    }
}
