using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    public Sound[] sounds;

    [SerializeField] public AudioClip[] soundList;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource musicSource;

    [Range(0.1f, 10f)] [SerializeField] public float fadeThreshold = 0.1f;
    [ReadOnly] public float volumeTemp;


    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void Start()
    {
        sfxSource = GetComponentsInChildren<AudioSource>()[0]; //first in hierarchy
        musicSource = GetComponentsInChildren<AudioSource>()[1];

        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0: //Main Menu
                StopMusic();
                PlayMusic("ClockworkRondo"); //TitleMusic
                break;
            case 1: //Tutorial
            case 2:
            case 4: //Level1
            case 5: //Level2
            case 6: //Level3
            case 7:
                StopMusic();
                PlayMusic("LevelOne");
                break;
        }
    }

    public void SwitchAudio()
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0: //Main Menu
                StopMusic();
                PlayMusic("ClockwordRondo");
                break;
            case 1: //Tutorial
            case 2:
            case 4: //Level1
            case 5: //Level2
            case 6: //Level3
            case 7:
                StopMusic();
                PlayMusic("LevelOne");
                break;
        }
    }

    /*void Awake() {
        if(Instance == null) {
            Instance = this;
        }
        else Destroy(gameObject);

        foreach(Sound s in sounds) {
            s.source = this.gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = false;
        }
    }

    void Start() {
        switch(SceneManager.GetActiveScene().buildIndex)
        {
            case 0: //Main Menu
                Play("TitleMenu");
                Stop("LevelOne");
                break;
            case 1: //Tutorial
            case 2:
            case 4: //Level1
            case 5: //Level2
            case 6: //Level3
            case 7:
                Stop("TitleMenu");
                Play("LevelOne");
                break;
        }
    }

    public void SwitchAudio()
    {
        switch(SceneManager.GetActiveScene().buildIndex)
        {
            case 0: //Main Menu
                Play("TitleMenu");
                Stop("LevelOne");
                break;
            case 1: //Tutorial
            case 2:
            case 4: //Level1
            case 5: //Level2
            case 6: //Level3
            case 7:
                Stop("TitleMenu");
                Play("LevelOne");
                break;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) return;
        if(s.source == null) return;
        s.source.volume = s.volume;
        s.source.Play();
    }

    public void Stop(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null) return;
        s.source.Stop();
    }

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
        AudioClip clip = Array.Find(soundList, sound => sound.name == name);
        if (clip == null || musicSource == null) return;
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic() //Play this first before play music
    {
        if (musicSource != null && musicSource.isPlaying)
            musicSource.Stop();
    }

    public void PlaySFX(string name)
    {
        AudioClip clip = Array.Find(soundList, sound => sound.name == name);
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip);
    }

    
}
