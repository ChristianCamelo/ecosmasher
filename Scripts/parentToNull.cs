using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class parentToNull : MonoBehaviour
{
    private RectTransform rect;
    public RectTransform canvas;
    public RectTransform holder;
    // Start is called before the first frame update
    public string powerUp;
    public Animator anim;
    public objBehavior spawner;
    Vector2 mouseP;
    Vector2 ogPos;
            float xR ;
        float yR;
    public bool follow=false;
    void Start()
    {

        rect = this.GetComponent<RectTransform>();
        canvas = GameObject.FindGameObjectWithTag("canvas").GetComponent<RectTransform>();
        spawner = GameObject.FindGameObjectWithTag("objective").GetComponent<objBehavior>();
        ogPos = rect.position;
        //bt.onClick.AddListener(useCard);
       /*bt.onClick.AddListener(() => followClicker(mouseP));*/

    }
    void LateUpdate(){
        xR = rect.position.x;
        yR = rect.position.y;
        mouseP = Input.mousePosition;
        if(follow){
            rect.position=mouseP;
            rect.sizeDelta = new Vector2(200,300);
        }
        else{
            rect.position=ogPos;
            rect.sizeDelta = new Vector2(100,150);
        } 
        pressedStand();
    }

    // Update is called once per frame
    void useCard(){
        spawner.cardUse(powerUp);
    }
    //250 * 450 
    //150 * 250
    private void pressedStand(){
        if(mouseP.x<(xR+rect.rect.width/2)&&
            mouseP.x>(xR-rect.rect.width/2)){
            if(mouseP.y<(yR+rect.rect.height/2)&&
                mouseP.y>(yR-rect.rect.height/2)){
                    if(Input.GetMouseButtonDown(0)){
                        follow = true;
                        Debug.Log(xR);
                        Debug.Log(yR);
                        anim.SetBool("grabbed",true);
                    }
            }
        }
        if(Input.GetMouseButtonUp(0)){
            if(xR<600&&xR>400&&yR<300&&yR>100){
                        Debug.Log("ON USE "+powerUp);
                        useCard();
                        Destroy(this.gameObject);
            }else{
                follow=false;
                anim.SetBool("grabbed",false);
            }
        }

    }
}
