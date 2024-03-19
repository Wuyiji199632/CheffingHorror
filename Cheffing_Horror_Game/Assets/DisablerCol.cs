using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablerCol : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hit the player!");
        }
    }
}
