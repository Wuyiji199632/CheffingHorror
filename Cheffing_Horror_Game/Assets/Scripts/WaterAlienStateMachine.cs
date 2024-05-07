using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaterAlienStateMachine : AlienManager
{
    

    //[SerializeField]
    //private bool isHitByLight=false;
    
    public override void OnStateEnter()
    {
        Debug.Log("Water Alien State Enter");
        this.alienAnim=GetComponent<Animator>();
    }

    public override void OnStateStay()
    {
       
       
        LockOnPlayer();
        Debug.Log("Water Alien State Stay");

        switch (currentState)
        {
            case AlienState.State.Idle:
                Debug.Log("Water Alien is Idle");
                ProcessIdle();
                break;
            case AlienState.State.Eat:
                Debug.Log("Water Alien Eats!");
                ProcessEat();
                break;

            case AlienState.State.Attack:
                Debug.Log("Water Alien Attacks!");
                ProcessAttack();
                break;
            case AlienState.State.Hurt:
                Debug.Log("Water Alien is Hurt!");
                ProcessHurt();
                break;
          
            case AlienState.State.Blinded:
                Debug.Log("Water Alien is Blinded!");
                ProcessBlinded();
                break;
            case AlienState.State.Scared:
                Debug.Log("Water Alien is Scared!");
                ProcessScared();
                break;
            case AlienState.State.Tased:
                Debug.Log("Water Alien is Tased!");
                ProcessTased();
                break;
            case AlienState.State.Dead:
                Debug.Log("Water Alien is Dead!");
                ProcessDead();
                break;

        }
    }

    private void LockOnPlayer()
    {
        if(currentState!=AlienState.State.Dead)
        {
            Vector3 dirToPlayer = playerComponent.transform.position - transform.position;

            dirToPlayer.y = 0;

            float distanceToPlayer = dirToPlayer.magnitude;

            Vector3 targetPos = playerComponent.transform.position;

            // Check if direction is not zero
            if (distanceToPlayer >= 4)
            {
                // Create a rotation looking at the player
                Quaternion lookRotation = Quaternion.LookRotation(dirToPlayer.normalized);

                // Optionally, smooth the rotation transition
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10);


            }
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
        if (WorldManager.Instance.paused) { return; }
        OnStateStay();
    }
    #region Functions for transtions among states

    private void ProcessIdle()
    {
        alienAnim.SetTrigger("Idle");

        SoundManager.Instance.PlayNormalBreathingSound();
    }

    private void ProcessEat()
    {

    }

    private void ProcessAttack()
    {
        //alienAnim.SetTrigger("Attack");

        Vector3 dirToPlayer = playerComponent.transform.position - transform.position;

        dirToPlayer.y = 0;

        float distanceToPlayer = dirToPlayer.magnitude;

        Vector3 targetPos = playerComponent.transform.position;

        // Check if direction is not zero
        if (distanceToPlayer>=4)
        {
            // Create a rotation looking at the player
            Quaternion lookRotation = Quaternion.LookRotation(dirToPlayer.normalized);

            // Optionally, smooth the rotation transition
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10);

            transform.Translate(Vector3.forward * Time.deltaTime * 10);

            
            // Trigger the attack animation
            alienAnim.SetTrigger("Attack");

            SoundManager.Instance.PlayAgitatedBreathingSound();

          
        }

        //StartCoroutine(JumpForwardToPlayer());

    }
    private void ProcessHurt()
    {
        alienAnim.SetTrigger("HurtQuick");
        hurt = true;
        alienAnim.SetBool("BeHurt", hurt);
    }

    private void ProcessBlinded()
    {
       

        if(!playerComponent.torchCol.enabled)
            currentState = AlienState.State.Idle;

        alienAnim.SetTrigger("Blinded");
        SoundManager.Instance.PlayUncontentBreathingSound();

      
    }
    private void ProcessScared()
    {
        alienAnim.SetTrigger("Scared");
    }
    private void ProcessTased()
    {

        alienAnim.SetTrigger("Tased");

        if (!playerComponent.taserCol.enabled)
            currentState = AlienState.State.Idle;
        SoundManager.Instance.PlayAgitatedBreathingSound();
    }
    private void ProcessDead()
    {
        if (hurt)
        {
            currentState = AlienState.State.Dead;
            alienAnim.SetTrigger("Die");
        }
        else
        {
            currentState = AlienState.State.Tased;
        }
       
    }
    #endregion

    public void ResetState()
    {
        currentState = AlienState.State.Idle;
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (WorldManager.Instance.paused) { return; }
        if (other.gameObject.GetComponent<LightCollider>() != null&&playerComponent.torchCol.enabled)
        {
            Debug.Log("Water Alien is hit by light");
           
            currentState = AlienState.State.Blinded;

                                
        }

        if (other.gameObject.name == "Taser")
        {
            Debug.Log("Alien hit by taser");
            currentState = AlienState.State.Tased;
        }

        if(other.gameObject.name=="Spray_Bottle")
        {
            Debug.Log("Alien is hit by spray bottle");
           currentState = AlienState.State.Hurt;
        }

       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Rubber_Duck")
        {
            currentState = AlienState.State.Scared;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (WorldManager.Instance.paused) { return; }
        if (other.gameObject.GetComponent<LightCollider>() != null)
        {
            Debug.Log("Water Alien is hit by light");

            currentState = AlienState.State.Idle;


        }

        //if (other.gameObject.name == "Taser")
        //{
        //    Debug.Log("Alien stops being hit by taser");
        //    currentState = AlienState.State.Idle;
        //}
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Rubber_Duck")
        {
            ResetState();
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.GetComponent<LightCollider>() != null)
    //    {
    //        Debug.Log("Water Alien is no longer hit by light");
    //        isHitByLight = false;
    //    }
    //}


}


