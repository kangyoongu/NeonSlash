using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public enum Clips
{
    PlayerShot,
    PlayerHit,
    PlayerDie,
    Wave,
    Upgrade,
    Healing,
    Inv,
    ScoreUP,
    Cancel,
    Dash,
    Skill1,
    OrbHit,
    Clear,
    Button,
    BossSpawn,
    PutMoney
}

[Serializable]
public struct Clips3D
{
    public AudioClip enemyShot;
    public AudioClip enemyDie;
    public AudioClip bossDash;
    public AudioClip bossLaser;
}

public class SoundManager : SingleTon<SoundManager>
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SFXSlider;
    private AudioSource _audioSource;

    [SerializeField] private List<AudioString> _audios;

    [SerializeField] private AudioClip inGameBGM;
    [SerializeField] private AudioClip mainBGM;
    [SerializeField] private AudioSource bgmSource;

    public Clips3D clips3D;

    private Dictionary<Clips, AudioClip> _audioDic = new Dictionary<Clips, AudioClip>();

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        for(int i = 0; i < _audios.Count; i++)
        {
            _audioDic.Add(_audios[i].name, _audios[i].clip);
        }
    }
    private void Start()
    {
        bgmSource.clip = mainBGM;
        bgmSource.Play();
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
    public void PlayAudio(Clips clips, float volumn = 0.4f)
    {
        _audioSource.PlayOneShot(_audioDic[clips], volumn);
    }
    public void PlayInGameSound()
    {
        bgmSource.clip = inGameBGM;
        bgmSource.Play();
    }
}

[Serializable]
public struct AudioString
{
    public AudioClip clip;
    public Clips name;
}