using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveOptions
{
    //for save options like sound settings
    public float masterVolume;
    public float effectsVolume;
    public float musicVolume;
    public bool muted;

    public bool fullScreen;
    public int resolutionIndex;
    public int qualityIndex;

    public SaveOptions()
    {
        masterVolume = MiscData.masterVolume;
        effectsVolume = MiscData.effectsVolume;
        musicVolume = MiscData.musicVolume;
        muted = MiscData.muted;
        fullScreen = MiscData.fullScreen;
        qualityIndex = MiscData.qualityIndex;
        resolutionIndex = MiscData.resolutionIndex;
    }
}
