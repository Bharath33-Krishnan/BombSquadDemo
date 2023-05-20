using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Unity Works on a Component Based System and Wrapping your head arround Components is necessary
 * https://docs.unity3d.com/Manual/Components.html
 * */
public class Player : MonoBehaviour
{
    public static List<Player> players = new List<Player>();//Initialise a list to store all active players

    [Header("Player Properties")]
    [SerializeField]float moveSpeed;
    [SerializeField] float rotSpeed;
    public GameObject BombPrefab;//Predefined Bomb Object

    [SerializeField] float ThrowForce = 20f;

    public GameObject currentObj { get; private set; }//If Player has an Bomb in his hands scalable to any object


    Rigidbody rb;

    private void OnEnable()
    {
        players.Add(this);//Add Player to list on enabling this script
    }
    private void OnDisable()
    {
        players.Remove(this);//Remove Player from list on Disabling this script
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();//Get the Rigidbody Component From Player Object
        //https://docs.unity.cn/2021.2/Documentation/ScriptReference/Rigidbody.html
    }

    // Update is called once per frame
    void Update()
    {
        //Game Loop
        
    }

    private void FixedUpdate()
    {
        //Physics Based Game Loop


        MovePlayer();//Handles Player Movement


        GetBomb();//Handles Bomb Throw and equip
    }

    void MovePlayer()
    {
        //Getting Input using old Input System
        Vector2 moveVec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        //Moves Position of Rigidbody to given Vector3
        rb.MovePosition(transform.position+transform.forward * moveVec.y * moveSpeed*Time.deltaTime);
        //Sets Angular Velocity Vector for Player Rotation 
        rb.angularVelocity = new Vector3(0, moveVec.x, 0) * rotSpeed;

    }

    void GetBomb()
    {
        if(BombPrefab == null)
            return;

        
        if (Input.GetAxisRaw("Fire2") != 0 && currentObj == null)
        {
            //On right Click and No bomb is in Hand Currently Equip a Bomb 
            currentObj = Instantiate(BombPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
            
            
            //The Equipped Bomb Should not be influenced by gravity
            currentObj.GetComponent<Rigidbody>().isKinematic = true;
        }
        else if(currentObj != null)
        {
            //if a Object is Equipped It tracks Players Position with an Offset in Y Axis
            currentObj.GetComponent<Rigidbody>().MovePosition(transform.position + Vector3.up * 2);

            
            if (Input.GetAxisRaw("Fire1") != 0 && currentObj != null)
            {
                //On Clicking Right Mouse Button throw the object
                currentObj.GetComponent<Rigidbody>().isKinematic = false;

                //Physics to Calculate Throw Force
                currentObj.GetComponent<Rigidbody>().AddForce((transform.forward + transform.up).normalized * (ThrowForce+rb.velocity.magnitude*100));
                //transform.up gives a vector pointing in upwards direction relative to the object to which
                //this script is attatched
                

                //Once Object is Thrown it should be set to null 
                currentObj = null;
            }
        }
        
    }

    public void ResetBomb()
    {
        //Reset currentObj to null
        currentObj = null;
    }


}
