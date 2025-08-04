using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetMusicVolume()
    {
        SetVolume("MusicVolume", musicVolumeSlider.value);
    }
    public void SetSFXVolume()
    {
        SetVolume("SFXVolume", sfxVolumeSlider.value);
    }
    public void SetMasterVolume()
    {
        SetVolume("MasterVolume", masterVolumeSlider.value);
    }   
    public void SetVolume(string name,float volume)
    {
        float db = Mathf.Log10(volume) * 20; 
        if (volume == 0)
        {
            db = -80; 
        }
        audioMixer.SetFloat(name, db);
    }
}
