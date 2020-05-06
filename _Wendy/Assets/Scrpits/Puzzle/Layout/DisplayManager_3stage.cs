using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DisplayManager_3stage : MonoBehaviour
{
    private int[] answer_Arry = new int[8]; // 정답

    public int[] input_Arry = new int[8]; // 장식장에 놓여진 인형들의 아이템 코드들
    private GameObject[] input_DollArry = new GameObject[8];

    private DisplayLocation[] displayLocation_script;
    public GameObject[] doll_obj = new GameObject[8];

    void Start()
    {
        // - 배열 0 초기화
        Array.Clear(answer_Arry, 0, answer_Arry.Length);
        Array.Clear(input_Arry, 0, input_Arry.Length);

        // - 정답 배열
        answer_Arry[0] = 35;
        answer_Arry[1] = 36;
        answer_Arry[2] = 33;
        answer_Arry[3] = 30;
        answer_Arry[4] = 32;
        answer_Arry[5] = 31;
        answer_Arry[6] = 34;
        answer_Arry[7] = 37;

        displayLocation_script = GetComponentsInChildren<DisplayLocation>();
    }

    void Update()
    {

    }

    public bool compare_Answer()
    {
        for (int i = 0; i < 8; i++)
        {
            if (input_Arry[i] == answer_Arry[i])
            {
                continue;
            }
            else
                return false;
        }

        return true;
    }

    public void set_DisplayArry(int index, int code)
    {
        input_Arry[index] = code;

        //int tempCode = code - 30;//code % 30;
    }

    public void reset_DisplayArry(int index)
    {
        input_Arry[index] = 0;
    }

    public void init_inputArry(int[] tempArry)
    {
        for (int i = 0; i < 8; i++)
        {
            input_Arry[i] = tempArry[i];
        }
    }

    //
    public void Create_sameOne()
    {
        for (int i = 0; i < 8; i++)
        {
            // - 비교, 원하는 위치에 원하는 인형인지 검사 -> @
            for (int j = 0; j < 8; j++)
            {
                if (input_Arry[i] == doll_obj[j].GetComponent<ItemPickUp>().get_itemCode())
                {
                    displayLocation_script[i].setup_Doll(doll_obj[j]);
                    break;
                }
            }

            //콜라이더 삭제 -> 다른 스크립트에서 함
            //input_DollArry[index].GetComponent<BoxCollider>()
        }
    }
}
