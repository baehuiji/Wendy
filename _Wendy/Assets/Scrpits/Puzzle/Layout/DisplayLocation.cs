using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayLocation : MonoBehaviour
{
    public bool state = false; //있으면 true, 없으면 false
    public int location_Num = 0;
    
    GameObject clone;
    BoxCollider[] colliders;

    void Start()
    {
    }

    void Update()
    {

    }

    public void setup_Doll(GameObject obj, int select)
    {
        switch (select)
        {
            case 1:
                clone = Instantiate(obj, gameObject.transform.position, transform.rotation);

                break;

            case 2:
                //// 2. 습득한 것을 비활성화, 이후 이동, 활성화 : (ref GameObject obj, int select)
                //obj.transform.position = gameObject.transform.position;
                //obj.transform.rotation = gameObject.transform.rotation;
                //obj.transform.gameObject.SetActive(true);

                //Debug.Log("test");

                break;

            case 3:
                //DisplayManager 에서 바꾼다
                break;

            default:
                break;
        }

        // - 콜라이더
        colliders = clone.GetComponents<BoxCollider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }

        state = true;
    }
    public Transform get_Lotation_Trans()
    {
        return gameObject.transform;
    }

    public void take_Doll()
    {
        state = false;
    }

    public bool tryToPut_doll()
    {
        if (state) //인형이 놓여진 상태일때
            return false;
        else
            return true;
    }

    public void set_locaNum(int num)
    {
        location_Num = num;
    }

    public void complete_layout()
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }
    }

    // + 위치
    public Vector3 get_DisplayPosition()
    {
        return transform.position;
    }

    // + 회전값
    public Quaternion get_DisplayRotation()
    {
        return transform.rotation;
    }

    // + 상태
    public void lay_Doll()
    {
        state = true;
    }
}
