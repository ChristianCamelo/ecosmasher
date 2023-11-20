using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class truckBehavior : MonoBehaviour
{
    public GameObject audioSource;
    public GameObject clicker;
    public AudioClip []deadClips;
    public GameObject Spawner;
    private List<GameObject> treesV;
    public GameObject ragdoll;
    public GameObject model;
    public Vector3 destinyPos;
    public Quaternion rotActual;
    public Animator animator;
    public GameObject tree;
    public Vector3 spawnPos;
    private NavMeshAgent agent;
    public GameObject[]weapons;
    public GameObject weapon;
    public bool hasGoal = false;
    public bool attack;
    public int v;
    // Start is called before the first frame update
    private void Start (){
        //-----LEER CONTEXTO-----
        spawnPos = this.transform.position;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource =GameObject.FindGameObjectWithTag("MainCamera"); 
        Spawner = GameObject.FindGameObjectWithTag("objective");
        //-----ENCONTRAR OBJETIVO-----

    }    
    private void Update(){
        v = Mathf.RoundToInt(Mathf.Sqrt(Mathf.Pow(agent.velocity.x,2)+Mathf.Pow(agent.velocity.x,2)));
        
        //-----ANIMATOR-----
        if(v>0)
            animator.SetBool("attack",false);
        if(v<=0)
            animator.SetBool("attack",true);
        if(clicker.GetComponent<clickableBehavior>().isdead==true){
            deadEnemy();
        }
    }
    private Vector3 setDestination(){
        Vector3 target = new Vector3(0,0,0)*-1;
        return target;
    }
    public void deadEnemy(){//------MORIR------
        //--------------SOLTAR ARMA----------------
        if(weapon){
            weapon.GetComponent<ragdollBehavior>().isAlive = true;
            weapon.transform.SetParent(null);
            weapon.GetComponent<Rigidbody>().isKinematic = false;
        }
        Quaternion rot = transform.rotation;
        //--------------REMOVER DE LOS ENEMIGOS LEIDOS DEL ARBOL------------
        if(tree)
        tree.GetComponent<treeBehavior>().enemies.Remove(this.gameObject);
        if(Spawner)
        Spawner.GetComponent<objBehavior>().enemyVector.Remove(this.gameObject);
        animator.SetBool("isDead",true);
        //Destroy(this.gameObject);
    }
    protected void destroyThis(){
        int i = Random.Range(0,deadClips.Length);
        audioSource.GetComponent<AudioSource>().PlayOneShot(deadClips[i],1);
        Destroy(this.gameObject);
        
    }
}
