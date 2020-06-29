using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FramePuzzle_ChangeCam : MonoBehaviour
{
    private Camera mainCamera;
    private Camera fpCamera;

    private AudioListener mainListener;
    private AudioListener fpListener;

    //public GameObject Aim; //카메라깊이를 이용하게 되면서 필요없게됨
    public GameObject _textSub;
    public GameObject _Aim;
    public GameObject _info;


    private Player_HJ playerController;

    private ActionController_02_VER2 actionController;
    private FirstPersonCamera mainCamMove_script;
    private FramePuzzle_Enter puzzleEnter_script;

    private FramePuzzle_Controller fpController;

    void Start()
    {
        mainCamera = Camera.main;
        fpCamera = GetComponent<Camera>();

        mainListener = mainCamera.GetComponent<AudioListener>();
        fpListener = GetComponent<AudioListener>();

        actionController = Camera.main.GetComponent<ActionController_02_VER2>();
        mainCamMove_script = Camera.main.GetComponent<FirstPersonCamera>();

        puzzleEnter_script = Camera.main.GetComponent<FramePuzzle_Enter>();
        fpController = GameObject.FindObjectOfType<FramePuzzle_Controller>();

        playerController = GameObject.FindObjectOfType<Player_HJ>();        
        
        //초기화
        //mainListener.enabled = true;
        //fpListener.enabled = false;
    }

    void Update()
    {

    }

    //0.6
    public void change_Camera(bool b)
    {
        //1. 카메라의 프로젝션설정을 바꾸는것

        //2. 카메라 두개를 사용, camera 기능 on, off
        //- 단 카메라 바꿀때마다 오디오리스너 main카메라것은 off,
        //- 카메라 바꿧을때 플레이어 이동 안 먹히게 하기

        if (b) //액자퍼즐 카메라 on
        {
            Cursor.lockState = CursorLockMode.None; //커서고정 해제 *Confined:화면안

            // - 카메라와 리스너
            fpCamera.enabled = true;
            fpListener.enabled = true;

            mainCamera.enabled = false;
            mainListener.enabled = false;

            // - ui
            _textSub.SetActive(true);
            _Aim.gameObject.SetActive(false);
            _info.SetActive(false);

            // - 메인카메라,플레이어 스크립트 off
            //actionController.enabled = false;
            mainCamMove_script.enabled = false;
            puzzleEnter_script.enabled = false;

            playerController.enabled = false;

            // - 퍼즐 스크립트 on
            fpController.enabled = true;
            fpController.set_state();
        }
        else //if(type == 0) //다시 돌아가기
        {
            Cursor.lockState = CursorLockMode.Locked; //커서 고정

            // - 카메라와 리스너
            mainCamera.enabled = true;
            mainListener.enabled = true;

            fpCamera.enabled = false;
            fpListener.enabled = false;

            // - ui
            _textSub.SetActive(false);
            _Aim.gameObject.SetActive(true);
            //_info.SetActive(true);

            // - 퍼즐 스크립트 off
            fpController.enabled = false;

            // - 메인카메라, 플레이어이동 스크립트 on
            //actionController.enabled = true;
            StartCoroutine(TimetoWaitFor());
        }
    }

    IEnumerator TimetoWaitFor()
    {
        yield return new WaitForSeconds(0.1f);

        playerController.enabled = true;
        //actionController.enabled = true;
        mainCamMove_script.enabled = true;
        puzzleEnter_script.enabled = true;
    }
}
