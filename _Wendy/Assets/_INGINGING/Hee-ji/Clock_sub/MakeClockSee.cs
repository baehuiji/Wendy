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

    // - 카메라 전환
    ChangeCam_SubClock ChangeCam_script;
    // - 시계보기 스크립트
    SeeingSubClock SeeClock_script;

    void Start()
    {
        SeeClock_script = GameObject.FindObjectOfType<SeeingSubClock>();

        camera = GetComponent<Camera>(); //메인카메라

        FramelayerMask = (1 << LayerMask.NameToLayer("ClockPuzzle"));
        CamObstacle_layerMask = (1 << LayerMask.NameToLayer("Obstacle")) + (1 << LayerMask.NameToLayer("ClockPuzzle"));

        //외곽선
        OutlineController = GameObject.FindObjectOfType<DrawOutline_HJ>();

        //카메라전환
        ChangeCam_script = GameObject.FindObjectOfType<ChangeCam_SubClock>();
    }

    void Update()
    {
        LookAtClock();
        TryAction();
    }

    private void LookAtClock()
    {
        Mouse_ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(Mouse_ray, out hitInfo, range, CamObstacle_layerMask))
        {
            if (hitInfo.transform.CompareTag("SubClock"))
            {
                //외곽선 그리기
                if (pre_ol_index == -1)
                {
                    SetOutline setoutlin_script = hitInfo.transform.GetComponent<SetOutline>();
                    OutlineController.set_enabled(setoutlin_script._index, true);
                    pre_ol_index = setoutlin_script._index;
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

            ClickFrame();
        }
    }

    void ClickFrame()
    {
        //if (Physics.Raycast(Mouse_ray, out hitInfo, range, CamObstacle_layerMask))// 벽이 아닌지도 검사 
        if (hitInfo.transform != null)
        {
            if (hitInfo.transform.CompareTag("SubClock"))
            {
                //카메라 변경
                ChangeCam_script.change_Camera(true);
            }
        }
    }

}
