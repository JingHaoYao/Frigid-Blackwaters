using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundOptions : MonoBehaviour
{
    public AudioMixer generalMixer;
    public Image buttonImage;
    public Sprite muted, unmuted;
    public Slider generalSlider, effectsSlider, musicSlider;

    [Header("Only for Title Screen")]
    public VisualOptions visualOptions;

    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "Title Screen")
        {
            SaveOptions loadedOptions = SaveSystem.GetSaveOptions();
            if (loadedOptions != null)
            {
                SaveSystem.LoadOptions(loadedOptions);
                visualOptions.UpdateMenuAndSettings();
            }
        }

        setGeneralVolume(MiscData.masterVolume);
        generalSlider.value = MiscData.masterVolume;
        setEffectsVolume(MiscData.effectsVolume);
        effectsSlider.value = MiscData.effectsVolume;
        setMusicVolume(MiscData.musicVolume);
        musicSlider.value = MiscData.musicVolume;
        if(MiscData.muted == true)
        {
            FindObjectOfType<AudioListener>().enabled = false;
            buttonImage.sprite = muted;
        }
        else
        {
            FindObjectOfType<AudioListener>().enabled = true;
            buttonImage.sprite = unmuted;
        }
        this.gameObject.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && this.gameObject.activeSelf == true)
        {
            this.gameObject.SetActive(false);
            Time.timeScale = 1;
            if (GameObject.Find("PlayerShip"))
            {
                GameObject.Find("PlayerShip").GetComponent<PlayerScript>().windowAlreadyOpen = false;
            }
        }
    }

    float conversionFunction(float sliderValue)
    {
        return Mathf.Log10(Mathf.Clamp(1 - Mathf.Abs(sliderValue / 80), 0.0001f, 1)) * 20;
    }

    public void setGeneralVolume(float volume)
    {
        generalMixer.SetFloat("masterVolume", conversionFunction(volume));
        MiscData.masterVolume = volume;
    }

    public void setEffectsVolume(float volume)
    {
        generalMixer.SetFloat("effectsVolume", conversionFunction(volume));
        MiscData.effectsVolume = volume;
    }

    public void setMusicVolume(float volume)
    {
        generalMixer.SetFloat("musicVolume", conversionFunction(volume));
        MiscData.musicVolume = volume;
    }

    public void muteButton()
    {
        if(MiscData.muted == false)
        {
            MiscData.muted = true;
            buttonImage.sprite = muted;
            AudioListener.volume = 0;

        }
        else
        {
            MiscData.muted = false;
            buttonImage.sprite = unmuted;
            AudioListener.volume = 1;
        }
    }

    private void OnDisable()
    {
        SaveSystem.SaveOptions();
    }
}
