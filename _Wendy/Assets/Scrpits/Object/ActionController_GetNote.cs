using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ActionController_GetNote : MonoBehaviour
{
    [SerializeField]
    private float range;

    [SerializeField]
    private LayerMask layerMask;

    private RaycastHit hitaction;

    //[SerializeField]
    //private Image actionImage;
    // - 클릭버튼
    [SerializeField]
    private GameObject actionCaption;

    Chestaction cheataction;
    FlodNote clockNote_script;

    // - 쪽지 매니저
    NoteManger notemager;
    //RewardNote_Check noteCheck_script;

    private bool pickupActivated;

    public Text actiontext;

    // RewardNote_Check notecheck_script;
    public GameObject[] moveChest;

    private bool isPopup = false;
    private bool isFirstPage = true;
    private bool isLastPage = false;
    private bool getKey = false;
    bool popupNote = false;

    FirstPersonCamera fpCam_Script;
    Player_HJ player_script;

    private bool AniState = false;

    // - ui
    public GameObject Aim;
    //Vector3 halfScreen;

    // - 외곽선
    //private Camera mainCam;
    private DrawOutline_HJ OutlineController;
    private int pre_ol_index = -1; //이전 아웃라인 인덱스
    private bool outline_active = false;

    // - 장애물, 벽
    ObstacleReader obstacleReader_script;
    bool coverCheck = false; //막고잇으면 TRUE
    int _obstacle_layer;

    RewardNote_Check _pre_note_script = null;

    // - 쪽지 상태 스크립트
    SawNoteNumber note_num_script;

    void Start()
    {
        clockNote_script = GameObject.FindObjectOfType<FlodNote>();
        //noteCheck_script = GameObject.FindObjectOfType<RewardNote_Check>();
        fpCam_Script = Camera.main.GetComponent<FirstPersonCamera>();
        player_script = GameObject.FindObjectOfType<Player_HJ>();
        cheataction = FindObjectOfType<Chestaction>();
        notemager = FindObjectOfType<NoteManger>();
        //  notecheck_script = FindObjectOfType<RewardNote_Check>();
        //  noteCheck_script.enabled = true;

        // 외곽선
        //mainCam = GetComponent<Camera>();
        OutlineController = GameObject.FindObjectOfType<DrawOutline_HJ>();

        //장애물,벽
        obstacleReader_script = GameObject.FindObjectOfType<ObstacleReader>();
        _obstacle_layer = (1 << LayerMask.NameToLayer("NotePage")) + (1 << LayerMask.NameToLayer("Obstacle"));

        // 쪽지 상태 (싱글톤)
        note_num_script = FindObjectOfType<SawNoteNumber>();
    }

    void Update()
    {
        if (!popupNote)
        {
            if (CheckObstacle())
                return;

            Checkaction();
        }

        TryAction();
    }

    private void TryAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!popupNote) // 쪽지를 띄우기전
            {
                Check_Do_action();
            }
            else // 쪽지가 화면에 띄워진 이후에는 쪽지가 다시 되돌아갈 작업만 해야함
            {
                if (_pre_note_script.get_corPossible())
                {
                    //// - 쪽지 매니저
                    //notemager._popup = false; //reset_NoteState 함수에 포함

                    ActionDisappear(); //info 삭제 - 분명 삭제했는데 왜 자꾸 뜬담? 

                    popupNote = false; //팝업 해제

                    _pre_note_script.move_NoteAni();
                    _pre_note_script = null;
                }
            }
        }
    }


    private void Check_Do_action()
    {
        if (hitaction.transform != null)
        {
            //추후에 따로 분리할 예정입니다.
            //if (hitaction.transform.tag == "Action") //compare @
            //{
            //    int Chestnumber = hitaction.transform.parent.GetComponent<Chestaction>().Chest_number;
            //    moveChest[Chestnumber].transform.parent.GetComponent<Chestaction>().Start_action(1);
            //}

            if (hitaction.transform.tag == "Note") //compare @
            {
                //if (AniState == false)
                //{
                //    ActionDisappear(); //info 삭제 - 분명 삭제했는데 왜 자꾸 뜬담? 

                //    hitaction.transform.GetComponent<PageNote>().CheckAddcount(1);

                //    fpCam_Script.enabled = false;
                //    player_script.enabled = false;
                //    Aim.SetActive(false);

                //    DelayPlay();
                //}

                RewardNote_Check noteScript = hitaction.transform.GetComponent<RewardNote_Check>();

                if (noteScript.get_corPossible())
                {
                    // - 쪽지 이동 스크립트
                    _pre_note_script = noteScript;
                    // - 쪽지 매니저
                    notemager._popup = true;
                    // - 쪽지 상태, UI
                    hitaction.transform.GetComponent<PageNote>().CheckAddcount(1);
                    if (note_num_script != null)
                        note_num_script.SetNoteCount();

                    popupNote = true; //팝업 상태

                    ActionDisappear(); //info 삭제 - 분명 삭제했는데 왜 자꾸 뜬담? 

                    fpCam_Script.enabled = false;
                    player_script.SetDeActiveAni();
                    player_script.enabled = false;
                    Aim.SetActive(false);

                    // - 외곽선, 클릭버튼 해제보다 먼저 해야함
                    noteScript.move_NoteAni();

                    // - 외곽선 해제
                    OutlineController.set_enabled(pre_ol_index, false);
                    pre_ol_index = -1;
                    OutlineController.set_check(false);
                    outline_active = false;

                    // - 클릭버튼 비활성화
                    actionCaption.SetActive(false);
                }
            }

            //if (hitaction.transform.CompareTag("Door")) //compare @ //ActionController_Ending 스크립트에 있습니다
            //{
            //    notemager.OpenCondition();
            //}
        }
    }

    bool flipOver = false;

    void DelayPlay()
    {
        StartCoroutine(DelayNoteAni());
    }

    IEnumerator DelayNoteAni()
    {
        AniState = true;

        yield return new WaitForSeconds(0.2f);//3초동안 딜레이

        hitaction.transform.GetComponent<RewardNote_Check>().move_NoteAni();

        yield return new WaitForSeconds(0.2f);//3초동안 딜레이

        AniState = false;
    }


    private void PopUpNote()
    {
        if (flipOver)
            return;
    }

    public void reset_NoteState()
    {
        //책 팝업상태 해제
        isPopup = false; //팝업 상태 

        //카메라, 플레이어 이동 가능
        fpCam_Script.enabled = true;
        player_script.enabled = true;

        Aim.SetActive(true);

        // - 쪽지 매니저
        notemager._popup = false;
    }

    private void Checkaction()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitaction, range, layerMask))
        {
            //if (hitaction.transform.CompareTag("Action")) //compare @
            //{
            //    ActionAppear();
            //}

            if (hitaction.transform.CompareTag("Note")) //compare @
            {
                if (OutlineController.get_outline_okay())
                    return;

                NoteAppear();

                // - 클릭버튼 활성화
                actionCaption.SetActive(true);

                // - 외곽선
                SetOutline setoutlin_script = hitaction.transform.GetComponent<SetOutline>();
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

            //if (hitaction.transform.CompareTag("Door")) //compare @ //ActionController_Ending 스크립트에 있습니다
            //{
            //    DoorAppear();
            //}
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
    private void DoorAppear()
    {
        pickupActivated = true;
        //actiontext.gameObject.SetActive(true);
        //actiontext.text = " 밖으로 나간다";
    }

    private void NoteAppear()
    {
        pickupActivated = true;
        //actiontext.gameObject.SetActive(true);
        //  actiontext.text = " 쪽지 살펴보기 [Click]";
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

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("noteAction"))
    //    {
    //        GameDataManager.instance.AddCount(1);
    //    }


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
