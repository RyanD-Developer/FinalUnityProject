using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public AudioMixer m_audioMixer;

    Resolution[] m_resolutions;
    string[] m_qualitySettings;
    public TMP_Dropdown m_resolutionsDropdown;
    public TMP_Dropdown m_qualityDropdown;
    public Toggle m_fullscreenToggle;

    private void Start()
    {
        ConfigureResolutions();
        ConfigureQualitySettings();
        SetFullscreen(m_fullscreenToggle.isOn);
    }

    void ConfigureResolutions()
    {
        m_resolutions = Screen.resolutions;
        m_resolutionsDropdown.ClearOptions();
        List<string> resolutionOptions = new List<string>();
        int currentResolutionIndex = 0;
        for(int i = 0; i < m_resolutions.Length; i++)
        {
            string option = m_resolutions[i].width + "x" + m_resolutions[i].height;
            resolutionOptions.Add(option);

            if (m_resolutions[i].width == Screen.currentResolution.width && m_resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        m_resolutionsDropdown.AddOptions(resolutionOptions);
        m_resolutionsDropdown.value = currentResolutionIndex;
        m_resolutionsDropdown.RefreshShownValue();
    }

    void ConfigureQualitySettings()
    {
        m_qualitySettings = QualitySettings.names;
        m_qualityDropdown.ClearOptions();
        
        List<string> qualityOptions = new List<string>();

        int currentQualityIndex = QualitySettings.GetQualityLevel();

        for(int i = 0;i < m_qualitySettings.Length;i++)
        {
            string option = m_qualitySettings[i];
            qualityOptions.Add(option);
        }

        m_qualityDropdown.AddOptions(qualityOptions); 
        m_qualityDropdown.value = currentQualityIndex;
        m_qualityDropdown.RefreshShownValue();
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = m_resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetGraphicsQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex, true);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetVolume(float volume)
    {
        m_audioMixer.SetFloat("MainAudioVolume", volume);
    }
}
