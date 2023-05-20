using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    ParticleSystem expl;
    //The Particle System that plays on explosion

    public float AOE = 5;//The Area of Effect of Bomb
    public bool Exploded = false;
    
    [SerializeField]int PushForce = 300;

    [SerializeField] float expTime = 4f;//Time Before Explosion
    float ActiveTime = 0;//Time in which script got activated 

    SphereCollider col;//The Collider of the bomb

    void Start()
    {
        expl = GetComponentInChildren<ParticleSystem>();//Geting the Particle System from the Child refer inspector
        expl.Stop();//Stop the particle System if its playing
        ActiveTime = Time.timeSinceLevelLoad;//Get the time in which script is activated kindoff :-)
        col = GetComponentInChildren<SphereCollider>();//get the sphere collider attatched to the object
    }


    // Update is called once per frame
    void Update()
    {
        if(Time.timeSinceLevelLoad - ActiveTime > expTime)
        {
            //if The time since the object activated is grater than explosion time explode the bomb
            Explode();//Handles Explosion Logic
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GetComponent<Rigidbody>().velocity+= -transform.up*GetComponent<Rigidbody>().velocity.y/3;
        //Not So important code
    }

    public void Explode()
    {
        if (!Exploded)
        {
            //if Explode function is called and Exploded is set to false
            //That is Bomb has not exploded yet
            //Then Play the Particle  System
            expl.Play();
            foreach(Player p in Player.players)
            {
                //Checks if anyu player is within the explosion Radius
                //if yes a PushBack Force is Applied to them

                //Con : Explosion effect is felt through walls
                //Hasn't Implemented Player Life System 
                if ((p.transform.position - transform.position).sqrMagnitude <= AOE * AOE)
                {
                    p.GetComponent<Rigidbody>().AddForce((p.transform.position - transform.position).normalized * PushForce);
                }
                if(p.currentObj == gameObject)
                {
                    //If Bomb Explodes in Hand of a Player we Need to reset the equip status of bomb
                    //that is once exploded the player is not holding the bomb anymore
                    p.ResetBomb();
                }
            }
            GetComponent<MeshRenderer>().enabled = false;//The Bomb Disappears
            Exploded = true;//Set Exploded to true so that these functionalities will be called only once per Explosion
        }
      
        //Inbuilt function to destroy a gameobject after n secs specified
        Destroy(gameObject,1f);
    }

   
}
