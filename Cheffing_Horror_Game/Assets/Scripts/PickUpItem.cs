using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    private Rigidbody rb;

    private float forwardSpeed = 0;

    [SerializeField]
    private CameraMovement player;

    public bool thrown = false;

    public LayerMask groundLayer;
    private void Start()
    {
        rb = GetComponent<Rigidbody>(); player = GameObject.FindGameObjectWithTag("Player").GetComponent<CameraMovement>();
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



}
