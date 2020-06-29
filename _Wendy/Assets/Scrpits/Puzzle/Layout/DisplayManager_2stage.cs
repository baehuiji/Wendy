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
        // - 복제
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (input_Arry[i] == doll_obj[j].GetComponent<ItemPickUp>().get_itemCode())
                {
                    displayLocation_script[i].setup_Doll(doll_obj[j], 1);
                    break;
                }
            }
        }

        for (int i = 0; i < displayLocation_script.Length; i++)
        {
            displayLocation_script[i].enabled = false;
        }

    }

    public void Create_sameOne()
    {
        for (int i = 0; i < 8; i++)
        {
            // - 비교, 원하는 위치에 원하는 인형인지 검사 -> @
            for (int j = 0; j < 8; j++)
            {
                if (input_Arry[i] == doll_obj[j].GetComponent<ItemPickUp>().get_itemCode())
                {
                    displayLocation_script[i].setup_Doll(doll_obj[j], 1);
                    break;
                }
            }

            //콜라이더 삭제
            //input_DollArry[index].GetComponent<BoxCollider>()
        }
    }

    // +
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

            if (tempItemInfo != null)
            {
                if (find == tempItemInfo.get_itemCode())
                {
                    return i;
                }
            }
        }

        return -1;
    }

}
