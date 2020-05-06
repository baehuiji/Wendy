using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampLight : MonoBehaviour
{

    private GameObject target;


    private bool state;

    public int LampNum; // 램프 순서 


    ColliderMgr colliderManager;


    void Start()
    {
        colliderManager = FindObjectOfType<ColliderMgr>();
    }

    public void SetLight(int LampNum)
    {
   //print("마우스 입력 받았음");
                //   if (state == true)
                // {

        /*
                if (_Pointstate[0].activeInHierarchy == true)
                {
                    _Pointstate[0].gameObject.SetActive(false);

                }
                else
                {
                    _Pointstate[0].gameObject.SetActive(true);
                }


                if (_Pointstate[1].activeInHierarchy == true)
                {
                    _Pointstate[1].gameObject.SetActive(false);
                }
                else
                {
                    _Pointstate[1].gameObject.SetActive(true);
                }


                if (_Pointstate[2].activeInHierarchy == true)
                {
                    _Pointstate[2].gameObject.SetActive(false);
                }
                else
                {
                    _Pointstate[2].gameObject.SetActive(true);
                }
                */

        //Debug.Log(LampNum);
        
    }
}



