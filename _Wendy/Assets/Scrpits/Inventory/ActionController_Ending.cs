using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController_Ending : MonoBehaviour
{
    public Inventory theInventory;
    private SelectSlot selectSlot_script;

    [SerializeField]
    private float range;

    [SerializeField]
    private LayerMask layerMask;

    //[SerializeField]
    //private Text actionText;
    public GameObject actionCaption;

    private bool openActivated = false;
    private RaycastHit hitInfo;

    public GameObject Aim;

    private EndingVideo_Loading loadEnding_script;

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

    void Start()
    {
        selectSlot_script = GameObject.FindObjectOfType<SelectSlot>();

        loadEnding_script = GameObject.FindObjectOfType<EndingVideo_Loading>();

        // 외곽선
        OutlineController = GameObject.FindObjectOfType<DrawOutline_HJ>();

        //장애물,벽
        obstacleReader_script = GameObject.FindObjectOfType<ObstacleReader>();
        _obstacle_layer = (1 << LayerMask.NameToLayer("Ending")) + (1 << LayerMask.NameToLayer("Obstacle"));

        //쪽지매니저
        notemager = FindObjectOfType<NoteManger>();
    }

    void Update()
    {
        if (CheckObstacle())
            return;

        CheckHit();

        TryAction();
    }

    private void TryAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CanOpen();
        }
    }

    private void CheckHit()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
        {
            if (OutlineController.get_outline_okay())
                return;

            if (hitInfo.transform.tag == "Door") //compare @
            {
                InfoAppear();

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

    private void CanOpen()
    {
        if (openActivated)
        {
            if (hitInfo.transform != null)
            {
                // - 선택슬롯에 아무것도 없을때 (열쇠꽂기실패)
                if (theInventory.IsVoid_Slot(selectSlot_script.get_index()))
                {
                    //실패 사운드 @

                    return;
                }

                // - 선택슬롯에 무언가 있을때
                int select_itemCode = theInventory.get_ItemCode(selectSlot_script.get_index());
                Debug.Log(select_itemCode.ToString());

                // - 선택슬롯으로 열쇠를 가리키면 (성공)
                if (select_itemCode == 40)
                {
                    // - 쪽지매니저 호출
                    notemager.OpenCondition();

                    // - 영상틀기 (O)
                    //loadEnding_script.InStartFadeAnim(); //쪽지매니저와 합치기

                    //Aim.SetActive(false);
                }
                else
                {
                    // - (실패)
                    //실패 사운드 @

                }
            }
        }
    }

    private void InfoAppear()
    {
        openActivated = true;
        //actionText.gameObject.SetActive(true);
        //actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득" + " [Click]";
    }
    public void InfoDisappear()
    {
        openActivated = false;
        //actionText.gameObject.SetActive(false);
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
