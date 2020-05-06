using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ActionController_GetKey : MonoBehaviour
{
    public Inventory theInventory;

    [SerializeField]
    private float range;

    [SerializeField]
    private LayerMask layerMask;
    //public LayerMask layerMask_Ending;

    [SerializeField]
    private Text actionText;

    /// acquire true - false 
    private bool pickupActivated = false;//false;

    private RaycastHit hitInfo;
    private RaycastHit hitInfo2;

    // - 책
    public GameObject endingBook; // 팝업되는 책
    RewardBook_Open endingBook_script;
    int pageNum = 0;

    public Collider eB_colider; // 놓여진 책

    private bool isPopup = false;
    private bool isFirstPage = true;
    private bool isLastPage = false;
    private bool getKey = false;

    private bool possibleBookFlip = false;

    //
    FirstPersonCamera fpCam_Script;
    Player_HJ player_script;

    // - 열쇠 사용을 위한 선택슬롯
    public SelectSlot selectSlot_script;

    // - ui
    public GameObject Aim;
    Vector3 halfScreen;

    private ActionController_Ending endingCtrller_script;

    // - 현관 라이트
    public LightOn_3stage _lightOn_script;

    void Start()
    {
        endingBook_script = GameObject.FindObjectOfType<RewardBook_Open>();

        fpCam_Script = Camera.main.GetComponent<FirstPersonCamera>();
        player_script = GameObject.FindObjectOfType<Player_HJ>();

        eB_colider.enabled = true;
        endingBook_script.enabled = true;

        endingCtrller_script = GameObject.FindObjectOfType<ActionController_Ending>();

        //라이트
        _lightOn_script = GameObject.FindObjectOfType<LightOn_3stage>();

        //string[] res = UnityStats.screenRes.Split('x');
        //Debug.Log(int.Parse(res[0]) + " " + int.Parse(res[1]));
        //Vector3 halfScreen;
    }

    void Update()
    {
        CheckHit();

        TryAction();
    }

    private void TryAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // - 책 팝업, 넘기기
            PopUpBook();

            // - 책 / 열쇠 아이템 습득 
            CanPickUp();
        }
    }

    private void CheckHit()
    {
        if (!isPopup)
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
            {
                if (hitInfo.transform.tag == "Book_EB") //compare @
                {
                    InfoAppear();
                }
            }
            else
                InfoDisappear();
        }
        else
        {
            Vector3 worldPosition = Input.mousePosition;

            if (worldPosition.x >= Screen.width / 2)
            {
                possibleBookFlip = true;
            }
            else
            {
                possibleBookFlip = false;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo, range, layerMask))
            {
                if (isLastPage)
                {
                    if (hitInfo.transform.tag == "Key_EB") //compare @
                    {
                        getKey = true;
                        InfoAppear();
                    }
                }
                else
                    getKey = false;
            }
        }
    }

    private void CanPickUp()
    {
        if (pickupActivated)
        {
            if (hitInfo.transform != null)
            {
                if (hitInfo.transform.tag == "Book_EB") //compare @
                {
                    // - 책 팝업
                    endingBook_script.move_BookAni();

                    //카메라, 플레이어 이동 불가 
                    fpCam_Script.enabled = false;
                    player_script.enabled = false;

                    //책 팝업상태로 바꾸기
                    //eB_colider.enabled = false; //hitInfo.collider.enabled = false; -> RewardBook_Open으로 기능 옮김
                    isPopup = true;
                    InfoDisappear(); //info 삭제

                    //에임 없애기
                    Aim.SetActive(false);
                }

                if (isLastPage && !flipOver)
                {
                    if (hitInfo.transform.tag == "Key_EB") //compare @
                    {
                        if (theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item))
                        {
                            // - 아이템 습득
                            Destroy(hitInfo.transform.gameObject); //아이템 삭제

                            //
                            getKey = false;
                            InfoDisappear(); //info 삭제

                            // - 현관 라이트 켜기
                            _lightOn_script.LightOnFront();

                            // - 엔딩 활성화
                            endingCtrller_script.enabled = true;
                        }
                    }
                }
            }
        }
    }
    bool flipOver = false;

    private void PopUpBook()
    {
        if (flipOver)
            return;

        if (possibleBookFlip)
        {
            //if (hitInfo.transform != null)
            {
                //if (hitInfo.transform.tag == "Filp_EB") //compare @
                {
                    // - 책 넘기기
                    if (isFirstPage)  // 오픈
                    {
                        // 책 펴기
                        if (endingBook_script.play_BookAni(1))
                        {
                            pageNum++;

                            isFirstPage = false;
                        }
                    }
                    else
                    {
                        if (pageNum == 1)
                        {
                            // 책 넘기기
                            if (endingBook_script.play_BookAni(3))
                            {
                                pageNum++; //한번만 실행하기 위해
                            }
                        }
                        else
                        {
                            if (getKey) //열쇠를 얻을 수 있는 상태일땐, 열쇠 우선
                            {
                                return;
                            }

                            // 책 덮기
                            if (endingBook_script.play_BookAni(2))
                            {
                                flipOver = true;
                            }

                            //에임 나타나기
                            //Aim.SetActive(true);
                        }
                    }
                }
            }
        }
    }

    public void reset_BookState()
    {
        //책 팝업상태 해제
        isPopup = false;
        isFirstPage = true;
        isLastPage = false;

        pageNum = 0;

        //카메라, 플레이어 이동 가능
        fpCam_Script.enabled = true;
        player_script.enabled = true;

        Aim.SetActive(true);

        //책 덮기 애니메이션 중복 막기
        flipOver = false;
    }
    public void set_isLastPage()
    {
        isLastPage = true;
    }

    // Need to modify
    private void InfoAppear()
    {
        pickupActivated = true;
        //actionText.gameObject.SetActive(true);
        //actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득" + " [Click]";
    }
    public void InfoDisappear()
    {
        pickupActivated = false;
        //actionText.gameObject.SetActive(false);
    }

}
