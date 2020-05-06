//
// 2020 - 02 - 22
// 아이템 습득 (완)
// 배치퍼즐과 연동 (ing)
// @은 최적화를 위해 이후에 바꿀 코드
// #은 임시로 주석을 한 코드
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ActionController_02_VER2 : MonoBehaviour
{
    public Inventory theInventory;

    [SerializeField]
    private float range;

    [SerializeField]
    private LayerMask layerMask;


    [SerializeField]
    private Text actionText;

    /// acquire true - false 
    private bool pickupActivated = false;//false;
    private RaycastHit hitInfo;

    // - 아이템 사용하기
    public LayerMask layerMask_dlsplay;
    public SelectSlot selectSlot_script;
    private bool PickUp_state = false;

    // - 배치퍼즐 관리자
    private DisplayManager_2stage displayManager_script;
    private DisplayManager_3stage displayManager_script2;
    private RaycastHit hitInfo2;
    private bool enter_3stage = false;

    private ActionController_03 actionController_3stage_script;

    // - 지하실과 2층으로 가는 길목 @ -> 이후에 네비메시 실시간 베이킹으로 바꾸기 
    public GameObject tempDoor;
    public GameObject tempStairs;

    // - 손전등
    public GameObject FlashlightItem;
    Flashlight_PRO flash_script;

    // - 라이트
    LightOn_3stage _lightOn_script;

    // - 외곽선
    private DrawOutline_HJ OutlineController;
    private int pre_ol_index = 0; //이전 아웃라인 인덱스

    void Start()
    {
        //배치퍼즐
        displayManager_script = GameObject.FindObjectOfType<DisplayManager_2stage>();
        displayManager_script2 = GameObject.FindObjectOfType<DisplayManager_3stage>();

        displayManager_script2.enabled = false;

        actionController_3stage_script = FindObjectOfType<ActionController_03>();

        //손전등
        FlashlightItem.SetActive(false);
        flash_script = FlashlightItem.GetComponent<Flashlight_PRO>();
        flash_script.enabled = false;

        //라이트
        _lightOn_script = GameObject.FindObjectOfType<LightOn_3stage>();

        //외곽선
        OutlineController = GameObject.FindObjectOfType<DrawOutline_HJ>();
    }


    void Update()
    {
        CheckItem();
        Check_Location();

        TryAction();
    }


    private void TryAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // - 아이템 습득
            CanPickUp();

            // - 아이템 사용
            Check_use_Item();
        }
    }

    private void CanPickUp()
    {
        if (pickupActivated)
        {
            if (hitInfo.transform != null)
            {
                if (hitInfo.transform.tag == "Item") //compare @
                {
                    if (theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item))
                    {
                        // - 아이템 습득
                        Destroy(hitInfo.transform.gameObject); //아이템 삭제

                        InfoDisappear(); //info 삭제

                        PickUp_state = true; // 습득한 상태로 변경 -> 이후 Check_use_Item에서 location_script의 상태 업데이트하기 위해서

                        // - 장식장 클릭
                        if (hitInfo2.transform != null) //null @
                        {
                            DisplayLocation location_script = hitInfo2.transform.GetComponent<DisplayLocation>(); // @
                            int display_index = location_script.location_Num;

                            // #
                            displayManager_script.reset_DisplayArry(display_index);
                        }
                    }

                    if (hitInfo.transform.GetComponent<ItemPickUp>().item.itemName == "FlashlightItem")
                    {
                        Destroy(hitInfo.transform.gameObject); //아이템 삭제, 나중엔 코루틴으로 #

                        FlashlightItem.SetActive(true);
                        flash_script.enabled = true;
                    }
                }
                else
                {

                }
            }
        }
    }


    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "Item") //compare @
            {
                ItemInfoAppear();
            }
        }
        else
        {
            InfoDisappear();
        }
    }


    // Need to modify
    private void ItemInfoAppear()
    {
        pickupActivated = true;

        //info
        //actionText.gameObject.SetActive(true);
        //actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득" + " [Click]";
    }
    public void InfoDisappear()
    {
        pickupActivated = false;

        //info
        //actionText.gameObject.SetActive(false);
    }


    //
    private void Check_use_Item()
    {
        if (hitInfo2.transform != null)
        {
            if (hitInfo2.transform.tag == "Location") //compare @
            {
                // - 클릭한 장식장 위치의 스크립트 얻기
                DisplayLocation location_script = hitInfo2.transform.GetComponent<DisplayLocation>();

                if (PickUp_state)
                {
                    // 장식장 인형 가질수있으면 가져가기
                    location_script.take_Doll();
                    PickUp_state = false;

                    // #
                    //if (!enter_3stage)
                    displayManager_script.count--;
                }
                else
                {
                    // - 선택슬롯의 인덱스 얻기
                    int use_index = selectSlot_script.get_index();

                    if (!theInventory.IsVoid_Slot(use_index)) //슬롯에 아이템이 있는가? 있으면 IsVoid_Slot반환값이 false
                    {
                        if (location_script.tryToPut_doll()) //장식장 위치에 이미 인형이 있는가? 없으면 true
                        {

                            // - 아이템 생성  O
                            location_script.setup_Doll(theInventory.get_Item(use_index));


                            // - 장식장 비교를 위한 변수

                            // 아이템 코드
                            int doll_itemCode = theInventory.get_ItemCode(use_index);
                            // 장식장 위치 넘버
                            int display_index = location_script.location_Num;

                            // - 아이템 코드 저장 #
                            displayManager_script.set_DisplayArry(display_index, doll_itemCode);

                            // - 아이템사용 후, 슬롯 클리어  O
                            theInventory.clear_Slot(use_index);

                            // - count 증가 #
                            displayManager_script.count++;

                            // - 2스테이지 장식장 배치가 3스테이지로 옮겨짐. 한번만 실행 #
                            if (displayManager_script.count == 8)
                            {
                                //2스테이지 배치퍼즐 off
                                displayManager_script.enabled = false;
                                displayManager_script.destroy_colliders();

                                //3스테이지 배치퍼즐 on
                                displayManager_script2.enabled = true;
                                displayManager_script2.init_inputArry(displayManager_script.get_inputArry());
                                displayManager_script2.Create_sameOne();

                                //라이트
                                _lightOn_script.LightOn();

                                //지하실, 2층계단 오픈
                                tempDoor.SetActive(false);
                                tempStairs.SetActive(false);

                                enter_3stage = true;

                                //3스테이지 카메라 스크립트 on
                                actionController_3stage_script.enabled = true;

                                //손전등 없어지기
                                FlashlightItem.SetActive(false);
                            }
                        }
                    }
                }

            }
        }
        else
        {
            PickUp_state = false;
        }
    }

    // - 장식장 인형 위치의 콜라이더 확인
    private void Check_Location()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo2, range, layerMask_dlsplay))
        {
            if (hitInfo2.transform.tag == "Location")
            {

            }
        }
    }
}
