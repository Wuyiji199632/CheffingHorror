using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienManager : MonoBehaviour
{

    public Animator alienAnim;

    public AlienState.State currentState;

   public virtual void OnStateEnter() { }

   public virtual void OnStateStay() { }

   public virtual void OnStateExit() { }


    public class AlienState
    {
        public enum State { Idle,Eat,Attack,Hurt,HurtLong,HurtQuick,Blinded,Scared,Tased,Dead}
    }
}
