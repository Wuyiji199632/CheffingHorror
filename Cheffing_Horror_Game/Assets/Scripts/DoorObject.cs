using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorObject : MonoBehaviour
{
   public bool opened=false;

    public static DoorObject instance;
    private void Awake()
    {
        instance = this;
    }

    
}
