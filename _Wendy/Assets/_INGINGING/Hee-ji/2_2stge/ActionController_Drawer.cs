using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController_Drawer : MonoBehaviour
{


    [SerializeField]
    private float range;
    [SerializeField]
    private LayerMask layerMask;
    private RaycastHit hitaction;
    private Camera mainCam;

    private bool pickupActivated;
    //[SerializeField]
    //private Image actionImage;
    public GameObject actionCaption;

    // - 서랍
    public GameObject[] moveChest;

    // - 외곽선
    private DrawOutline_HJ OutlineController;
    private int pre_ol_index = -1; //이전 아웃라인 인덱스
    private bool outline_active = false;

    // - 장애물, 벽
    ObstacleReader obstacleReader_script;
    bool coverCheck = false; //막고잇으면 TRUE
    int _obstacle_layer;

    void Start()
    {
        mainCam = GetComponent<Camera>();

        // 외곽선
        OutlineController = GameObject.FindObjectOfType<DrawOutline_HJ>();

        // 장애물,벽
        obstacleReader_script = GameObject.FindObjectOfType<ObstacleReader>();
        _obstacle_layer = (1 << LayerMask.NameToLayer("Drawer")) + (1 << LayerMask.NameToLayer("Obstacle"));
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckObstacle())
            return;

        LookAtDrawer();
        TryAction();
    }

    private void LookAtDrawer()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitaction, range, layerMask))
        {

            if (hitaction.transform.CompareTag("Drawer")) //compare @
            {
                // - 외곽선
                SetOutline setoutlin_script = hitaction.transform.GetComponent<SetOutline>();
                int cur_ol_index = setoutlin_script._index;

                if (pre_ol_index == cur_ol_index) // 서랍-서랍 사이의 외곽선 보정
                {
                    if (OutlineController.get_outline_okay())
                        return;
                }

                ActionAppear();

                // - 클릭버튼 활성화
                actionCaption.SetActive(true);


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
            ActionDisappear();

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

    private void TryAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // - 서랍 클릭했는지 검사
            Check_Do_action();
        }
    }


    private void Check_Do_action()
    {
        if (hitaction.transform != null) //pickupActivated == true
        {
            if (hitaction.transform.tag == "Drawer") //compare @
            {
                int Chestnumber = hitaction.transform.parent.GetComponent<Chestaction>().Chest_number;
                int drawerType = hitaction.transform.parent.GetComponent<Chestaction>().type;
                moveChest[Chestnumber].transform.parent.GetComponent<Chestaction>().Start_action(drawerType);




                // - 외곽선 해제
                OutlineController.set_enabled(pre_ol_index, false);
                pre_ol_index = -1;
                OutlineController.set_check(false);
                outline_active = false;

                // - 클릭버튼 해제
                actionCaption.SetActive(false);

                // - 사운드
            }
        }
    }

    // Need to modify
    private void ActionAppear()
    {
        pickupActivated = true;
        //actiontext.gameObject.SetActive(true);
        //actiontext.text =  "서랍 여닫기 [Click]";
    }
    public void ActionDisappear()
    {
        pickupActivated = false;
        //actiontext.gameObject.SetActive(false);
    }

    private bool CheckObstacle()
    {
        // - 장애물 검사하기
        coverCheck = obstacleReader_script.LookAtFrame((int)_obstacle_layer);
        if (coverCheck)
        {
            pickupActivated = false;

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
