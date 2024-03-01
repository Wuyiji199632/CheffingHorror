using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public Sound[] sounds;
    public static SoundManager Instance { get; private set; }

    public AudioMixer audioMixer;
    private void Awake()
    {
        if (Instance == null)
        {
            // If no instance exists, this becomes the singleton instance
            Instance = this;
            // Ensure this object persists across scene loads
            DontDestroyOnLoad(gameObject);

            InitializeSounds();
        }
        else if (Instance != this)
        {
            // If an instance already exists and it's not this, destroy this object
            Destroy(gameObject);
        }
    }
    private void InitializeSounds()
    {
        foreach (Sound s in sounds)
        {
            GameObject _go = new GameObject($"Sound_{s.soundName}");
            _go.transform.SetParent(this.transform);
            AudioSource _source = _go.AddComponent<AudioSource>();          
            _source.clip = s.clip;
            _source.outputAudioMixerGroup = s.audioMixerGroup;
            _source.volume = s.volume;
            _source.pitch = s.pitch;
            _source.loop = s.loop;
        }
    }
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.soundName == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        AudioSource _source = GameObject.Find($"Sound_{name}").GetComponent<AudioSource>();
        if (_source != null)
        {
            _source.Play();
        }
    }
}
