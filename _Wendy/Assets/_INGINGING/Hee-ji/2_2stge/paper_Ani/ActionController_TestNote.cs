using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController_TestNote : MonoBehaviour
{
    /// acquire true - false 
    private bool pickupActivated = false;
    private RaycastHit hitInfo;
    [SerializeField]
    private float range;
    [SerializeField]
    private LayerMask layerMask;

    //[SerializeField]
    //private Image actionImage;
    // - 클릭버튼
    [SerializeField]
    private GameObject actionCaption;

    // - 쪽지
    //ViewNote_Ani_02 viewNote_script_clock;
    private FlodNote clockNote_script;

    bool popupNote = false;
    bool opening = false;

    //FirstPersonCamera fpCam_Script;
    //Player_HJ player_script; //쪽지이동스크립트에 포함되어있음
    public GameObject Aim;

    // - 외곽선
    private DrawOutline_HJ OutlineController;
    private int pre_ol_index = -1; //이전 아웃라인 인덱스
    private bool outline_active = false;

    // - 장애물, 벽
    ObstacleReader obstacleReader_script;
    bool coverCheck = false; //막고잇으면 TRUE
    int _obstacle_layer;

    // - 쪽지 매니저
    NoteManger notemager;

    // - 쪽지 상태 스크립트
    SawNoteNumber note_num_script;

    void Start()
    {
        OutlineController = GameObject.FindObjectOfType<DrawOutline_HJ>();

        clockNote_script = GameObject.FindObjectOfType<FlodNote>();

        //fpCam_Script = Camera.main.GetComponent<FirstPersonCamera>();
        //player_script = GameObject.FindObjectOfType<Player_HJ>();

        // 외곽선
        OutlineController = GameObject.FindObjectOfType<DrawOutline_HJ>();

        // 장애물,벽
        obstacleReader_script = GameObject.FindObjectOfType<ObstacleReader>();
        _obstacle_layer = (1 << LayerMask.NameToLayer("NotePage")) + (1 << LayerMask.NameToLayer("Obstacle"));

        // 쪽지 매니저
        notemager = FindObjectOfType<NoteManger>();

        // 쪽지 상태 (싱글톤)
        note_num_script = FindObjectOfType<SawNoteNumber>();
    }

    void Update()
    {
        if (!popupNote)
        {
            if (CheckObstacle())
                return;

            CheckNote();
        }
        TryAction();
    }

    private void TryAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // - 애니메이션 쪽지
            if (!popupNote)
            {
                CanPickUp();
            }
            else
            {
                // 애니메이션 쪽지 (시계쪽지)
                if (!opening) // 2번 : 시계쪽지 순서
                {
                    //쪽지 열기 애니메이션 (이동은 CanPickUp에서)
                    if (clockNote_script.openAni_Note())
                        opening = true;
                }
                else // 3번 : 시계쪽지 순서
                {
                    //접기 + 원위치로 이동
                    if (clockNote_script.endAni_Note())
                    {
                        //// - 쪽지 매니저
                        //notemager._popup = false; //reset_NoteState 함수에 포함

                        opening = false;
                        popupNote = false; //->위 호출이 다끝나면 변하게해야함
                    }
                }
            }
        }
    }

    private void CanPickUp()
    {
        if (pickupActivated)
        {
            if (hitInfo.transform != null)
            {
                if (hitInfo.transform.CompareTag("Note_CP")) // 1번 : 시계쪽지 순서
                {
                    // - 쪽지 상태, UI
                    hitInfo.transform.GetComponent<PageNote>().CheckAddcount(1);
                    if (note_num_script != null)
                        note_num_script.SetNoteCount();
                    // - 쪽지 매니저
                    notemager._popup = true;

                    InfoDisappear();

                    // - 상태
                    popupNote = true;

                    // - 모델링교체 (외곽선때문에)
                    clockNote_script.SetActive_Ani(true);
                    clockNote_script.SetActive_Outline(false);

                    // - 외곽선, 클릭버튼 해제보다 먼저 해야함
                    clockNote_script.startAni_Note();
                    Aim.SetActive(false);

                    // - 외곽선 해제
                    OutlineController.set_enabled(pre_ol_index, false);
                    pre_ol_index = -1;
                    OutlineController.set_check(false);
                    outline_active = false;

                    // - 클릭버튼 비활성화
                    actionCaption.SetActive(false);
                }
            }
        }
    }

    private void CheckNote()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.CompareTag("Note_CP"))
            {
                if (OutlineController.get_outline_okay())
                    return;

                // - info 띄우기
                ItemInfoAppear();

                // - 클릭버튼 활성화
                actionCaption.SetActive(true);

                // - 외곽선
                SetOutline setoutlin_script = hitInfo.transform.GetComponent<SetOutline>();
                int cur_ol_index = setoutlin_script._index;

                OutlineController.set_check(true);
                outline_active = true;

                if (pre_ol_index == -1)
                {
                    OutlineController.set_enabled(cur_ol_index, true);
                    pre_ol_index = cur_ol_index;
                }
                else
                {
                    OutlineController.set_enabled(pre_ol_index, false);
                    OutlineController.set_enabled(cur_ol_index, true);
                    pre_ol_index = cur_ol_index;
                }
            }
        }
        else
        {
            InfoDisappear();

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


    // Need to modify
    private void ItemInfoAppear()
    {
        pickupActivated = true;
        //actionImage.gameObject.SetActive(true);
    }

    public void InfoDisappear()
    {
        pickupActivated = false;
        //actionImage.gameObject.SetActive(false);
    }

    public void reset_NoteState()
    {
        //책 팝업상태 해제
        popupNote = false; //팝업 상태 

        ////카메라, 플레이어 이동 가능
        //fpCam_Script.enabled = true;
        //player_script.enabled = true;

        Aim.SetActive(true);

        // - 쪽지 매니저
        notemager._popup = false;
    }

    private bool CheckObstacle()
    {
        // - 장애물 검사하기
        coverCheck = obstacleReader_script.LookAtFrame(_obstacle_layer);
        if (coverCheck)
        {
            if (pre_ol_index != -1)
            {
                // - 외곽선 해제
                OutlineController.set_enabled(pre_ol_index, false);
                pre_ol_index = -1;
                OutlineController.set_check(false);
                outline_active = false;

                // - 클릭버튼 해제
                actionCaption.SetActive(false);
            }

            return true;
        }

        return false;
    }
}
