using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramePuzzle_Enter : MonoBehaviour
{
    Ray Mouse_ray;
    int FramelayerMask;
    private float range = 2.5f;
    RaycastHit hitInfo;

    int CamObstacle_layerMask;
    Camera camera;

    FramePuzzle_ChangeCam fpCameraController;

    bool puzzleEnd = false;

    bool test = false;

    // - 외곽선
    private DrawOutline_HJ OutlineController;
    public int pre_ol_index = -1; //이전 아웃라인 인덱스
    private bool outline_active = false;

    // - 클릭버튼
    public GameObject actionCaption;

    ActionController_02_VER2 actionCtrler2_script;

    // - 장애물, 벽
    ObstacleReader obstacleReader_script;
    bool coverCheck = false;

    // - 쪽지 매니저
    NoteManger notemager;


    void Start()
    {
        camera = GetComponent<Camera>(); //메인카메라

        FramelayerMask = (1 << LayerMask.NameToLayer("FramePuzzle"));
        CamObstacle_layerMask = (1 << LayerMask.NameToLayer("Obstacle")) + (1 << LayerMask.NameToLayer("FramePuzzle"));

        fpCameraController = GameObject.FindObjectOfType<FramePuzzle_ChangeCam>();

        //외곽선
        OutlineController = GameObject.FindObjectOfType<DrawOutline_HJ>();

        actionCtrler2_script = GameObject.FindObjectOfType<ActionController_02_VER2>();

        //장애물,벽
        obstacleReader_script = GameObject.FindObjectOfType<ObstacleReader>();

        // 쪽지 매니저
        notemager = FindObjectOfType<NoteManger>();
    }

    void Update()
    {
        if (puzzleEnd)
        {
            this.enabled = false;
            return;
        }

        if (CheckObstacle()) //장애물 체크
            return;

        if (notemager._popup == true) //쪽지 팝업 체크 : 팝업되지 않은 상태여야 가능하도록
            return;

        LookAtFrame();
        TryAction();

        //if (test)
        //    Debug.DrawRay(Mouse_ray.origin, Mouse_ray.direction * range, Color.red, range);
    }

    private void LookAtFrame()
    {
        Mouse_ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(Mouse_ray, out hitInfo, range, FramelayerMask))
        {
            if (OutlineController.get_outline_okay())
                return;

            if (hitInfo.transform.CompareTag("Frame"))
            {
                //// - 장애물 검사하기
                //coverCheck = obstacleReader_script.LookAtFrame(CamObstacle_layerMask);
                //if (coverCheck)
                //    return;

                // - 클릭버튼 활성화
                actionCaption.SetActive(true);

                // - 외곽선 그리기
                if (pre_ol_index == -1)
                {
                    SetOutline setoutlin_script = hitInfo.transform.GetComponent<SetOutline>();
                    OutlineController.set_check(true);
                    OutlineController.set_enabled(setoutlin_script._index, true);
                    pre_ol_index = setoutlin_script._index;
                    outline_active = true;
                }
            }
            else
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
            }
        }
        else
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

                // - 장애물 검사하기
                //coverCheck = obstacleReader_script.LookAtFrame(CamObstacle_layerMask);
            }
        }
    }

    void TryAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (puzzleEnd)
                return;

            if (!outline_active)
                return;

            //test = true;

            //스크린 클릭
            Vector3 p = Input.mousePosition;
            Mouse_ray = camera.ScreenPointToRay(Input.mousePosition);

            ClickFrame();
        }
    }

    void ClickFrame()
    {
        if (Physics.Raycast(Mouse_ray, out hitInfo, range, CamObstacle_layerMask))// 벽이 아닌지도 검사 
        {
            if (hitInfo.transform.CompareTag("Frame"))
            {
                //카메라 변경
                fpCameraController.change_Camera(true);

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
            }
        }
    }

    public void set_puzzleEnd()
    {
        puzzleEnd = true;
        OutlineController.set_enabled(0, false);
        OutlineController.set_check(false);
        actionCtrler2_script.enabled = true;

        //this.enabled = false; //-> 업데이트함수에 있음
    }

    private bool CheckObstacle()
    {
        //if (coverCheck)
        //{
        //    coverCheck = obstacleReader_script.LookAtFrame(CamObstacle_layerMask);
        //    return true;
        //}

        // - 장애물 검사하기
        coverCheck = obstacleReader_script.LookAtFrame(CamObstacle_layerMask);
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
