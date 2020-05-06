using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardDoor_Open : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    public void play_doorAni()
    {
        animator.SetBool("IsOpening", true);
    }
}
