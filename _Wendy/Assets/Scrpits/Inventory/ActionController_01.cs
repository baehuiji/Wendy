/////////////////////Inventory action/////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class ActionController_01 : MonoBehaviour
{
    public Inventory theInventory;

    [SerializeField]
    private float range = 10f; // 충돌 체크 구의 반경
    [SerializeField]
    private float length = 10f; // 충돌 체크의 최대 거리 

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private GameObject target;

    [SerializeField]
    private GameObject P_target;//타겟 발 오브젝트와 연결된 pivot

    public float angleRange = 180f;
    public float distance = 5f;
    public bool isCollision = false;
    Color _blue = new Color(0f, 0f, 1f, 0.2f);
    Color _red = new Color(1f, 0f, 0f, 0.2f);
    Vector3 direction;
    float dotValue = 0f;


    [SerializeField]
    //private Text actionText;
    private Image actionImage;

    /// acquire true - false 
    public bool pickupActivated = false;
    private RaycastHit hitInfo;

    // item event

    // item event count


    //- 외곽선
    private Camera mainCam;

    private bool PuzzleOn = false;
    private int BlockCount = 0;
    public int UseCount = 0;

    private DrawOutline_HJ OutlineController;
    private int pre_ol_index = 0; //이전 아웃라인 인덱스

    // - 퍼즐카메라로 이동
    private GameObject spotlight; //이미지
    public Text puzzleText;
    FadeAni_text text_script;
    public Image puzzleImage;
    FadeAni_guide guide_script;
    GuideCaption_Controller guideController_script;

    SelectSlot selectSlot_script;
    public GameObject puzzleKey;

    ChangeCam_1stage ChangeCam_script;

    BoxOpen boxOpen_script;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;

        mainCam = GetComponent<Camera>();//Camera.main;
        OutlineController = GameObject.FindObjectOfType<DrawOutline_HJ>();
        //layerMask = 1 << LayerMask.NameToLayer("Item");

        selectSlot_script = GameObject.FindObjectOfType<SelectSlot>();

        text_script = puzzleText.gameObject.GetComponent<FadeAni_text>();
        guide_script = puzzleImage.gameObject.GetComponent<FadeAni_guide>();
        guideController_script = puzzleImage.gameObject.GetComponent<GuideCaption_Controller>();

        ChangeCam_script = GameObject.FindObjectOfType<ChangeCam_1stage>();

        boxOpen_script = GameObject.FindObjectOfType<BoxOpen>();
    }

    void Update()
    {
        //if (ChangeCam_script.get_PuzzlPlay())
        //    return;

        CheckItem();
        TryAction();
    }

    private void OnDrawGizmos()  //OnDrawGizmosSelected()
    {
        Handles.color = isCollision ? _red : _blue;
        Handles.DrawSolidArc(target.transform.position, Vector3.up, target.transform.forward, angleRange / 2, distance);
        //                     타겟에의 위치에서, 타겟의 위치 앞전방으로,위아래 판별, 각도는 몇만큼, 방향은 몇만큼
        Handles.DrawSolidArc(target.transform.position, Vector3.up, target.transform.forward, -angleRange / 2, distance);
        //                     타겟에의 위치에서, 타겟의 위치 앞전방으로 ,위아래 판별, 각도는 몇만큼의 -, 방향은 몇만큼. 

        Gizmos.DrawSphere(P_target.transform.position, range);
    }

    private void TryAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //CheckItem();
            CanPickUp();
        }
    }

    private void CanPickUp()
    {
        if (pickupActivated)
        {
            if (hitInfo.transform != null)
            {
                if (hitInfo.transform.CompareTag("Item"))
                {
                    int hit_itemCode = hitInfo.transform.GetComponent<ItemPickUp>().item.itemCode;

                    // - 습득
                    if (hit_itemCode == 110) //퍼즐조각 일때
                    {
                        //Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + "획득했습니다");
                        Item hit_item = hitInfo.transform.GetComponent<ItemPickUp>().item;

                        // - 퍼즐조각 수집 개수 증가
                        BlockCount++;
                        if (BlockCount == 1)
                        {
                            guideController_script.change_sprite(1);
                            guide_script.InStartFadeAnim();
                        }
                        else
                        {
                            guideController_script.change_sprite(3);
                            guide_script.InStartFadeAnim();
                        }

                        // - 퍼즐조각 습득
                        theInventory.AcquireItem(hit_item);

                        // - 외곽선 없애기
                        hitInfo.transform.gameObject.SetActive(false);
                        OutlineController.set_enabled(pre_ol_index, false);
                        //OutlineController.set_destroy(pre_ol_index); //필요없을수도

                        // - info 사라짐
                        InfoDisappear();
                    }

                    // - 사용
                    else if (hit_itemCode == 111) //조각 배치
                    {
                        if (theInventory.IsVoid_Slot(selectSlot_script.get_index()))
                        {
                            //// - 텍스트 출력
                            //puzzleText.text = "퍼즐조각이 필요하다";
                            ////페이드아웃
                            //text_script.InStartFadeAnim();

                            guideController_script.change_sprite(2);
                            guide_script.InStartFadeAnim();

                            return;
                        }

                        int select_itemCode = theInventory.get_ItemCode(selectSlot_script.get_index());

                        // - 퍼즐 배치
                        if (select_itemCode == 110)
                        {
                            //// - 텍스트 출력
                            //puzzleText.text = "퍼즐조각을 배치했다";
                            ////페이드아웃
                            //text_script.InStartFadeAnim();

                            UseCount++;

                            // - 사용
                            Item selectItem = theInventory.get_ItemInfo(selectSlot_script.get_index());
                            theInventory.RemoveSlot(selectItem);
                            if (UseCount == 1)
                            {

                            }
                            else if (UseCount >= 5)
                            {
                                // - 퍼즐조각을 다 모았을때, 퍼즐 클릭가능한 상태가 된다
                                PuzzleOn = true;

                                // - info

                                // - 외곽선 , 빈퍼즐블럭 없애기
                                hitInfo.transform.gameObject.SetActive(false);
                                OutlineController.set_enabled(pre_ol_index, false);
                                puzzleKey.gameObject.SetActive(true);
                            }
                        }
                        else
                        {
                            //// - 텍스트 출력
                            //puzzleText.text = "퍼즐조각이 필요하다";
                            ////페이드아웃
                            //text_script.InStartFadeAnim();

                            guideController_script.change_sprite(2);
                            guide_script.InStartFadeAnim();
                        }
                    }
                    else if (hit_itemCode == 112) //퍼즐 완
                    {
                        // - 카메라 이동
                        ChangeCam_script.change_Camera(1);

                        // - info 없애기
                        InfoDisappear();
                        //text_script.stop_coroutine();
                        guide_script.stop_coroutine();
                    }
                    if (hit_itemCode == 113) //상자 일때
                    {
                        // - 콜라이더 비활성화
                        hitInfo.collider.enabled = false;

                        // - 외곽선 없애기
                        //hitInfo.transform.gameObject.SetActive(false);
                        OutlineController.set_enabled(pre_ol_index, false);

                        // - info 없애기
                        InfoDisappear();

                        // - 상자 애니메이션
                        boxOpen_script.set_aniBool();
                    }
                }
            }
        }
    }

    private void CheckItem()
    {
        if (Physics.SphereCast(P_target.transform.position, range, Vector3.up, out hitInfo, length, layerMask))
        {
            //                 레이저 발사 위치            , 구의 반경, 발사 방향,      충돌 결과,     최대거리, 레이어마스크

            // = 외곽선 =
            ItemPickUp pieceItem_script = hitInfo.transform.GetComponent<ItemPickUp>();

            if (CheckAllSectorform(hitInfo)) //hitInfo가 범위 안에 있으면 true
            {
                // - info 띄우기
                ItemInfoAppear(pieceItem_script.item);

                // - 외곽선
                OutlineController.set_enabled(pieceItem_script.outlineIndex, true);
                pre_ol_index = pieceItem_script.outlineIndex;
            }
            else
            {
                // - info 사라지게
                InfoDisappear();

                // - 외곽선 사라짐
                OutlineController.set_enabled(pre_ol_index, false);

                isCollision = false;
            }
        }
        else
        {
            InfoDisappear();
            isCollision = false;
            OutlineController.set_enabled(pre_ol_index, false);
        }
    }

    private bool CheckAllSectorform(RaycastHit tempHit) //범위안에 드는지 검사
    {
        dotValue = Mathf.Cos(Mathf.Deg2Rad * (angleRange / 2));
        direction = tempHit.transform.position - target.transform.position;
        if (direction.magnitude < distance)
        {
            if (Vector3.Dot(direction.normalized, target.transform.forward) > dotValue)
            {
                isCollision = true;
                return true;
            }
            else
            {
                isCollision = false;
                return false;
            }
        }
        else
        {
            isCollision = false;
            return false;
        }
    }

    // Need to modify
    private void ItemInfoAppear(Item tempItem)
    {
        pickupActivated = true;
        actionImage.gameObject.SetActive(true);
        //actionText.gameObject.SetActive(true);

        //int temp_IC = tempItem.itemCode;

        //if (temp_IC == 110) //Puzzle Block Piece
        //{
        //    actionText.text = tempItem.itemName + "  획득" + " [Click]";
        //}
        //else if (temp_IC == 111) //Door Block Piece
        //{
        //    actionText.text = "블럭 배치" + " [Click]";
        //}
        //else if (temp_IC == 112)
        //{
        //    actionText.text = "퍼즐 풀기" + " [Click]";
        //}
        //else if (temp_IC == 113)
        //{
        //    actionText.text = "상자 열기" + " [Click]";
        //}
    }

    public void InfoDisappear()
    {
        pickupActivated = false;
        actionImage.gameObject.SetActive(false);
        //actionText.gameObject.SetActive(false);
    }
}
