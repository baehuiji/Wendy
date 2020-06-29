using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ChangeCam_2stage : MonoBehaviour
{

    static public ChangeCam_2stage instance;

    [SerializeField]
    private string WendyLaughSound = "Cellar_laughWendy";

    private Camera mainCamera;
    private Camera CellarCamera;
    public GameObject FlashPack;
    public GameObject Target_Player;
    public GameObject Mid_Canva;
    public GameObject OutLineScript;

    public GameObject Set_Camera_position;

    private bool FadeState = false;

    private AudioListener mainListener;
    private AudioListener CellarListener;

    public bool See_Wendy = false;
    private bool FadeIng = false;

    FadeManager Fade_script;
    ActionController_02_VER2 actionController;

    float movetime = 0f;
    float time = 0f;

    private bool checkstage = false;
    public bool CheckStage { get { return checkstage; } }


    public RectTransform Up_Panel;
    public RectTransform Down_Panel;
    public RectTransform Right_Panel;
    public RectTransform Left_Panel;


    private int count = 0;

    //   private bool Fadstate = true;
    //  private bool test = true;

    public Camera flashCamera;
    //public Camera uiCamera;


    public GameObject playerModeling;
    Animator _animator = null;

    bool LaughState = false;

    // - 지하실 문 외곽선
    CellarDoorCollider cellar_script;

    void Start()
    {
        mainCamera = Camera.main;
        CellarCamera = GetComponent<Camera>();

        mainListener = mainCamera.GetComponent<AudioListener>();
        CellarListener = GetComponent<AudioListener>();

        //초기화
        mainListener.enabled = true;
        CellarListener.enabled = false;

        Fade_script = FindObjectOfType<FadeManager>();
        actionController = mainCamera.GetComponent<ActionController_02_VER2>();

        _animator = playerModeling.GetComponent<Animator>();

        // - 지하실문 스크립트, 문 외곽선을 위해
        cellar_script = GameObject.FindObjectOfType<CellarDoorCollider>();

    }



    public void change_Camera(int type)
    {
        if (type == 1) //지하실 카메라 on 
        {
            if (FadeIng == false)
            {

                if (See_Wendy)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        StartCoroutine(CameraFadeIn());
                        type = count;

                    }
                }

                else
                    StartCoroutine(CameraFadeOut(1.6f));

            }

        }

    }


    IEnumerator MoveCameraSee()
    {


        mainCamera.WorldToScreenPoint(new Vector3(-1.428f, 1.218f, -15.728f));


        yield return null;

    }

    // 검은 화면 나오기...
    IEnumerator MoveOutPanel(float duration)
    {
        time = 0f;




        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        // Vector2 EWallEndPos = Wall_E.transform.localPosition + new Vector3(2, 0, 0);
        // 위치를 움직일 위치를 다시 잡습니다! 시작위치랑, 끝위치를 다시 잡습니다!!!! 후유! 

        movetime = duration;

        while (duration > 0.0f) // 선형보간이 진행됩니다. 선형보간의 이동이 끝날때까지! 
        {
            duration -= Time.deltaTime; // 벽 2개가 움직입니다! 천천히 움직입니다!!  현재 5초로 입력했을시 5초동안 움직이는 것을 확인완료했습니다!
            time += Time.deltaTime;

            Up_Panel.anchoredPosition = Vector2.Lerp(Up_Panel.anchoredPosition, new Vector2(0, 590), time / movetime);
            Down_Panel.anchoredPosition = Vector2.Lerp(Down_Panel.anchoredPosition, new Vector2(0, -590), time / movetime);
            Right_Panel.anchoredPosition = Vector2.Lerp(Right_Panel.anchoredPosition, new Vector2(1020, 0), time / movetime);
            Left_Panel.anchoredPosition = Vector2.Lerp(Left_Panel.anchoredPosition, new Vector2(-1020, 0), time / movetime);
            //   Wall_E.transform.position = Vector3.Lerp(Wall_E.transform.position, E_WallStop, t);

            yield return waitForEndOfFrame;
        }


        yield return new WaitForSeconds(1);


        //Mathf.Lerp ( 시작점, 종료점, 거리비율을 받는데 )
        // 시작점에는 오브젝트의 현재 위치를 받고 - 종료점에는 오브젝트 현재 위치 + 10.
        // 다시 시작점에는 오브젝트의 종료 위치를 받고 - 종료점에는 그 시작점의 + 10.


    }


    // 검은 화면 나오기...
    IEnumerator MoveInPanel(float duration)
    {
        time = 0f;

        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        // Vector2 EWallEndPos = Wall_E.transform.localPosition + new Vector3(2, 0, 0);
        // 위치를 움직일 위치를 다시 잡습니다! 시작위치랑, 끝위치를 다시 잡습니다!!!! 후유! 

        movetime = duration;

        while (duration > 0.0f) // 선형보간이 진행됩니다. 선형보간의 이동이 끝날때까지! 
        {
            duration -= Time.deltaTime; // 벽 2개가 움직입니다! 천천히 움직입니다!!  현재 5초로 입력했을시 5초동안 움직이는 것을 확인완료했습니다!
            time += Time.deltaTime;

            Up_Panel.anchoredPosition = Vector2.Lerp(Up_Panel.anchoredPosition, new Vector2(0, 695), time / movetime);
            Down_Panel.anchoredPosition = Vector2.Lerp(Down_Panel.anchoredPosition, new Vector2(0, -695), time / movetime);
            Right_Panel.anchoredPosition = Vector2.Lerp(Right_Panel.anchoredPosition, new Vector2(1120, 0), time / movetime);
            Left_Panel.anchoredPosition = Vector2.Lerp(Left_Panel.anchoredPosition, new Vector2(-1120, 0), time / movetime);
            //   Wall_E.transform.position = Vector3.Lerp(Wall_E.transform.position, E_WallStop, t);

            yield return waitForEndOfFrame;
        }


        yield return new WaitForSeconds(1);


        //Mathf.Lerp ( 시작점, 종료점, 거리비율을 받는데 )
        // 시작점에는 오브젝트의 현재 위치를 받고 - 종료점에는 오브젝트 현재 위치 + 10.
        // 다시 시작점에는 오브젝트의 종료 위치를 받고 - 종료점에는 그 시작점의 + 10.


    }




    // 카메라 변경 기능

    IEnumerator CameraFadeOut(float duration)
    {
        FadeIng = true;
        time = 0f;

        //플레이어 이동 스크립트 끄기
        _animator.SetBool("IsWalking", false);
        Target_Player.gameObject.GetComponent<Player_HJ>().enabled = false;
        mainCamera.gameObject.GetComponent<FirstPersonCamera>().enabled = false;
        OutLineScript.SetActive(false);

        StartCoroutine(MoveOutPanel(4f));
        yield return new WaitForSeconds(0.5f);
        //카메라 확대, 이동이 들어감

        Vector3 SavePoint = mainCamera.transform.position;
        Vector3 SetPoint = Set_Camera_position.transform.position; //카메라 페이드 인 아웃 들어가야함.... 흐음...
                                                                   //  mainCamera.WorldToViewportPoint = new Vector3(-1.428f, 1.218f, -15.728f);
        Vector3 SaveRotationPoint = mainCamera.transform.eulerAngles;
        Vector3 SetRotationPoint = Set_Camera_position.transform.eulerAngles; //카메라 페이드 인 아웃 들어가야함.... 흐음...
        float FieldSave = mainCamera.fieldOfView;

        movetime = duration;
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        //손전등 카메라, 에임 끔
        FlashPack.SetActive(false);
        Mid_Canva.SetActive(false);
        flashCamera.enabled = false;

        while (duration > 0.0f) // 선형보간이 진행됩니다. 선형보간의 이동이 끝날때까지! 
        {
            duration -= Time.deltaTime; // 벽 2개가 움직입니다! 천천히 움직입니다!!  현재 5초로 입력했을시 5초동안 움직이는 것을 확인완료했습니다!
            time += Time.deltaTime;

            if (mainCamera.fieldOfView >= 38)
            {
                mainCamera.fieldOfView -= 0.5f;
                yield return waitForEndOfFrame;
            }

            mainCamera.transform.position = Vector3.Lerp(SavePoint, SetPoint, time / movetime);
            mainCamera.transform.eulerAngles = Vector3.Lerp(SaveRotationPoint, SetRotationPoint, time / movetime);
        }

        Fade_script.FadeOut();


        yield return new WaitForSeconds(2f);

        //메인 카메라/손전등 카메라 끔, 십자선 끔 
        mainCamera.enabled = false;
        mainListener.enabled = false;

        //지하실 카메라 켬
        CellarCamera.enabled = true;
        CellarListener.enabled = true;

        Fade_script.FadeIn();
        yield return new WaitForSeconds(1f);

        LaughState = true;
        DelayWendyLaugh();
       // SoundManger.instance.PlayLoopSound(WendyLaughSound);


        mainCamera.transform.position = SavePoint;
        mainCamera.transform.eulerAngles = SaveRotationPoint;
        mainCamera.fieldOfView = FieldSave;

        See_Wendy = true;

        FadeIng = false;
    }

    void DelayWendyLaugh()
    {

        StartCoroutine(WendySound());
    }


    IEnumerator WendySound()
    {
        if (LaughState)
        {
            while (true)
            {
                SoundManger.instance.PlaySound(WendyLaughSound);
                yield return new WaitForSeconds(5f);

                if(LaughState == false)
                {
                    break;
                }
            }
        }


    }        

    IEnumerator CameraFadeIn()
    {
        FadeIng = true;

        // 페이드 아웃 실행
        Fade_script.FadeOut();
        yield return new WaitForSeconds(2f);

        LaughState = false; 
       // SoundManger.instance.StopEffectSound(WendyLaughSound);


        // 메인 카메라키고 지하실 카메라 끔 , 메인 카메라 켬
        mainCamera.enabled = true;
        CellarCamera.enabled = false;

        Fade_script.FadeIn();
        yield return new WaitForSeconds(1f);

        StartCoroutine(MoveInPanel(4f));

        yield return new WaitForSeconds(1f);


        // 플레이어 움직이는 스크립트 켬
        OutLineScript.SetActive(true);
        Mid_Canva.SetActive(true);
        FlashPack.SetActive(true);
        flashCamera.enabled = true;

        count = 0;
        Target_Player.gameObject.GetComponent<Player_HJ>().enabled = true;
        mainCamera.gameObject.GetComponent<FirstPersonCamera>().enabled = true;
        See_Wendy = false;

        // - 문외곽선 다시 생성 시키기
        cellar_script.set_state(true);

        FadeIng = false;

    }



}
