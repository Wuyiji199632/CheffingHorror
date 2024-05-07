using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SoundManager : MonoBehaviour
{
    public Sound[] sounds;
    public static SoundManager Instance { get; private set; }

    public AudioMixer masterAudioMixer;

    public GameObject audioAdjustmentPanel;

    public Slider masterSoundVolumeSlider,musicVolumeSlider,creatureVolumeSlider;

    public AudioSource BGM_AudioSource,BGM_Narrative,BGM_Narrator;

    public AudioSource alienSound;

    public List<AudioClip> clipsPlayedForUI = new List<AudioClip>(); //Clips for indication sound in the UI

    public List<AudioClip> clipsPlayedForScene = new List<AudioClip>(); //Clips for indication sound in the scene during gameplay

    public List<AudioClip> narrativeAudio= new List<AudioClip>(); //Clips for narrative audio

    public List<AudioClip> narratorAudio = new List<AudioClip>(); //Clips for narrator audio

    public List<AudioClip> alienClips = new List<AudioClip>(); //Clips for alien audio

    public AudioSource labSpeakerAudioSource;

    public bool aToEPlayed = false;

    public AudioClip startBGM;
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
        BGM_Narrative=GameObject.Find("Sound_Narrative").GetComponent<AudioSource>();
        BGM_Narrator = GameObject.Find("Sound_Narrator").GetComponent<AudioSource>();
        alienSound = GameObject.Find("Sound_AlienSound").GetComponent<AudioSource>();
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
        if (clipsPlayedForUI.Count > 0)
        {

            GameObject.Find("Sound_Ambient").GetComponent<AudioSource>().clip = clipsPlayedForUI[0];
            AudioSource AmbientSoundSource = GameObject.Find("Sound_Ambient").GetComponent<AudioSource>();
            AmbientSoundSource.Play();
           
        }
    }

    public void PlaySelectionSound()
    {
        if (clipsPlayedForUI.Count > 0)
        {
            AudioSource AmbientSoundSource = null;
            GameObject.Find("Sound_Ambient").GetComponent<AudioSource>().clip = clipsPlayedForUI[1];
            AmbientSoundSource = GameObject.Find("Sound_Ambient").GetComponent<AudioSource>();
            AmbientSoundSource.Play();

        }
    }

    public void PlayGameStartSound()
    {
        if (clipsPlayedForUI.Count > 0)
        {
            AudioSource AmbientSoundSource = null;
            GameObject.Find("Sound_Ambient").GetComponent<AudioSource>().clip = clipsPlayedForUI[2];
            AmbientSoundSource = GameObject.Find("Sound_Ambient").GetComponent<AudioSource>();
            AmbientSoundSource.Play();

        }
    }

    public void ChangeToInGameBGM()
    {
        AudioSource AmbientSoundSource = null;
        BGM_AudioSource.clip = clipsPlayedForUI[3];
        BGM_AudioSource.loop = true;
        AmbientSoundSource = BGM_AudioSource;
        AmbientSoundSource.Play();
    }
    public void RevertToStartBGM()
    {
        AudioSource AmbientSoundSource = null;
        GameObject.Find("Sound_BGM").GetComponent<AudioSource>().clip = startBGM;

        AmbientSoundSource = GameObject.Find("Sound_BGM").GetComponent<AudioSource>();
        AmbientSoundSource.Play();
    }
    public void PlayZappingSound()
    {
        AudioSource AmbientSoundSource = null;
        GameObject.Find("Sound_Ambient").GetComponent<AudioSource>().clip = clipsPlayedForScene[0];
        GameObject.Find("Sound_Ambient").GetComponent<AudioSource>().loop = true;
        AmbientSoundSource = GameObject.Find("Sound_Ambient").GetComponent<AudioSource>();

        AmbientSoundSource.Play();
    }

    public void StopZappingSound()
    {
        AudioSource AmbientSoundSource = null;
        GameObject.Find("Sound_Ambient").GetComponent<AudioSource>().clip = clipsPlayedForScene[0];

        AmbientSoundSource = GameObject.Find("Sound_Ambient").GetComponent<AudioSource>();
        AmbientSoundSource.Stop();
    }

    public void PlayPinchSound()
    {
        AudioSource AmbientSoundSource = null;
        GameObject.Find("Sound_Ambient").GetComponent<AudioSource>().clip = clipsPlayedForScene[1];
        GameObject.Find("Sound_Ambient").GetComponent<AudioSource>().loop = false;
        AmbientSoundSource = GameObject.Find("Sound_Ambient").GetComponent<AudioSource>();

        AmbientSoundSource.Play();
    }
    public void PlaySpraySound()
    {
        AudioSource AmbientSoundSource = null;
        GameObject.Find("Sound_Ambient").GetComponent<AudioSource>().clip = clipsPlayedForScene[2];
        GameObject.Find("Sound_Ambient").GetComponent<AudioSource>().loop = false;
        AmbientSoundSource = GameObject.Find("Sound_Ambient").GetComponent<AudioSource>();

        AmbientSoundSource.Play();
    }
    public void PlayNarrativeSound()
    {
        //AudioSource narrativeAudioSource = null;
        BGM_Narrative.clip = narrativeAudio[0];
        BGM_Narrative = GameObject.Find("Sound_Narrative").GetComponent<AudioSource>();
        BGM_Narrative.Play();
       
    }

    public void PlayNarratorSoundAE()
    {
        //AudioSource narratorAudioSource = null;
        BGM_Narrator.clip = narratorAudio[0];
        BGM_Narrator = GameObject.Find("Sound_Narrator").GetComponent<AudioSource>();
        BGM_Narrator.Play();
    }

    public void PlayNarratorSoundF()
    {
        BGM_Narrator.clip = narratorAudio[1];
        BGM_Narrator = GameObject.Find("Sound_Narrator").GetComponent<AudioSource>();
        BGM_Narrator.Play();
    }

    public void PlayNarratorSoundGH()
    {
        BGM_Narrator.clip = narratorAudio[2];
        BGM_Narrator = GameObject.Find("Sound_Narrator").GetComponent<AudioSource>();
        BGM_Narrator.Play();
    }

    public void PlayNarratorSoundI()
    {
        BGM_Narrator.clip = narratorAudio[3];
        BGM_Narrator = GameObject.Find("Sound_Narrator").GetComponent<AudioSource>();
        BGM_Narrator.Play();
    }
    public void PlayNarratorSoundJ()
    {
        BGM_Narrator.clip = narratorAudio[4];
        BGM_Narrator = GameObject.Find("Sound_Narrator").GetComponent<AudioSource>();
        BGM_Narrator.Play();
    }
    public void StopSound()
    {
        BGM_AudioSource.Stop();
        BGM_Narrative.Stop();
        BGM_Narrator.Stop();
    }
    public void PlayNormalBreathingSound()
    {
        alienSound.clip = alienClips[0];
        alienSound.loop = true;
        alienSound.Play();

    }

    public void PlayUncontentBreathingSound()
    {
        alienSound.clip = alienClips[1];
        alienSound.loop = true;
        alienSound.Play();
    }

    public void PlayAgitatedBreathingSound()
    {
        alienSound.clip = alienClips[2];
        alienSound.loop = true;
        alienSound.Play();
    }

    public void PlayAlienScream()
    {
        alienSound.clip = alienClips[3];
        alienSound.loop = false;
        alienSound.Play();
    }

    public void PlayFacilitySound()
    {
        BGM_AudioSource.clip = clipsPlayedForUI[4];
        BGM_AudioSource.loop = true;
        BGM_AudioSource.Play();
    }

    public IEnumerator WaitForClipToEnd(AudioSource source)
    {
        yield return new WaitForSeconds(source.clip.length);
    }

    private void AddHoverSoundToButton(Button button)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>() ?? button.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((eventData) => { SoundManager.Instance.PlayMouseHoverSound(); });

        trigger.triggers.Add(entry);
    }
}
