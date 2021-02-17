using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer MainSound;

    public Dropdown resolutionDropdown;

    Resolution[] resolutions;

    public Slider brightness;
    public static float intensity = 0.4f;

    public Slider sliderY;
    public Slider sliderX;
    public static float SensitivityY = 5;
    public static float SensitivityX = 5;
    public static bool smoothRotation = false;
    public static bool invertY = false;

    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " X " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetMasterLvl(float masterLvl)
    {
        MainSound.SetFloat("masterVol", masterLvl);
    }
    public void SetMusicLvl(float musicLvl)
    {
        MainSound.SetFloat("musicVol", musicLvl);
    }
    public void SetSfxLvl(float sfxLvl)
    {
        MainSound.SetFloat("sfxVol", sfxLvl);
    }
    public void SetBrightness()
    {
        intensity = brightness.value;
        RenderSettings.ambientLight = new Color(intensity, intensity, intensity, 1);
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetSensitivity(bool Y)
    {
        if(Y)
            SensitivityY = sliderY.value;
        else
            SensitivityX = sliderX.value;
    }
    public void SetControlInvertY()
    {
        invertY = !invertY;
    }
    public void SetControlSmooth()
    {
        smoothRotation = !smoothRotation;
    }
}