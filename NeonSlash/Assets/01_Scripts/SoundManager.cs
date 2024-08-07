using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SFXSlider;

    private void Start()
    {
        Apply();
    }
    public void Apply()
    {
        BGMSlider.value = JsonManager.Instance.BGM;
        SFXSlider.value = JsonManager.Instance.SFX;
        audioMixer.SetFloat("BGM", Mathf.Log10(BGMSlider.value) * 20f);
        audioMixer.SetFloat("SFX", Mathf.Log10(SFXSlider.value) * 20f);
    }

    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20f);
        JsonManager.Instance.BGM = volume;
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20f);
        JsonManager.Instance.SFX = volume;
    }
}
