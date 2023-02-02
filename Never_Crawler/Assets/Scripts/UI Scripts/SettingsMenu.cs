using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    

    public AudioMixerGroup masterAudioMixer;
    public AudioMixerGroup musicAudioMixer;
    public AudioMixerGroup soundEffectAudioMixer;

    [SerializeField] Slider _masterSlider;
    [SerializeField] Slider _musicSlider;
    [SerializeField] Slider _SFXSlider;

    Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();

        if(_animator != null)
        {
            _animator.Play("FadeIn");
        }
    }

    public void SetMasterAudioMixer(float volume)
    {
        masterAudioMixer.audioMixer.SetFloat("Master_Volume", Mathf.Log10(volume) * 20f);
    }

    public void SetMusicAudioMixer(float volume)
    {
        musicAudioMixer.audioMixer.SetFloat("Music_Volume", Mathf.Log10(volume) * 20f);
    }

    public void SetSoundAudioMixer(float volume)
    {
        soundEffectAudioMixer.audioMixer.SetFloat("FX_Volume", Mathf.Log10(volume) * 20f);
    }

    public void SetAudioSliders()
    {
        _masterSlider.value = GetMixerVolume(masterAudioMixer, "Master_Volume");
        _musicSlider.value = GetMixerVolume(musicAudioMixer, "Music_Volume");
        _SFXSlider.value = GetMixerVolume(soundEffectAudioMixer, "FX_Volume");
    }

    float GetMixerVolume(AudioMixerGroup mixerToGet, string ParameterName)
    {
        float tempVol = 0f;
        bool hasVal = masterAudioMixer.audioMixer.GetFloat(ParameterName, out tempVol);

        if (hasVal)
        {
            tempVol /= 20f;
            tempVol = Mathf.Pow(10, tempVol);
        }

        return tempVol;
    }

    public void CloseSettingsMenu()
    {
        if(_animator != null)
        {
            _animator.Play("FadeOut");
        }
    }

    public void FadeOutEnd()
    {
        if (FindObjectOfType<MainMenu>())
        {
            FindObjectOfType<MainMenu>().ActivateMenu();
        }
        else
        {

        }
    }
}
