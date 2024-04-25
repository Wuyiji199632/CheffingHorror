using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaterAlienStateMachine : AlienManager
{

    
    public override void OnStateEnter()
    {
        Debug.Log("Water Alien State Enter");
        this.alienAnim=GetComponent<Animator>();
    }

    public override void OnStateStay()
    {
        Debug.Log("Water Alien State Stay");

        switch (currentState)
        {
            case AlienState.State.Idle:
                Debug.Log("Water Alien is Idle");
                break;
            case AlienState.State.Eat:
                Debug.Log("Water Alien Eats!");
                break;

            case AlienState.State.Attack:
                Debug.Log("Water Alien Attacks!");
                break;
            case AlienState.State.Hurt:
                Debug.Log("Water Alien is Hurt!");
                break;
            case AlienState.State.HurtLong:
                Debug.Log("Water Alien is Hurt Long!");
                break;
            case AlienState.State.HurtQuick:
                Debug.Log("Water Alien is Hurt Quick!");
                break;
            case AlienState.State.Blinded:
                Debug.Log("Water Alien is Blinded!");
                break;
            case AlienState.State.Scared:
                Debug.Log("Water Alien is Scared!");
                break;
            case AlienState.State.Tased:
                Debug.Log("Water Alien is Tased!");
                break;
            case AlienState.State.Dead:
                Debug.Log("Water Alien is Dead!");
                break;

        }
    }

    public override void OnStateExit()
    {
        Debug.Log("Water Alien State Exit");
    }
    // Start is called before the first frame update
    void Start()
    {
        OnStateEnter();
    }

    // Update is called once per frame
    void Update()
    {
        OnStateStay();
    }
    #region Functions for transtions among states

    private void ProcessIdle()
    {

    }

    private void ProcessEat()
    {

    }

    private void ProcessAttack()
    {

    }

    private void ProcessHurt()
    {

    }
    private void ProcessHurtLong()
    {

    }
    private void ProcessHurtQuick()
    {

    }
    private void ProcessBlinded()
    {

    }
    private void ProcessScared()
    {

    }
    private void ProcessTased()
    {

    }
    private void ProcessDead()
    {

    }
    #endregion

}


