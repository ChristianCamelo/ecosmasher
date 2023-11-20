using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class objBehavior : MonoBehaviour
{
    public Material virusMat;
    public Material normalMat;
    public InputField[] inputs;
    public bool state = false;
    public int treeLimit;
    public int enemyLimit;
    //---------------OBJS SPAWNEADOS-----------
    public List<GameObject> treeVector;
    public List<GameObject> enemyVector;
    public List<GameObject> dozerVector;
    //---------------OBJS A SPAWNEAR-----------
    public GameObject tree;
    public GameObject malaria;
    public GameObject thunder;
    public GameObject[]props;
    public AudioClip loseSound;
    public GameObject enemy;
    public GameObject dozer;
    public GameObject truck;
    //---------------STATS DE LOS ENEMIGOS--------
    public int enemyVel;
    public int radiusZoneMx,radiusZoneMn,radiusNpc;
    //---------------TIEMPO DE SPAWN------------
    public int timeB; //spawn arboles
    public int timeA; //spawn enemigo
    public float checkDis;
    public float angle;
    public float lifePts;
    public Text life;
    public int contC;
    public int contB;
    public int contA;
    public float tps;
    public int treeLife;

    public GameObject cam;

    public GameObject lose;
    // Start is called before the first frame update
    void Start(){
        lose.SetActive(false);
        bool grid = generateGrid();
        bool enviroment = generateEnviroment(props);
        treeVector = new List<GameObject>();
        //-----SPAWNER INICIAL-----
        int a = spawnEntity(tree,radiusZoneMx,radiusZoneMn,1,0); 
        
    }
    // Update is called once per frame
    void LateUpdate(){
        life.text = "Tree Amount" + treeVector.Count.ToString();
        if(state==true){
            if(enemyVector.Count<=enemyLimit){
                int p = Random.Range(0,5);//Campos de probabilidad
                    if(p<5)contA = spawnEntity(enemy,radiusNpc,radiusNpc,contA,timeA);
                    if(dozerVector.Count<4){
                    if(p==4)contA = spawnEntity(dozer,radiusNpc,radiusNpc,contA,timeA);
                    }
                    if(p==5)contA = spawnEntity(truck,radiusNpc,radiusNpc,contA,timeA);
                }
            if(treeVector.Count<=treeLimit)contB = spawnEntity(tree,radiusZoneMx,radiusZoneMn,contB,timeB); 
        }
        if(treeVector.Count<=0){
            if(state)cam.GetComponent<AudioSource>().PlayOneShot(loseSound,1);
            state=false;
            lose.SetActive(true);
            
        }
    }


    public int spawnEntity(GameObject obj,int radiusMx, int radiusMn,int cont,int timeMx){
        //Recibe un contador y lo devuelve en tiempo real, si supera el tiempo aparece un obj en un radio sobre el centro
       cont++;
       int timeTMP = (-10*treeVector.Count)+timeMx;
        Vector3 spawnPos = new Vector3();
        //-----BUSCAR LUGAR-----
        spawnPos = spawnPlace(radiusMn,radiusMx);
        //-----TIEMPO CUMPLIDO-----
            if(cont>timeTMP){
                GameObject newObj = Instantiate(obj,spawnPos,obj.transform.rotation);
                if(newObj.tag == "tree"){
                    treeVector.Add(newObj);
                }
                if(newObj.tag == "enemy"){
                    enemyVector.Add(newObj);
                }
                if(newObj.tag =="dozer"){
                    dozerVector.Add(newObj);
                }
                cont = 0; 
            }
        return cont;
    }
    public Vector3 spawnPlace(int radiusMn,int radiusMx){
        Vector3 ret;
        int radius = Random.Range(radiusMn,radiusMx);
        float angle = Random.Range(0,360);
        float x = radius*(Mathf.Sin(angle));
        float z = radius*(Mathf.Cos(angle));
        ret = new Vector3(x,0,z);
        if(isFree(ret)==true){
            return ret;
        }
        else{
            return spawnPlace(radiusMn,radiusMx);
        }
    }
    public bool isFree(Vector3 place){
        bool a=true;
        return a;
    }
    
    IEnumerator setSpeed(int red){
        enemyVel-=red;
        yield return new WaitForSeconds(5);
        enemyVel+=red;
        StopAllCoroutines();
    }
    IEnumerator setTps(int red){
        tps=0;
        enemyVel-=red;
        yield return new WaitForSeconds(10);
        tps=1;
        enemyVel+=red;
        StopAllCoroutines();
    }
    
    public void cardUse(string s){
        switch(s){
            case "virus": for(int i =0;i<enemyVector.Count;i++){
                        int p = Random.Range(0,2);
                        if(p==1){
                            enemyVector[i].GetComponentInChildren<SkinnedMeshRenderer>().material=virusMat;
                            enemyVector[i].GetComponent<navMeshAgent>().infected=true;
                            }
                        }       
                    break;
            case "hambre": StartCoroutine(setTps(3));
                    break;
            case "calor": StartCoroutine(setSpeed(3));
                    break;
            case "malaria": for(int f=0;f<1;f++){
                                Instantiate(malaria,spawnPlace(radiusZoneMn,radiusZoneMn),Quaternion.identity);
                            }  
                    break;
            case "thunder": for(int f=0;f<3;f++){
                                Instantiate(thunder,spawnPlace(radiusZoneMn,radiusZoneMn*2),Quaternion.identity);
                            }  
                    break;
            case "nuke":  GameObject[]enemies = GameObject.FindGameObjectsWithTag("enemy"); 
                            for(int i=0;i<enemies.Length;i++){
                                enemies[i].GetComponent<navMeshAgent>().deadEnemy();
                            }
                    break;
        }
    }

    public bool generateGrid() {
        //float r2=Mathf.Sqrt((Mathf .Pow(radiusZoneMn,2))+(Mathf.Pow(radiusZoneMn,2)));
        int r=radiusZoneMn;
        for(int i=-1*r;i<r;i++){
            for(int j=r;j>-r;j--){
                Vector3 pos = new Vector3(i,0,j);
                if(Vector3.Distance(pos,this.transform.position)<=r){
                    Debug.DrawLine(pos,new Vector3(i,3,j),Color.red,10);
                }
            }
        }
        return true;
    }

    public bool generateEnviroment(GameObject []obj){
        int r=radiusZoneMn*3;
        for(int i=-1*r;i<r;i++){
            for(int j=r;j>-r;j--){
                Vector3 pos = new Vector3(i,0,j);
                float posToCenter = Vector3.Distance(Vector3.zero,pos);
                //Debug.Log(i+" "+j+" to center diference is "+posToCenter);
                float spawnProb=Random.Range(0,posToCenter);
                if(spawnProb<1.5f){
                   int pickPos = Random.Range(0,obj.Length);
                   int ranRot = Random.Range(0,360)*Random.Range(-1,1);
                   GameObject pickedObj = obj[pickPos];
                   Quaternion rot = new Quaternion(Quaternion.identity.x,ranRot,Quaternion.identity.z,Quaternion.identity.w);
                   Instantiate(pickedObj, pos,rot); 
                }
                //if(Vector3.Distance(pos,this.transform.position)){
                //}
            }
        }return true;
    }

    public void startGame(){
        /*int[] info=new int[4];
        for(int i =0;i<4;i++){
            info[i] = int.Parse(inputs[i].text.ToString());
        }
        */
        enemyLimit=15;
        timeA=500;//time enemies
        treeLimit=10;
        timeB=2000;//time tree
        state = true;
        for(int j =0;j<4;j++){
            inputs[j].gameObject.SetActive(false);
        }
    }
    public void resetGame(){
               SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
