using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Sound", menuName = "Audio/Sound")]
public class Sound : ScriptableObject
{
    public string soundName;
    public AudioClip clip;
    public AudioMixerGroup audioMixerGroup;
    [Range(0f, 1f)]
    public float volume = 0.7f;
    [Range(.1f, 3f)]
    public float pitch = 1f;

    public bool loop = false;
}
