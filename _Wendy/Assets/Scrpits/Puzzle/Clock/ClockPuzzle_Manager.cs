using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockPuzzle_Manager : MonoBehaviour
{
    [SerializeField]
    private string TurnDial = "CP_trunDial";

    [SerializeField]
    private string TryButton = "CP_tryButton";

    [SerializeField]
    private string OpenClock = "CP_openClock";

    // - 시계퍼즐 풀 수 있는지 상태 확인하기 (범위에 들어왔는가)
    public bool active = false;
    private bool end = false; //클리어상태

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
    bool popup_anmu = false;
    private bool outline_active = false;

    // - 클릭버튼
    public GameObject actionCaption;

    // - 서브시계 스크립트
    private MakeClockSee SeeClock_script;
 

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

        SeeClock_script = GameObject.FindObjectOfType<MakeClockSee>();
    }

    void Update()
    {
        LookAtClock();
        check_click();
    }

    private void LookAtClock()
    {
        if (popup_anmu)
            return;

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
            else
            {
                if (pre_ol_index != -1)
                {
                    //외곽선 해제
                    OutlineController.set_enabled(pre_ol_index, false);
                    pre_ol_index = -1;
                    OutlineController.set_check(false);
                    outline_active = false;

                    // - 클릭버튼 해제
                    actionCaption.SetActive(false);
                }
            }
        }
        else
        {
            if (pre_ol_index != -1)
            {
                //외곽선 해제
                OutlineController.set_enabled(pre_ol_index, false);
                pre_ol_index = -1;
                OutlineController.set_check(false);
                outline_active = false;

                // - 클릭버튼 해제
                actionCaption.SetActive(false);
            }
        }
    }

    private void DrawOutline()
    {
        // - 클릭버튼 활성화
        actionCaption.SetActive(true);

        // - 외곽선 그리기
        SetOutline setoutlin_script = hitInfo.transform.GetComponent<SetOutline>();

        if (setoutlin_script._index <= 5 && setoutlin_script._index != 0) //시계퍼즐은 조건(범위에들어와야함)이 있기때문에 우선권을 가진다
        {
            OutlineController.set_check(true);
            outline_active = true;

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
    }

    private void check_click()
    {
        if (end)
            return;

        if (!active)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (!outline_active)
                return;

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

                SoundManger.instance.PlaySound(TurnDial);
                input_script.click_button();
            }
            else if (hitInfo.transform.tag == "EnterButton_CP")
            {
                SoundManger.instance.PlaySound(TryButton);

                if (!enterBtn_script.get_result()) // 오답
                {
                    popup_anmu = true;
                    if (pre_ol_index != -1)
                    {
                        //외곽선 해제
                        OutlineController.set_enabled(pre_ol_index, false);
                        pre_ol_index = -1;
                        OutlineController.set_check(false);
                        outline_active = false;

                        // - 클릭버튼 해제
                        actionCaption.SetActive(false);
                    }
                    // 앵무새 애니메이션
                    cuckoo_script.start_cuckooAni();
                }
                else //정답
                {
                    // - 시계움직임 멈추기
                    fan_script.cp_is_over();

                    // - 시계판 열리는 애니메이션
                    SoundManger.instance.PlaySound(OpenClock);
                    reward_script.set_Ani_param();


                    // - 피터팬인형 활성화
                    reward.SetActive(true);

                    // - 외곽선 해제
                    if (pre_ol_index != -1)
                    {
                        OutlineController.set_enabled(pre_ol_index, false);
                        pre_ol_index = -1;
                        OutlineController.set_check(false);
                        outline_active = false;

                        // - 클릭버튼 해제
                        actionCaption.SetActive(false);
                    }

                    // - 해제, 코루틴으로 몇초뒤 스크립트가 enable = false 되는것은 @ -> ?
                    end = true;
                    SeeClock_script.enabled = false;
                    this.enabled = false;
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

    public void set_popup_anmu(bool b)
    {
        popup_anmu = b;
    }

    public void release_collider()
    {
        //시계퍼즐 영역을 벗어나면, 
        if (pre_ol_index != -1)
        {
            OutlineController.set_enabled(pre_ol_index, false);
            pre_ol_index = -1;
            OutlineController.set_check(false);
            outline_active = false;

            // - 클릭버튼 해제
            actionCaption.SetActive(false);
        }
    }
}
