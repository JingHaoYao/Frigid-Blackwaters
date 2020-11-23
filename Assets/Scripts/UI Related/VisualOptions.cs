using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualOptions : MonoBehaviour
{
    Resolution[] resolutions;
    [SerializeField] Dropdown resolutionDropDown;
    [SerializeField] Dropdown qualityDropDown;
    [SerializeField] Toggle fullScreenToggle;

    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropDown.ClearOptions();

        List<string> dropDownOptions = new List<string>();
        int currentResolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            dropDownOptions.Add(resolutions[i].width + " x " + resolutions[i].height);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
                MiscData.resolutionIndex = i;
            }
        }

        resolutionDropDown.value = currentResolutionIndex;
        resolutionDropDown.RefreshShownValue();

        resolutionDropDown.AddOptions(dropDownOptions);

        this.gameObject.SetActive(false);
    }

    public void UpdateMenuAndSettings()
    {
        QualitySettings.SetQualityLevel(MiscData.qualityIndex);
        qualityDropDown.value = MiscData.qualityIndex;
        qualityDropDown.RefreshShownValue();

        Resolution[] tempResolutions = Screen.resolutions;

        Screen.SetResolution(tempResolutions[MiscData.resolutionIndex].width, tempResolutions[MiscData.resolutionIndex].height, MiscData.fullScreen);
        resolutionDropDown.value = MiscData.resolutionIndex;
        resolutionDropDown.RefreshShownValue();

        Screen.fullScreen = MiscData.fullScreen;
        fullScreenToggle.isOn = MiscData.fullScreen;
        
    }

    public void SetQuality(int quality)
    {
        QualitySettings.SetQualityLevel(quality);
        MiscData.qualityIndex = quality;
        SaveSystem.SaveOptions();
        // Save this in save options
    }

    public void SetFullscreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
        MiscData.fullScreen = fullScreen;
        SaveSystem.SaveOptions();
    }

    public void SetResolution(int resolution)
    {
        if (resolutions != null)
        {
            Screen.SetResolution(resolutions[resolution].width, resolutions[resolution].height, Screen.fullScreen);
            MiscData.resolutionIndex = resolution;
            SaveSystem.SaveOptions();
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && this.gameObject.activeSelf == true)
        {
            this.gameObject.SetActive(false);
            Time.timeScale = 1;
            if (PlayerProperties.playerScript != null)
            {
                PlayerProperties.playerScript.windowAlreadyOpen = false;
            }
        }
    }

}
