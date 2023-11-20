using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickableBehavior : MonoBehaviour
{
    public Animator anim;
    public bool isdead;
    public int life;
    // Start is called before the first frame update
    void Start()
    {
        isdead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(life<=0)isdead  = true;
    }
    void OnMouseDown() {
        if(anim)anim.SetTrigger("hit");
        life--;
        
    }
    private void OnTriggerStay(Collider other) {
        if(other.tag=="thunder"){
            life--;
        }
    }
    private void OnCollisionEnter(Collision other) {
        if(other.collider.tag=="thunder"){
            life--;
        }
    }
    
}
