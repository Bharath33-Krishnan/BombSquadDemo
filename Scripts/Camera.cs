using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    //offset to camera pos
    public Vector3 offset;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        Vector3 sum = Vector3.zero;


        foreach(Player p in Player.players)
        {
            //finding sum of positions to all Players
            sum += p.transform.position;
        }


        //Dividing it by Player Count to get average position
        sum /= Player.players.Count;

        //set Camera Position to the average position ofsetted by an offset
        transform.position = sum - offset;

        //The Camera will always look at the average Position
        transform.LookAt(sum);
   
    }
}
