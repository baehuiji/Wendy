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
 
        
    }
}



