using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SelectSlot : MonoBehaviour
{
    private Vector3 lastTargetPosition = Vector3.zero;
    private Vector3 currentTargetPosition = Vector3.zero;
    private float currentLerpDistance = 0.0f; //progress, 진행백분율

    public Transform[] slotPos;
    public int cur_index = 0;
    public int last_index = 0;

    public bool scrollEnd = true;
    public bool scrollStart = false;

    private float movingSpeedOrTime = 0.2f; //프로그레스에 쓰임

    public bool select_EndSlot = false;

    public GameObject Up_Wheel; 
    public GameObject Down_Wheel;

    public Sprite Upsprite;
    public Sprite UPLsprite;

    public Sprite Downsprite;
    public Sprite DownLsprite;


    void Start()
    {
        lastTargetPosition = slotPos[cur_index].position; // startTargetPos;
        currentTargetPosition = slotPos[last_index].position; // startTargetPos;
        currentLerpDistance = 0.0f;
    }

    void LateUpdate() // Update()
    {
        if (scrollEnd)
            return;

        transform.position = Vector3.Lerp(slotPos[last_index].position, slotPos[cur_index].position, currentLerpDistance);

        if (last_index < cur_index)
        {
            Down_Wheel.GetComponent<Image>().sprite = DownLsprite;
        }
        if (cur_index < last_index)
        {
            Up_Wheel.GetComponent<Image>().sprite = UPLsprite;
        }

        currentLerpDistance += Time.deltaTime / movingSpeedOrTime; //0.02f;
        if (currentLerpDistance > 1.2f && !select_EndSlot)
        {
            reset_slotPos();
        }
    }

    public void Set_slotPos_index(int dir)
    {
        if (!scrollStart || select_EndSlot)
        {
            //스크롤 애니메이션 시작
            scrollStart = true;
            scrollEnd = false;

            //지난 인덱스, 그것의 포지션 위치 업데이트
            last_index = cur_index;
            lastTargetPosition = currentTargetPosition;

            //현재 인덱스, 그것의 포지션 위치 업데이트
            select_EndSlot = false;
            cur_index += dir;
            if (cur_index < 0)
            {
                select_EndSlot = true;
                cur_index = 0;

                currentLerpDistance = 1.0f;
            }
            else if (cur_index > 10)
            {
                select_EndSlot = true;
                cur_index = 10;

                currentLerpDistance = 1.0f;
            }

            if (last_index != cur_index)
            {
                currentLerpDistance = 0.0f;
            }

            currentTargetPosition = slotPos[cur_index].position;

        }
    }

    public void reset_slotPos()
    {
        scrollStart = false;
        scrollEnd = true;

        Up_Wheel.GetComponent<Image>().sprite = Upsprite;
        Down_Wheel.GetComponent<Image>().sprite = Downsprite;

        //select_EndSlot = false;
    }

    public int get_index()
    {
        return cur_index;
    }
}
