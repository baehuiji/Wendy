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

    //[SerializeField]
    //private Text actionText;
    public GameObject actionCaption;

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
    public bool getKey = false;

    private bool possibleBookFlip = false;
    bool flipOver = false;

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
    //public LightOn_3stage _lightOn_script; // 열쇠를 얻으면 현관쪽이 밝아짐 : 기획에 없는 기능

    // - 외곽선
    private DrawOutline_HJ OutlineController;
    private int pre_ol_index = -1; //이전 아웃라인 인덱스
    private bool outline_active = false;

    // - 장애물, 벽
    ObstacleReader obstacleReader_script;
    bool coverCheck = false; //막고잇으면 TRUE
    int _obstacle_layer;

    public PageNote pageNote_script; // 엔딩책의 PageNote

    // - 쪽지 매니저
    NoteManger notemager;

    void Start()
    {
        endingBook_script = GameObject.FindObjectOfType<RewardBook_Open>();

        fpCam_Script = Camera.main.GetComponent<FirstPersonCamera>();
        player_script = GameObject.FindObjectOfType<Player_HJ>();

        eB_colider.enabled = true;
        endingBook_script.enabled = true;

        endingCtrller_script = GameObject.FindObjectOfType<ActionController_Ending>();

        //라이트
        //_lightOn_script = GameObject.FindObjectOfType<LightOn_3stage>();

        //string[] res = UnityStats.screenRes.Split('x');
        //Debug.Log(int.Parse(res[0]) + " " + int.Parse(res[1]));
        //Vector3 halfScreen;

        // 외곽선
        OutlineController = GameObject.FindObjectOfType<DrawOutline_HJ>();

        //장애물,벽
        obstacleReader_script = GameObject.FindObjectOfType<ObstacleReader>();
        _obstacle_layer = (1 << LayerMask.NameToLayer("Item")) + (1 << LayerMask.NameToLayer("Obstacle"));

        // 쪽지 매니저
        notemager = FindObjectOfType<NoteManger>(); //-> 해당 스크립트는 쪽지 매니저 검사 안해도 댐
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
            //if (!outline_active) //-> 이 코드 없어야함, FramePuzzle_Enter와 마찬가지로 외곽선부분이 곂치는게 없음
            //    return;

            // - 책 팝업 상태에서 페이지 넘기기
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
                if (OutlineController.get_outline_okay())
                    return;

                if (hitInfo.transform.tag == "Book_EB") //compare @
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
                if (isLastPage && !flipOver)
                {
                    if (OutlineController.get_outline_okay())
                        return;

                    if (hitInfo.transform.tag == "Key_EB") //compare @
                    {
                        getKey = true;
                        InfoAppear();

                        // - 클릭버튼 활성화
                        actionCaption.SetActive(true);

                        // - 외곽선
                        ItemPickUp pieceItem_script = hitInfo.transform.GetComponent<ItemPickUp>();
                        int cur_ol_index = pieceItem_script.outlineIndex;

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
                    else // 여기 안거침
                    {
                        getKey = false;
                    }
                }
                else
                {
                    getKey = false;
                }
            }
            else
            {
                getKey = false;

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
    }

    private void CanPickUp()
    {
        if (pickupActivated)
        {
            if (hitInfo.transform != null)
            {
                if (hitInfo.transform.tag == "Book_EB") //compare @
                {
                    // - 쪽지상태, 카운트 늘리기
                    pageNote_script.CheckAddcount(1);
                    // - 쪽지 매니저
                    notemager._popup = true;

                    // - 외곽선 해제
                    OutlineController.set_enabled(pre_ol_index, false);
                    pre_ol_index = -1;
                    OutlineController.set_check(false);
                    outline_active = false;

                    // - 클릭버튼 비활성화
                    actionCaption.SetActive(false);

                    // - 책 팝업
                    endingBook_script.move_BookAni();

                    //카메라, 플레이어 이동 불가 
                    fpCam_Script.enabled = false;
                    player_script.SetDeActiveAni();
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
                            //hitInfo.transform.GetComponent<PageNote>().CheckAddcount(1); 

                            // - 아이템 습득
                            hitInfo.transform.gameObject.SetActive(false); //아이템 비활성화

                            // - 외곽선 해제
                            OutlineController.set_enabled(pre_ol_index, false);
                            pre_ol_index = -1;
                            OutlineController.set_check(false);
                            outline_active = false;

                            // - 클릭버튼 비활성화
                            actionCaption.SetActive(false);

                            //
                            getKey = false;
                            InfoDisappear(); //info 삭제

                            // - 현관 라이트 켜기
                            //_lightOn_script.LightOnFront();

                            // - 엔딩 활성화
                            endingCtrller_script.enabled = true;
                        }
                    }
                }
            }
        }
    }

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

                                // - 쪽지 매니저
                                notemager._popup = false; 
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
