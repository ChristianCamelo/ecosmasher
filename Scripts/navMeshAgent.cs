using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class navMeshAgent : MonoBehaviour
{
    public bool infected=false;
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
        //agent.speed = Spawner.GetComponent<objBehavior>().enemyVel;
        if(weapons.Length!=0)
            weapon = setWeapon(weapons);
        //-----ENCONTRAR OBJETIVO-----
        treesV = new List<GameObject>();//vector de posiciones vacio
        treesV = Spawner.GetComponent<objBehavior>().treeVector;//vector asignado
        tree = findNewTree(tree);
    }    
    private void Update(){
        agent.speed = Spawner.GetComponent<objBehavior>().enemyVel;
        v = Mathf.RoundToInt(Mathf.Sqrt(Mathf.Pow(agent.velocity.x,2)+Mathf.Pow(agent.velocity.x,2)));
        //-----ATACA ARBOL-----
        if(hasGoal==true){
            if(tree!=null)//aseguramos que hay arbol
                DealDamage(tree);//ataca el arbol
        }
        //-----ARBOL INEXISTENTE-----
        if(tree==null){
            hasGoal=false;
            tree=findNewTree(tree);
            if(tree==this.gameObject){
                tree=null;
            }
        }
        //-----ANIMATOR-----
        if(v>0)
            animator.SetBool("attack",false);
        if(v<=0)
            animator.SetBool("attack",true);
        if(clicker.GetComponent<clickableBehavior>().isdead==true){
            deadEnemy();
        }
        if(infected)StartCoroutine(waitToDie());
    }
    IEnumerator waitToDie(){
        yield return new WaitForSeconds(3f);
        deadEnemy();
    }
    private GameObject setDestination(List<GameObject>trees){
        float distanceMx=100000f;
        GameObject des=this.gameObject;
        //Debug.Log("i read "+(int)trees.Count+" trees");
        for(int i=0;i<(int)trees.Count;i++){
            if(trees[i]!=null){
                Vector3 treePos = trees[i].GetComponentInChildren<treeBehavior>().getUbication();
                float dis = Vector3.Distance(spawnPos,treePos);
                if(dis<distanceMx){
                    distanceMx=dis;
                    des = trees[i];
                }
            }
        }return des;
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
        Spawner.GetComponent<objBehavior>().dozerVector.Remove(this.gameObject);
        animator.SetBool("isDead",true);
        //Destroy(this.gameObject);
    }
    private void DealDamage(GameObject tree){
        //------------------DISTANCIA A OBJETIVO-----------------------
        if(Vector3.Distance(transform.position,agent.destination)<2f){
            agent.GetComponent<NavMeshAgent>().isStopped=true;//detenerse
            if(tree!=this.gameObject)
                tree.GetComponent<treeBehavior>().addEnemy(this.gameObject);//arbol lee enemigo
            hasGoal=false;
        }
    }
    private GameObject findNewTree(GameObject tree){
        agent.GetComponent<NavMeshAgent>().isStopped=false;
        while(hasGoal==false){
            tree=setDestination(treesV);
            agent.destination=tree.GetComponent<Transform>().position;
            hasGoal=true;
        }return tree;
    }
    public GameObject setWeapon(GameObject[]w){
        GameObject picked;
        for(int i=0;i<w.Length;i++){
            w[i].SetActive(false);
        }
        picked = w[Random.Range(0,w.Length)];
        picked.GetComponent<Rigidbody>().isKinematic = true;
        picked.SetActive(true);
        return picked;
    }
    protected void destroyThis(){
        int i = Random.Range(0,deadClips.Length);
        audioSource.GetComponent<AudioSource>().PlayOneShot(deadClips[i],1);
        Destroy(this.gameObject);
        
    }
}
