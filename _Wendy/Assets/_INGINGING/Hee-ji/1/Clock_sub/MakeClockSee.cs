using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeClockSee : MonoBehaviour
{
    Ray Mouse_ray;
    int FramelayerMask;
    private float range = 2.5f;
    RaycastHit hitInfo;

    int CamObstacle_layerMask;
    Camera camera;

    // - 외곽선
    private DrawOutline_HJ OutlineController;
    public int pre_ol_index = -1; //이전 아웃라인 인덱스
    private bool outline_active = false;

    // - 카메라 전환
    ChangeCam_SubClock ChangeCam_script;
    // - 시계보기 스크립트
    private SeeingSubClock SeeClock_script;
    private SubClockInfo sub_clock_info;

    // - 클릭버튼
    public GameObject actionCaption;

    // - 시계 인덱스
    //private int cindex = 0;

    public Transform puzzleCam_trans;

    // - 장애물, 벽
    ObstacleReader obstacleReader_script;
    bool coverCheck = false; //막고잇으면 TRUE
    int obstacle_layer;

    void Start()
    {
        camera = GetComponent<Camera>(); //메인카메라

        FramelayerMask = (1 << LayerMask.NameToLayer("ClockPuzzle"));
        CamObstacle_layerMask = (1 << LayerMask.NameToLayer("Obstacle")) + (1 << LayerMask.NameToLayer("ClockPuzzle"));

        //외곽선
        OutlineController = GameObject.FindObjectOfType<DrawOutline_HJ>();

        //카메라전환
        ChangeCam_script = GameObject.FindObjectOfType<ChangeCam_SubClock>();

        //SeeClock_script = GameObject.FindObjectOfType<SeeingSubClock>();

        //장애물,벽
        obstacleReader_script = GameObject.FindObjectOfType<ObstacleReader>();
        obstacle_layer = (1 << LayerMask.NameToLayer("ClockPuzzle")) + (1 << LayerMask.NameToLayer("Obstacle"));
    }

    void Update()
    {
        if (CheckObstacle())
            return;

        LookAtClock();
        TryAction();
    }

    private void LookAtClock()
    {
        Mouse_ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(Mouse_ray, out hitInfo, range, CamObstacle_layerMask))
        {
            //if (OutlineController.get_outline_okay())
            //    return;

            if (hitInfo.transform.CompareTag("SubClock"))
            {
                // - 클릭버튼 활성화
                actionCaption.SetActive(true);

                //외곽선 그리기
                //if (pre_ol_index == -1)
                //{
                //    sub_clock_info = hitInfo.transform.gameObject.GetComponent<SubClockInfo>();

                //    SetOutline setoutlin_script = hitInfo.transform.GetComponent<SetOutline>();
                //    OutlineController.set_enabled(setoutlin_script._index, true);
                //    OutlineController.set_check(true);  //
                //    outline_active = true;              //
                //    pre_ol_index = setoutlin_script._index;
                //}

                sub_clock_info = hitInfo.transform.gameObject.GetComponent<SubClockInfo>();

                SetOutline setoutlin_script = hitInfo.transform.GetComponent<SetOutline>();
                int cur_index = setoutlin_script._index;

                OutlineController.set_check(true);
                outline_active = true;

                if (pre_ol_index == -1)
                {
                    OutlineController.set_enabled(cur_index, true);
                    pre_ol_index = cur_index;
                }
                else
                {
                    OutlineController.set_enabled(pre_ol_index, false);
                    OutlineController.set_enabled(cur_index, true);
                    pre_ol_index = cur_index;
                }
            }
            else
            {
                if (pre_ol_index != -1)
                {
                    // - 클릭버튼 해제
                    actionCaption.SetActive(false);

                    //외곽선 해제
                    OutlineController.set_enabled(pre_ol_index, false);
                    pre_ol_index = -1;
                    OutlineController.set_check(false);
                    outline_active = false;
                }
            }
        }
        else
        {
            if (pre_ol_index != -1)
            {
                // - 클릭버튼 해제
                actionCaption.SetActive(false);

                //외곽선 해제
                OutlineController.set_enabled(pre_ol_index, false);
                pre_ol_index = -1;
                OutlineController.set_check(false);
                outline_active = false;
            }
        }
    }

    void TryAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //스크린 클릭
            //Vector3 p = Input.mousePosition;
            //Mouse_ray = camera.ScreenPointToRay(Input.mousePosition);
            if (!outline_active)
                return;

            ClickClock();
        }
    }

    void ClickClock()
    {
        //if (Physics.Raycast(Mouse_ray, out hitInfo, range, CamObstacle_layerMask))// 벽이 아닌지도 검사 
        if (hitInfo.transform != null)
        {
            if (hitInfo.transform.CompareTag("SubClock"))
            {
                // - 클릭버튼 해제
                actionCaption.SetActive(false);

                if (pre_ol_index != -1)
                {
                    //외곽선 해제
                    OutlineController.set_enabled(pre_ol_index, false);
                    pre_ol_index = -1;
                    OutlineController.set_check(false);
                    outline_active = false;
                }

                //카메라 변경
                ChangeCam_script.change_Camera(true);

                puzzleCam_trans.position = sub_clock_info._camPos.position;
                //ChangeCam_script.SeeingClock_script = sub_clock_info.seeingSubClock_script;
            }
        }
    }

    public void set_active(bool b)
    {
        if(b == false) //비활성화하기직전 
        {
            if (pre_ol_index != -1)
            {
                // - 클릭버튼 해제
                actionCaption.SetActive(false);

                //외곽선 해제
                OutlineController.set_enabled(pre_ol_index, false);
                pre_ol_index = -1;
                OutlineController.set_check(false);
                outline_active = false;
            }
        }
    }

    private bool CheckObstacle()
    {
        // - 장애물 검사하기
        coverCheck = obstacleReader_script.LookAtFrame(obstacle_layer);
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
