using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public Sound[] sounds;
    public static SoundManager Instance { get; private set; }

    public AudioMixer masterAudioMixer;

    public GameObject audioAdjustmentPanel;

    public Slider masterSoundVolumeSlider,musicVolumeSlider,creatureVolumeSlider;

    public AudioSource BGM_AudioSource;

    public List<AudioClip> clips = new List<AudioClip>(); //Clips for indication sound in the UI

    [Header("In-game Pause Menu")]
    public Button SettingsBtn, MainMenuBtn, QuitBtn;
    private void Awake()
    {
        if (Instance == null)
        {
            // If no instance exists, this becomes the singleton instance
            Instance = this;
            // Ensure this object persists across scene loads
            DontDestroyOnLoad(this);

            InitializeSounds();
        }
        else if (Instance != this)
        {
            // If an instance already exists and it's not this, destroy this object
            Destroy(gameObject);
        }
    }
    private void Start()
    {
       
        BGM_AudioSource=GameObject.Find("Sound_BGM").GetComponent<AudioSource>();

       
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
    IEnumerator InitializeUIElementsWhenReady()
    {
        yield return new WaitUntil(() => audioAdjustmentPanel.activeInHierarchy);

        if (audioAdjustmentPanel.activeInHierarchy)
        {
            masterSoundVolumeSlider = GameObject.FindGameObjectWithTag("MasterVolume").GetComponent<Slider>();
            musicVolumeSlider = GameObject.FindGameObjectWithTag("MusicVolume").GetComponent<Slider>();
            creatureVolumeSlider = GameObject.FindGameObjectWithTag("CreatureVolume").GetComponent<Slider>();
        }
   

        // Setup the sliders' initial values and callbacks here
    }
  
    public void ShowAudioAdjustmentPanel(bool audioPageOpened)
    {
        audioAdjustmentPanel.SetActive(audioPageOpened);
        StartCoroutine(InitializeUIElementsWhenReady());
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

    public void SetMasterVolume(float sliderValue)
    {
        sliderValue = masterSoundVolumeSlider.value;

        if (sliderValue <= 0)
        {
            masterAudioMixer.SetFloat("MasterVolume", -80f); // Use a value like -80 dB for silence.
        }
        else
        {
            float volumeDb = Mathf.Log10(sliderValue) * 20;
            masterAudioMixer.SetFloat("MasterVolume", volumeDb);
        }
    }

    public void SetMusicVolume(float sliderValue)
    {

        sliderValue = musicVolumeSlider.value;

        if (sliderValue <= 0)
        {
            masterAudioMixer.SetFloat("BGMVolume", -80f); // Use a value like -80 dB for silence.
        }
        else
        {
            float volumeDb = Mathf.Log10(sliderValue) * 20;
            masterAudioMixer.SetFloat("BGMVolume", volumeDb);
        }
    }


    public void PlayMouseHoverSound()
    {
        if (clips.Count > 0)
        {

            GameObject.Find("Sound_Ambient").GetComponent<AudioSource>().clip = clips[0];
            AudioSource AmbientSoundSource = GameObject.Find("Sound_Ambient").GetComponent<AudioSource>();
            AmbientSoundSource.Play();
           
        }
    }

    public void PlaySelectionSound()
    {
        if (clips.Count > 0)
        {

            GameObject.Find("Sound_Ambient").GetComponent<AudioSource>().clip = clips[1];
            AudioSource AmbientSoundSource = GameObject.Find("Sound_Ambient").GetComponent<AudioSource>();
            AmbientSoundSource.Play();

        }
    }
}
