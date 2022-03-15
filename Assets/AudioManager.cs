// Samuel Rouillard, Tristan Caetano, Elijah Karpf
// Descend Project
// CIS 464 Project 1

using UnityEngine.Audio;
using UnityEngine;
using System;

// TO ADD A SOUND TO A SPECIFIC ACTION
// FindObjectOfType<AudioManager>().Play("###");
// The ### will be replaced with the title of the audio in the AudioManager.
// AudioManager works like a sound library, that command is calling
// the selected sound to be played.

public class AudioManager : MonoBehaviour {
    public Sound[] sounds;
    public AudioMixerGroup musicMixerGroup;
    public AudioMixerGroup soundEffectsMixerGroup;

    public static AudioManager instance;

    // Start is called before the first frame update
    void Awake() {
        // Making sure the AudioManager doesn't duplicate between scenes.
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        // Options for sound library.
        foreach(Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            switch(s.audioType) {
                case Sound.AudioTypes.soundEffect:
                    s.source.outputAudioMixerGroup = soundEffectsMixerGroup;
                    break;
                
                case Sound.AudioTypes.music:
                    s.source.outputAudioMixerGroup = musicMixerGroup;
                    break;
            }
        }
    }

    // This method is a basic method for static background music.
    void Start() {
        Play("Lonesome");
    }

    // Playing the respective sound.
    public void Play(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null) {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }
}