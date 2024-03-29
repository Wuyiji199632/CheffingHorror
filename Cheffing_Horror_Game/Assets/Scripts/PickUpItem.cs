using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PickUpItem : MonoBehaviour
{
    private Rigidbody rb;

    private float forwardSpeed = 0;

    [SerializeField]
    private CameraMovement player;

    public bool thrown = false;

    public LayerMask groundLayer;

    private AudioSource pickupAudio;

    public List<AudioClip> pickupAudioClips;

    public bool itemFunctionOn = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody>(); player = GameObject.FindGameObjectWithTag("Player").GetComponent<CameraMovement>();

        pickupAudio=GetComponent<AudioSource>();
    }
    private void LateUpdate()
    {
        if (this.name == "Rubber_Duck")
        {
            forwardSpeed=thrown ? 10 : 0.0f;
            Vector3 direction = transform.position - player.transform.position;
            if (thrown)
            {               
                rb.isKinematic = false;
               
            }

            rb.velocity = rb.velocity+ direction*forwardSpeed*Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other != null)
        {
            thrown = false;
        }
    }

   
    public void PlayTorchSounds()
    {
        if(gameObject.name== "Torch")
        {
            pickupAudio.clip = itemFunctionOn ? pickupAudioClips[0] : pickupAudioClips[1]; pickupAudio.Play();
        }
    }



}
