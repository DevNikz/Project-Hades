using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using UnityEngine.Audio;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    public Sound[] sounds;

    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource menuSource;
    [SerializeField] AudioMixer masterMixer;

    [Range(0.1f, 10f)][SerializeField] public float fadeThreshold = 0.1f;
    [ReadOnly] public float volumeTemp;

    private bool hasPlayedAlert;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        hasPlayedAlert = false;
    }

    private void Start()
    {
        sfxSource = GetComponentsInChildren<AudioSource>()[0]; //first in hierarchy
        musicSource = GetComponentsInChildren<AudioSource>()[1];
        menuSource = GetComponentsInChildren<AudioSource>()[2];

        SetMasterVolume(SaveManager.Instance.CurrentSettings.masterVolume);
        SetMusicVolume(SaveManager.Instance.CurrentSettings.musicVolume);
        SetSFXVolume(SaveManager.Instance.CurrentSettings.gameplayVolume);
        SetMenuVolume(SaveManager.Instance.CurrentSettings.menuVolume);

        SceneManager.sceneLoaded += OnSceneLoaded;
        SwitchAudio();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0: //Main Menu
                StopMusic();
                PlayMusic("ClockworkRondo"); //TitleMusic
                break;
            case 1: //Tutorial
            case 4: //Level1
            case 5: //Level2
            case 6: //Level3
                StopMusic();
                PlayMusic("LevelOne");
                break;
            case 2:
                StopMusic();
                PlayMusic("HubMain");
                break;
            case 7:
                StopMusic();
                PlayMusic("CronosMain");
                break;
        }
    }

    public void SwitchAudio()
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0: //Main Menu
                StopMusic();
                PlayMusic("ClockworkRondo"); //TitleMusic
                break;
            case 1: //Tutorial
            case 4: //Level1
            case 5: //Level2
            case 6: //Level3
                StopMusic();
                PlayMusic("LevelOne");
                break;
            case 2:
                StopMusic();
                PlayMusic("HubMain");
                break;
            case 7:
                StopMusic();
                PlayMusic("CronosMain");
                break;
        }
    }

    public void SetMasterVolume(float value)
    {
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
    }

    public void SetSFXVolume(float value)
    {
        masterMixer.SetFloat("SfxVolume", Mathf.Log10(value) * 20);
    }

    public void SetMusicVolume(float value)
    {
        masterMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    }

    public void SetMenuVolume(float value)
    {
        masterMixer.SetFloat("MenuVolume", Mathf.Log10(value) * 20);
    }

    /*
    //Experimental Features (Warning!!)
    public void FadeIn(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null) return;
        
        s.source.volume = 0;
        StartCoroutine(PlayFade(s,s.volume));
        s.source.Play();
    }

    public void FadeOut(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        StartCoroutine(StopFade(s, s.volume));
    }

    IEnumerator PlayFade(Sound s, float maxVol) {
        var timeElapsed = 0f;
        
        while(s.source.volume < maxVol) {
            s.source.volume = Mathf.Lerp(0 , maxVol , timeElapsed / fadeThreshold);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator StopFade(Sound s, float maxVol) {
        yield return new WaitForSeconds(1f);
        var timeElapsed = 0f;

        while(s.source.volume > 0) {
            s.source.volume = Mathf.Lerp(maxVol, 0, timeElapsed / fadeThreshold);
            volumeTemp = Mathf.Lerp(maxVol, 0, timeElapsed / fadeThreshold);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        if(s.source.volume == 0) s.source.Stop();
    }*/


    public void PlayMusic(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null || s.clip == null || musicSource == null) return;

        musicSource.clip = s.clip;
        musicSource.Play();
        musicSource.loop = true;
    }

    public void StopMusic() //Call this first before play music
    {
        if (musicSource != null && musicSource.isPlaying)
            musicSource.Stop();
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null || s.clip == null || sfxSource == null) return;    
        
        if(name == "Alerted")
        {
            if (!hasPlayedAlert)
            {
                sfxSource.PlayOneShot(s.clip, s.volume);
                hasPlayedAlert = true;
            }
            else return;
        }
        else sfxSource.PlayOneShot(s.clip, s.volume);
    }

    public void PlaySFXAtPosition(string name, Vector3 position)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null || s.clip == null) return;

        GameObject tempGO = new GameObject("TempSFX3D_" + s.name);
        tempGO.transform.position = position;

        AudioSource tempSource = tempGO.AddComponent<AudioSource>();
        tempSource.clip = s.clip;
        tempSource.volume = s.volume;
        tempSource.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
        tempSource.outputAudioMixerGroup = sfxSource.outputAudioMixerGroup;
        tempSource.spatialBlend = 1.0f;
        tempSource.minDistance = 1.0f;  
        tempSource.maxDistance = 90.0f;
        tempSource.rolloffMode = AudioRolloffMode.Linear;

        tempSource.Play();

        Destroy(tempGO, s.clip.length / tempSource.pitch);
    }

    public void PlayMenu(string name) //not used?
    {
        //AudioClip clip = Array.Find(soundList, sound => sound.name == name);
        //if (clip == null || menuSource == null) return;
        //menuSource.PlayOneShot(clip);
    }

    
}
