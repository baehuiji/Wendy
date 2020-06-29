using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRange : MonoBehaviour
{
    public WendyAI _wendy_ai;
    private bool once = true; // 최초 접촉 검사

    public GameObject basement_door_ring;
    public GameObject stair_ring;

    public GameObject temp_stair; //2층

    void Start()
    {
        //_wendy_ai = GameObject.FindObjectOfType<WendyAI>;
    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wendy")
        {
            if (!once)
            {
                _wendy_ai.CollideWithPlayer();
            }
            else
            {
                once = false;

                // 원형 이펙트
                basement_door_ring.SetActive(false);
                stair_ring.SetActive(true);

                // 2층가는 길목 콜라이더 없애기
                temp_stair.SetActive(false);
            }
        }
    }
}
