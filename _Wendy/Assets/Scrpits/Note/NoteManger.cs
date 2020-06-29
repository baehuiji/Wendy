using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NoteManger : MonoBehaviour
{
    static public NoteManger instance;

    public GameObject NoteImage;

    public Sprite Note_1;
    public Sprite Note_2;
    public Sprite Note_3;
    public Sprite Note_4;
    public Sprite Note_5;

    private int checkcount = 0;
    public int CheckCount { get { return checkcount; } }

    FirstPersonCamera fpCam_Script;
    Player_HJ player_script;

    public GameObject ConditionPanel;
    public GameObject Aim;

    public bool _popup = false;

    // 영상
    private EndingVideo_Loading loadEnding_script;
    private ActionController_Ending endingContrler_script;

    void Start()
    {
        fpCam_Script = Camera.main.GetComponent<FirstPersonCamera>();
        player_script = GameObject.FindObjectOfType<Player_HJ>();
        NoteImage.gameObject.SetActive(false);
        if (ConditionPanel != null)
            ConditionPanel.SetActive(false);

        // - 재생시켜야할 엔딩 영상
        loadEnding_script = GameObject.FindObjectOfType<EndingVideo_Loading>();

        // - 해제해야할 액션 컨트롤러
        endingContrler_script = GameObject.FindObjectOfType<ActionController_Ending>();
    }

    void Update()
    {

        NotecheckCount();

        // 이미 본 것은 볼 필요없이 수정
        //
    }

    public void OpenCondition()
    {
        if (checkcount != 5)
        {
            // 조건 불만족시 경고창
            Cursor.lockState = CursorLockMode.None; //커서 고정 해제 -- 오옹 신기해요
            ConditionPanel.SetActive(true);

            // 해제해야할 스크립트
            fpCam_Script.enabled = false;
            player_script.enabled = false;
            endingContrler_script.enabled = false;

            // 십자선 제거
            Aim.SetActive(false);
        }
        else
        {
            //Debug.Log("무사히 밖으로 나갔습니다");

            loadEnding_script.InStartFadeAnim(); //쪽지매니저와 합치기
        }
    }

    public void YesClickEvent()
    {
        //Debug.Log("집 밖으로 나갔습니다");
        ConditionPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked; //커서 고정 --오아아앙!

        // - 아래 loadEnding_script 스크립트의 InStartFadeAnim 함수 내용과 곂칠지도
        fpCam_Script.enabled = true;
        player_script.enabled = true;
        endingContrler_script.enabled = true;
        Aim.SetActive(true);

        // - 영상 나오게
        loadEnding_script.InStartFadeAnim(); //쪽지매니저와 합치기
    }

    public void NoClickEvent()
    {
        ConditionPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked; //커서 고정 --오아아앙!

        fpCam_Script.enabled = true;
        player_script.enabled = true;
        endingContrler_script.enabled = true;
        Aim.SetActive(true);
    }

    private void NotecheckCount()
    {
        if (checkcount != 0)
        {
            NoteImage.gameObject.SetActive(true);

        }
        //만약에 노트 check 가 된다면..! check... 음... 흐음...

        if (checkcount == 1)
        {
            NoteImage.GetComponent<Image>().sprite = Note_1;

        }
        if (checkcount == 2)
        {
            NoteImage.GetComponent<Image>().sprite = Note_2;

        }
        if (checkcount == 3)
        {
            NoteImage.GetComponent<Image>().sprite = Note_3;

        }
        if (checkcount == 4)
        {
            NoteImage.GetComponent<Image>().sprite = Note_4;

        }
        if (checkcount == 5)
        {
            NoteImage.GetComponent<Image>().sprite = Note_5;
            //if()
        }

    }
    public void AddCount(int c)
    {
        checkcount++;
    }
}
