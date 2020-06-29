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
    private string bellSound = "AP_bell";
    [SerializeField]
    private string deskSound = "AP_openDesk";

    [SerializeField]
    private float range;

    [SerializeField]
    private LayerMask layerMask;
    int item_layerMask;

    //[SerializeField]
    //private Text actionText;
    // - 클릭버튼
    [SerializeField]
    private GameObject actionCaption;

    /// acquire true - false 
    private bool pickupActivated = false;//false;
    private RaycastHit hitInfo;

    // - 아이템 사용하기
    public LayerMask layerMask_dlsplay;
    int location_layerMask;
    public SelectSlot selectSlot_script;
    private bool PickUp_state = false;

    // - 배치퍼즐 관리자
    private DisplayManager_2stage displayManager_script;
    private DisplayManager_3stage displayManager_script2;
    private RaycastHit hitInfo2;
    private bool enter_3stage = false;

    private ActionController_03 actionController_3stage_script;

    // - 지하실과 2층으로 가는 길목 @ -> 이후에 네비메시 실시간 베이킹으로 바꾸기 
    public GameObject tempDoor; //지하실 + 2층
    public GameObject tempStairs; //지하실 문
    public GameObject temp_basement; //지하실
    public GameObject temp_stair; //2층

    //- 지하실과 2층 길목에 있을 가이드 링
    public GameObject Ring_Particle;
    ParticleSystem event_ringParticle;

    // - 손전등
    public GameObject FlashlightItem;
    Flashlight_PRO flash_script;
    OnTrigger_Flash flash_end;

    // - 라이트
    LightOn_3stage _lightOn_script;

    // - 외곽선
    private DrawOutline_HJ OutlineController;
    private int pre_ol_index = -1; //이전 아웃라인 인덱스
    private bool outline_active = false;

    // - Wendy AI
    WendyAI wendyAI_Script;

    // - 3스테이지 배치퍼즐
    DollAniManager dollAniManager_script;

    // - 장애물, 벽
    ObstacleReader obstacleReader_script;
    bool coverCheck = false; //막고잇으면 TRUE, 아이템
    bool coverCheck2 = false; //막고잇으면 TRUE, 배치퍼즐 장식장 location 확인

    // - 쪽지 매니저
    NoteManger notemager;


    void Start()
    {
        item_layerMask = (1 << LayerMask.NameToLayer("Item")) + (1 << LayerMask.NameToLayer("Doll"));

        //배치퍼즐
        location_layerMask = (1 << LayerMask.NameToLayer("Display"));
        displayManager_script = GameObject.FindObjectOfType<DisplayManager_2stage>();
        displayManager_script2 = GameObject.FindObjectOfType<DisplayManager_3stage>();

        displayManager_script2.enabled = false;

        actionController_3stage_script = FindObjectOfType<ActionController_03>();

        //손전등
        FlashlightItem.SetActive(false);
        flash_script = FlashlightItem.GetComponent<Flashlight_PRO>();
        flash_script.enabled = false;
        flash_end = FindObjectOfType<OnTrigger_Flash>();

        //파티클
        event_ringParticle = Ring_Particle.GetComponentInChildren<ParticleSystem>();

        //라이트
        _lightOn_script = GameObject.FindObjectOfType<LightOn_3stage>();

        //외곽선
        OutlineController = GameObject.FindObjectOfType<DrawOutline_HJ>();

        //웬디
        wendyAI_Script = GameObject.FindObjectOfType<WendyAI>();

        //장애물,벽
        obstacleReader_script = GameObject.FindObjectOfType<ObstacleReader>();

        // 쪽지 매니저
        notemager = FindObjectOfType<NoteManger>();
    }

    void Update()
    {
        if (CheckObstacle()) //장애물 체크
            return;

        if (notemager._popup == true) //쪽지 팝업 체크
            return;

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
        if (!outline_active) // 습득과 관련된 아이템은 외곽선과 상관 있다
            return;

        if (pickupActivated)
        {
            if (hitInfo.transform != null)
            {
                if (hitInfo.transform.CompareTag("Item"))
                {
                    if (theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item))
                    {
                        // - 아이템 습득
                        hitInfo.transform.gameObject.SetActive(false); //아이템 비활성화
                        OutlineController.set_enabled(pre_ol_index, false);
                        OutlineController.set_check(false);
                        outline_active = false;

                        InfoDisappear(); //info 삭제

                        PickUp_state = true; // 습득한 상태로 변경 -> 이후 Check_use_Item에서 location_script의 상태 업데이트하기 위해서

                        // - 장식장 클릭 (인형이 놓여져있음)
                        if (hitInfo2.transform != null) //null @
                        {
                            if (hitInfo2.transform.tag == "Location")
                            {
                                DisplayLocation location_script = hitInfo2.transform.GetComponent<DisplayLocation>(); // @
                                int display_index = location_script.location_Num;

                                // #
                                displayManager_script.reset_DisplayArry(display_index);
                            }
                        }
                    }
                    else if (hitInfo.transform.GetComponent<ItemPickUp>().item.itemName == "FlashlightItem")
                    {
                        //Destroy(hitInfo.transform.gameObject); //아이템 삭제, 나중엔 코루틴으로 #

                        // - 손전등(아이템) 비활성화
                        hitInfo.transform.gameObject.SetActive(false);
                        OutlineController.set_enabled(pre_ol_index, false);

                        // - 손전등 기능 on
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
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, item_layerMask))
        {
            if (OutlineController.get_outline_okay())
                return;

            if (hitInfo.transform.tag == "Item") //compare @
            {
                //// - 장애물 검사하기
                //coverCheck = obstacleReader_script.LookAtFrame((int)layerMask);
                //if (coverCheck)
                //{
                //    pickupActivated = false;

                //    if (pre_ol_index != -1)
                //    {
                //        // - 외곽선 해제
                //        OutlineController.set_enabled(pre_ol_index, false);
                //        pre_ol_index = -1;
                //        OutlineController.set_check(false);
                //        outline_active = false;

                //        // - 클릭버튼 해제
                //        actionCaption.SetActive(false);

                //        // - 장애물 검사하기
                //        coverCheck = obstacleReader_script.LookAtFrame((int)layerMask);
                //    }

                //    return;
                //}

                // - 인포
                ItemInfoAppear();

                // - 클릭버튼 활성화
                actionCaption.SetActive(true);

                // - 외곽선
                ItemPickUp pieceItem_script = hitInfo.transform.GetComponent<ItemPickUp>();
                int cur_ol_index = pieceItem_script.outlineIndex;

                // 1
                //if (pre_ol_index != cur_ol_index)
                //{
                //    OutlineController.set_enabled(pieceItem_script.outlineIndex, true);
                //    if (pre_ol_index != -1)
                //        OutlineController.set_enabled(pre_ol_index, false);
                //    pre_ol_index = pieceItem_script.outlineIndex;
                //}
                // 2
                //if (cur_ol_index >= 14 && cur_ol_index != 0) 
                {
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
                if (pre_ol_index != -1)
                {
                    // - 외곽선 해제
                    OutlineController.set_enabled(pre_ol_index, false);
                    pre_ol_index = -1;
                    OutlineController.set_check(false);
                    outline_active = false;

                    // - 클릭버튼 해제
                    actionCaption.SetActive(false);

                    //// - 장애물 검사하기
                    //coverCheck = obstacleReader_script.LookAtFrame((int)layerMask);
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

                //// - 장애물 검사하기
                //coverCheck = obstacleReader_script.LookAtFrame((int)layerMask);
            }
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
            if (hitInfo2.transform.CompareTag("Location")) //compare @
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
                            // - 아이템 활성화

                            // 아이템 코드
                            int doll_itemCode = theInventory.get_ItemCode(use_index);
                            // 장식장 놓을때, 이동
                            int doll_displayIndex = displayManager_script.compareItemCode(doll_itemCode); //인형매니저(2배치퍼즐)에서 아이템 인덱스 얻기
                            displayManager_script.MoveSelectedInputArry(
                                doll_displayIndex,
                                location_script.get_DisplayPosition(),
                                location_script.get_DisplayRotation());
                            location_script.lay_Doll();

                            // - 장식장 비교를 위한 변수

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
                                //tempDoor.SetActive(false); //콜라이더 비활성화
                                //tempStairs.SetActive(false);
                                temp_basement.SetActive(false);

                                DoorOpen_Basement doorAni = tempStairs.GetComponent<DoorOpen_Basement>();
                                doorAni.StartDoorAni();



                                enter_3stage = true;

                                //3스테이지 카메라 스크립트 on
                                actionController_3stage_script.enabled = true;

                                //손전등 없어지기 - 손전등 관련 스크립트 가져오기 
                                //FlashlightItem.SetActive(false);
                                flash_end.FlashLightEnd(1);

                                //웬디 AI on
                                wendyAI_Script.ClearLayoutPuzzle();
                                wendyAI_Script.colliderChange();

                                //지하실 파티클 오픈
                                Ring_Particle.SetActive(true);
                                event_ringParticle.Play();


                                // 외곽선 해제                   
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
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo2, range, location_layerMask)) //layerMask_dlsplay
        {
            if (hitInfo2.transform.CompareTag("Location"))
            {
                //hitInfo2는 장식장 위치
            }
        }
    }

    private bool CheckObstacle()
    {
        // - 장애물 검사하기
        coverCheck = obstacleReader_script.LookAtFrame((int)layerMask);
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
