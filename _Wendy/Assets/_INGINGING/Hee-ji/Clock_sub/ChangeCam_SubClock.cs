using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCam_SubClock : MonoBehaviour
{
    private Camera _mainCamera;
    private Camera _subclockCamera;
    public Camera _FlashCamera;

    private AudioListener _mainListener;
    private AudioListener _subclockListener;

    private Player_HJ playerController;

    private ActionController_02_VER2 actionController;
    private FirstPersonCamera mainCamMove_script;

    private FramePuzzle_Controller fpController;
    //temp
    public GameObject _Aim;
    public GameObject _info;

    private MakeClockSee MakeClockSee_script;
    private SeeingSubClock SeeingClock_script;

    void Start()
    {
        _mainCamera = Camera.main;
        _subclockCamera = GetComponent<Camera>();

        _mainListener = _mainCamera.GetComponent<AudioListener>();
        _subclockListener = GetComponent<AudioListener>();

        playerController = GameObject.FindObjectOfType<Player_HJ>();

        actionController = Camera.main.GetComponent<ActionController_02_VER2>();
        mainCamMove_script = Camera.main.GetComponent<FirstPersonCamera>();

        //시계보기관련 스크립트
        MakeClockSee_script = GameObject.FindObjectOfType<MakeClockSee>();
        SeeingClock_script = GameObject.FindObjectOfType<SeeingSubClock>();
    }

    void Update()
    {

    }

    //0.6
    public void change_Camera(bool select)
    {
        if (select) //서브시계카메라 카메라 on
        {
            Cursor.lockState = CursorLockMode.None; //커서고정 해제 *Confined:화면안

            // - 카메라와 리스너
            _subclockCamera.enabled = true;
            _subclockListener.enabled = true;

            _mainCamera.enabled = false;
            _FlashCamera.enabled = false;
            _mainListener.enabled = false;

            // - ui
            _Aim.gameObject.SetActive(false);
            _info.SetActive(false);

            // - 메인카메라,플레이어 스크립트 off
            actionController.enabled = false;
            mainCamMove_script.enabled = false;

            playerController.enabled = false;

            // - 서브시계 보게만드는 스크리트 off, 보기 스크립트 on
            MakeClockSee_script.enabled = false;
            SeeingClock_script.enabled = true;
        }
        else //if(type == 0) //다시 돌아가기
        {
            Cursor.lockState = CursorLockMode.Locked; //커서 고정

            // - 카메라와 리스너
            _mainCamera.enabled = true;
            _FlashCamera.enabled = true;
            _mainListener.enabled = true;

            _subclockCamera.enabled = false;
            _subclockListener.enabled = false;

            // - ui
            _Aim.gameObject.SetActive(true);
            //_info.SetActive(true);

            // - 퍼즐 스크립트 off
            SeeingClock_script.enabled = false;

            // - 메인카메라, 플레이어이동 스크립트 on
            //actionController.enabled = true;
            StartCoroutine(TimetoWaitFor());
        }
    }

    IEnumerator TimetoWaitFor()
    {
        yield return new WaitForSeconds(0.1f);

        actionController.enabled = true;
        mainCamMove_script.enabled = true;
        playerController.enabled = true;

        MakeClockSee_script.enabled = true;
    }
}
