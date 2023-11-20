using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class treeBehavior : MonoBehaviour
{
    public GameObject audioSource;
    public AudioClip[] woodSounds;
    public GameObject tx;
    public GameObject spawner;
    public List<GameObject> enemies;
    public Animator anim;
    public GameObject[] renders;
    public float life;
    public bool alive;
    public float tps;

    // Start is called before the first frame update
    void Start()
    {
        
        alive = true;
        audioSource =GameObject.FindGameObjectWithTag("MainCamera"); 
        spawner = GameObject.FindGameObjectWithTag("objective");
        life=spawner.GetComponent<objBehavior>().treeLife;
        tps=spawner.GetComponent<objBehavior>().tps;
        anim = GetComponent<Animator>();
        setRender(renders);
        transform.Rotate(0,Random.Range(0,361),0,Space.Self);
        //Debug.Log("My ubication is"+this.transform.position);
    }
    // Update is called once per frame
    void LateUpdate()
    {
        getDamaged();
        tx.GetComponent<TMPro.TextMeshPro>().text = Mathf.Round(life).ToString();
        if(life<0){
            alive=false;
            spawner.GetComponent<objBehavior>().treeVector.Remove(this.gameObject);

        }
    }
    public void dieTree(){
        Destroy(this.gameObject);
    }
    public void addEnemy(GameObject obj){
        enemies.Add(obj);
        if(obj.tag == "dozer"){
            life = 0;
        }
    }
    public Vector3 getUbication(){
        return this.transform.position;
    }
    public void getDamaged(){
        life-=tps*(int)enemies.Count*Time.deltaTime;
        animationPlayer();
    }
    public void setRender(GameObject[]v){
        for(int i=0;i<v.Length;i++){
            v[i].SetActive(false);
        }
        int p = Random.Range(0,v.Length);
        //Debug.Log("Rednering "+p);
        float size=Random.Range(1f,1.5f);
        v[p].GetComponent<Transform>().localScale = new Vector3(size,size,size);
        v[p].SetActive(true);
    }
    public void animationPlayer(){
        if(enemies.Count>0){
            anim.SetBool("gettingHit",true); 
            anim.speed = 0.7f+(enemies.Count/10);
        }
        else{
            anim.SetBool("gettingHit",false);
            anim.speed = 1;
        }
        if(alive==false){
            anim.SetBool("dead",true);
        }
    }
    public void getHitSound(){
        int i = Random.Range(0,woodSounds.Length);
        audioSource.GetComponent<AudioSource>().PlayOneShot(woodSounds[i],1);
    }

}
