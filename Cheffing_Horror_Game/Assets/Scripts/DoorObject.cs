using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DoorObject : MonoBehaviour
{
    public bool opened=false;

    private AudioSource doorAudio;

    public static DoorObject instance;

    public List<AudioClip> doorAudioClips= new List<AudioClip>();

    //public CameraMovement playerComponent;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        doorAudio=GetComponent<AudioSource>();
        //playerComponent=GameObject.Find("PlayerComponentContainer").GetComponent<CameraMovement>();
    }

    public void PlayDoorSounds()
    {
        doorAudio.clip = opened ? doorAudioClips[0] : doorAudioClips[1];

        doorAudio.Play();

        
    }


}
