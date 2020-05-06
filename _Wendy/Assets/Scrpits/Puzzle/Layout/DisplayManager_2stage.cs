using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DisplayManager_2stage : MonoBehaviour
{
    private int[] input_Arry = new int[8]; //장식장에 놓여진 인형들의 아이템 코드들
    private GameObject[] input_DollArry = new GameObject[8];

    private DisplayLocation[] displayLocation_script;

    public int count = 0;

    public GameObject[] doll_obj = new GameObject[8];

    void Start()
    {
        // - 배열 0 초기화
        Array.Clear(input_Arry, 0, input_Arry.Length);

        // - DisplayLocation 스크립트 배열
        displayLocation_script = GetComponentsInChildren<DisplayLocation>();

        for (int i = 0; i < displayLocation_script.Length; i++)
        {
            displayLocation_script[i].set_locaNum(i);
        }
    }

    void Update()
    {

    }

    public void set_DisplayArry(int index, int code)
    {
        input_Arry[index] = code;
    }

    public void reset_DisplayArry(int index)
    {
        input_Arry[index] = 0;
    }

    public int[] get_inputArry()
    {
        return input_Arry;
    }

    public void destroy_colliders()
    {
        for (int i = 0; i < displayLocation_script.Length; i++)
        {
            displayLocation_script[i].complete_layout();
        }
    }


}
