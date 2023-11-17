using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    // Audio players components.
    public AudioSource EffectsSource;
    public AudioSource MusicSource;

    // Random pitch adjustment range.
    public float LowPitchRange = .95f;
    public float HighPitchRange = 1.05f;

    // Singleton instance.
    public static SoundManager Instance = null;
    private void Awake() 
    {
        if (Instance != null && Instance != this) { Destroy(this); } 
        else { Instance = this;} 
      //  DontDestroyOnLoad (gameObject);
    }
   
    // Play a single clip through the sound effects source.
    public void Play(AudioClip clip)
    {
        Debug.Log("2");
        EffectsSource.clip = clip;
        EffectsSource.Play();
    }
    // Play a single clip through the music source.
    public void PlayMusic(AudioClip clip)
    {
        MusicSource.clip = clip;
        MusicSource.Play();
    }
    
    public void AudioChanged(Slider audioSlide) { EffectsSource.volume = audioSlide.value; }
    public void SFXChanged(Slider audioSlide) { MusicSource.volume = audioSlide.value; }
    
    

}