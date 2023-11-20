using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ragdollBehavior : MonoBehaviour
{
    public int timer;
    public bool isAlive;
    public int i;
    public bool morir=false;
    void Start()
    { 
        i=0;
        if(this.gameObject.tag!="thunder")isAlive=false;
        else{isAlive=true;}
    }
    // Update is called once per frame
    void FixedUpdate()
    {   i++;
        if(i>=timer&&isAlive==true){
            morir=true;
                        Destroy(this.gameObject);
        }

    }
}
