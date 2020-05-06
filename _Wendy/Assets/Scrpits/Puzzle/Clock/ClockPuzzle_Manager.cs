using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockPuzzle_Manager : MonoBehaviour
{
    // - 시계퍼즐 풀 수 있는지 상태 확인하기 (범위에 들어왔는가)
    public bool active = false;
    private bool end = false;

    // - 클릭
    // 카메라
    private Camera mainCamera;
    private Transform mCT;
    // 레이케스트
    private RaycastHit hitInfo;
    private float range = 2.5f;
    public LayerMask layerMask;

    Move_Cuckoo cuckoo_script;

    EnterClockAnswer enterBtn_script;
    DoorAni_reward reward_script;
    CustomFan fan_script;

    public GameObject reward;

    // - 외곽선
    private DrawOutline_HJ OutlineController;
    public int pre_ol_index = -1; //이전 아웃라인 인덱스


    void Start()
    {
        mainCamera = Camera.main;
        mCT = mainCamera.transform;

        cuckoo_script = GameObject.FindObjectOfType<Move_Cuckoo>();

        enterBtn_script = GameObject.FindObjectOfType<EnterClockAnswer>();
        reward_script = GameObject.FindObjectOfType<DoorAni_reward>();
        fan_script = GameObject.FindObjectOfType<CustomFan>();

        //외곽선
        OutlineController = GameObject.FindObjectOfType<DrawOutline_HJ>();
    }

    void Update()
    {
        LookAtClock();
        check_click();
    }

    private void LookAtClock()
    {
        if (Physics.Raycast(mCT.position, mCT.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.CompareTag("InputButton_CP"))
            {
                DrawOutline();
            }
            else if (hitInfo.transform.tag == "EnterButton_CP")
            {
                DrawOutline();
            }
        }
        else
        {
            if (pre_ol_index != -1)
            {
                //외곽선 해제
                OutlineController.set_enabled(pre_ol_index, false);
                pre_ol_index = -1;
            }
        }
    }

    private void DrawOutline()
    {
        //외곽선 그리기
        SetOutline setoutlin_script = hitInfo.transform.GetComponent<SetOutline>();
        if (pre_ol_index == -1)
        {
            OutlineController.set_enabled(setoutlin_script._index, true);
            pre_ol_index = setoutlin_script._index;
        }
        else
        {
            OutlineController.set_enabled(pre_ol_index, false);
            OutlineController.set_enabled(setoutlin_script._index, true);
            pre_ol_index = setoutlin_script._index;
        }
    }

    private void check_click()
    {
        if (end)
            return;

        if (!active)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            check_collider();
        }
    }

    private void check_collider()
    {
        //if (Physics.Raycast(mCT.position, mCT.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
        if (hitInfo.transform != null)
        {
            if (hitInfo.transform.tag == "InputButton_CP")
            {
                InputClockAnswer input_script = hitInfo.transform.GetComponent<InputClockAnswer>();
                input_script.click_button();
            }
            else if (hitInfo.transform.tag == "EnterButton_CP")
            {
                if (!enterBtn_script.get_result())
                {
                    cuckoo_script.start_cuckooAni();
                }
                else
                {
                    // - 코루틴으로 몇초뒤 스크립트가 enable = false 되는것은 @
                    end = true;
                    fan_script.cp_is_over();

                    // - 시계판 열리는 애니메이션
                    reward_script.set_Ani_param();

                    // - 피터팬인형 활성화
                    reward.SetActive(true);
                }
            }
        }
    }

    public void set_active(bool btemp)
    {
        active = btemp;
    }
    public bool get_active()
    {
        return active;
    }


    public Vector3 get_mainCam_pos()
    {
        return mCT.position;
    }

    public Vector3 get_hitinfo_pos()
    {
        return hitInfo.point;
    }
}
