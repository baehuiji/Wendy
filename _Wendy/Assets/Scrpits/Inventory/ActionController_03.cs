using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ActionController_03 : MonoBehaviour
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
    private DisplayManager_3stage displayManager_script2;
    private RaycastHit hitInfo2;

    private ActionController_02_VER2 actionController_2stage_script;

    RewardDoor_Open doorAnimation;

    // - 책을 습득하기 위한 새로운 스크립트
    //bool end = false;
    ActionController_GetKey getKey_script;

    void Start()
    {
        displayManager_script2 = GameObject.FindObjectOfType<DisplayManager_3stage>();
        actionController_2stage_script = FindObjectOfType<ActionController_02_VER2>();

        actionController_2stage_script.enabled = false;

        doorAnimation = GameObject.FindObjectOfType<RewardDoor_Open>();

        getKey_script = GameObject.FindObjectOfType<ActionController_GetKey>();
    }

    void Update()
    {
        CheckItem();

        //if (end)
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
                            displayManager_script2.reset_DisplayArry(display_index);
                        }
                    }
                }
                //else if (hitInfo.transform.tag == "Book") //compare @
                //{

                //}
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
            InfoDisappear();
    }

    // Need to modify
    private void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득" + " [Click]";
    }
    public void InfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
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
                            displayManager_script2.set_DisplayArry(display_index, doll_itemCode);


                            // - 아이템사용 후, 슬롯 클리어  O
                            theInventory.clear_Slot(use_index);
                        }
                    }
                }

            }
            else if (hitInfo2.transform.tag == "Enter") //compare @
            {
                // Enter 버튼을 누르고,
                if (displayManager_script2.compare_Answer()) // 맞았을때,
                {
                    doorAnimation.play_doorAni();
                    this.enabled = false;

                    getKey_script.enabled = true;
                    //Debug.Log("clear - layout puzzle - 3stage");
                }
                else // 틀렸을때
                {
                    //Debug.Log("wrong answer - layout puzzle - 3stage");
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

        }
    }

    //public void set_ending()
    //{
    //    end = true;
    //    hitInfo2 = null;
    //}
}
