using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardGenerator : MonoBehaviour
{
    private bool onPlay;
    public RectTransform spawnPos;
    public RectTransform holder;
    public int timeToCard;
    public List<GameObject> cardsOnGame;
    public int freed;
    int cont;
    public List<GameObject>cards;
    // Start is called before the first frame update
    void Start()
    {
        cont = 0;
        cardsOnGame = new List<GameObject>(){null,null,null,null};
    }

    // Update is called once per frame
    void LateUpdate()
    {
        onPlay = GameObject.FindGameObjectWithTag("objective").GetComponent<objBehavior>().state;
        if(onPlay==true){
        cont++;
        if(timeToCard<cont){
            for(int i=0;i<4;i++){
                if(cardsOnGame[i]==null){
                    Debug.Log("Position "+i+" is empty");
                    freed=i;
                    break;
                }
                else{
                    freed=-1;
                }
            }
            if(freed!=-1){
                int p = Random.Range(0,cards.Count);
                GameObject a = Instantiate(cards[p],new Vector2((200)+100*freed,-10),Quaternion.identity);
                a.transform.SetParent(holder);
                cardsOnGame[freed]=a;
                cont = 0;
                spawnPos.Translate(new Vector2(100,0)); 
            }

        }
    }

    }
}
