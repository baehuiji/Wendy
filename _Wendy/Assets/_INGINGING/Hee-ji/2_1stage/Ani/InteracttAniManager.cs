using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteracttAniManager : MonoBehaviour
{
    //Reward Manager

    public GameObject[] _reward = new GameObject[5]; //퍼즐조각 5게


    void Start()
    {      
        // - 셔플
        // +
        
        // - 배치
        // +

        // - 비활성화
        for(int i =0; i < _reward.Length; ++i)
        {
            _reward[i].SetActive(false);
        }
    }

    void Update()
    {
        
    }

    public void Active_Piece(int index)
    {
        _reward[index].SetActive(true);
    }
}
