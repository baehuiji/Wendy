using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAni_reward : MonoBehaviour
{


    private Animator aniController;
    private bool param = true;

    void Start()
    {
        aniController = GetComponent<Animator>();
    }

    void Update()
    {

    }

    public void set_Ani_param()
    {
        aniController.SetBool("IsRightAnswer", true);
    }

}
