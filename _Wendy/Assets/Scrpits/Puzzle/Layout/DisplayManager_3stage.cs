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

        for (int i = 0; i < displayLocation_script.Length; i++)
        {
            displayLocation_script[i].set_locaNum(i);
        }
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
        for (int i = 0; i < displayLocation_script.Length; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (input_Arry[i] == doll_obj[j].GetComponent<ItemPickUp>().get_itemCode())
                {
                    MoveSelectedInputArry(
                    j, displayLocation_script[i].get_DisplayPosition(), displayLocation_script[i].get_DisplayRotation());
                    break;
                }
            }
        }
    }

    public void MoveSelectedInputArry(int index, Vector3 newPos, Quaternion newRot)
    {
        doll_obj[index].transform.position = newPos;
        doll_obj[index].transform.rotation = newRot;
        doll_obj[index].SetActive(true);
    }

    // +
    public int compareItemCode(int find)
    {
        ItemPickUp tempItemInfo = null;
        for (int i = 0; i < doll_obj.Length; i++)
        {
            tempItemInfo = doll_obj[i].transform.GetComponent<ItemPickUp>();
            if (find == tempItemInfo.get_itemCode())
            {
                return i;
            }
        }

        return -1;
    }

    // 배치퍼즐에 아무것도 놓아진게 있는지 확인 -> 장식장이 비워져있으면 false
    public bool get_inputState()
    {
        for (int i = 0; i < 8; i++)
        {
            if (input_Arry[i] == 0)
            {
                continue;
            }
            else
            {
                return true;
            }
        }

        return false;
    }
}
