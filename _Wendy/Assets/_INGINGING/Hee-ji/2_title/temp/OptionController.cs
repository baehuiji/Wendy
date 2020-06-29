using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionController : MonoBehaviour
{
    public GameObject _option_window;

    public int stage = 0; //1 : 1스테이지, 2 : 2,3스테이지
    int cursurLock = 0; // 0 : none , 1 : lock
    int camMove = 0; // 0 : none , 1 : active
    int playerMove = 0; // 0 : none , 1 : active
    int actionScript = 0; // 0 : none , 1 : active

    // 1스테이지
    Player_1stage player_script;
    ActionController_01 actionCtrler_script;

    //2스테이지


    void Start()
    {
        // 1스테이지
        if (stage == 1)
        {
            player_script = GameObject.FindObjectOfType<Player_1stage>();
            actionCtrler_script = GameObject.FindObjectOfType<ActionController_01>();
            cursurLock = 0;
        }
        else if (stage == 2)
        {
            cursurLock = 1;
        }
    }

    void Update()
    {

    }

    public void ContinueButton()
    {
        if (stage == 1)
        {
            //player_script.enabled = true;
            //if (actionCtrler_script.enabled == false)
            //    actionCtrler_script.enabled = true;
        }
        else if (stage == 2)
        {

        }

        _option_window.SetActive(false);
    }
    
    public void ESCButton()
    {
        // 마우스 커서 모드 (얻기)
        //var


        // 회생할 조작 스크립트 (얻기)

        if (stage == 1)
        {

        }
        else if (stage == 2)
        {

        }
    }

    void get_use_script()
    {
        player_script.enabled = true;
        if (actionCtrler_script.enabled == false)
        {
            actionCtrler_script.enabled = true;
        }
    }
}
