using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Cuckoo : MonoBehaviour
{
    [SerializeField]
    private string CrowSound = "CP_crow";

    // - 이동
    private bool movestate = false;
    public Transform startTransform;
    public Transform endTransform;
    private bool moveAniState = false;

    public float journeyTime = 0.8F; //걸리는 시간
    private float startTime;
    public float circularDegree = 0.5f;
    //public float direction = 0.0f; //방향

    public float rotSpeed;
    public float moveSpeed;


    // - 애니메이션
    private Animator animator;
    private static string EventEndAnimationName = "Base Layer.angmu_fly";
    private static string EvenStartAnimationName = "Base Layer.angmu_standby";
    private bool aniState = false;
    //private static string AniName = "cuckoo_dundun";

    // - 애니메이션 이름
    private static string AniName_idle = "angmu_idle";
    private static string AniName_start = "angmu_standby";
    private static string AniName_end = "angmu_fly";

    //X
    private static int AniNameHash; //X
    //private static string AniLayerName_string = "Base Layer";
    //private static string AniName_string = ".cuckoo_dundun";


    // - 활성화가 되어있는지
    private bool active = false;

    // - 카메라 흔들기 / 모션블러 / 카메라 이동 멈추기
    CameraShake cameraShake_script;


    public FirstPersonCamera fpCam_Script;

    //bool boolValue = false;

    // - 문
    DoorAni_cuckoo doorAni_script;

    // - 캐릭터
    Player_HJ player_script;

    // - 
    ClockPuzzle_Manager cpManager_script;
    private Vector3 mainCam_pos = Vector3.zero; // to
    private Vector3 ckstart_pos = Vector3.zero; // from : startTransform 의 pos
                                                //private Vector3 hit_position = Vector3.zero; // XXX필요없음 : to : 레이케스트 : 레이와 부딪친 hitinfo의 접촉점

    public Vector3 magicVec = new Vector3(0f, 1f, 0f);

    bool once = false;

    private float journeyLength;


    void Start()
    {
        animator = GetComponent<Animator>();

        //start_cuckooAni();

        cameraShake_script = Camera.main.GetComponent<CameraShake>();
        fpCam_Script = Camera.main.GetComponent<FirstPersonCamera>();

        //0
        //public UnityStandardAssets.CinematicEffects.MotionBlurEditor motionBlur_script;
        //UnityStandardAssets.CinematicEffects.MotionBlurEditor motionBlur = Camera.main.GetComponent<MotionBlur>();
        //1
        //motionBlur_script = GetComponent<MotionBlur>();
        //GUI.enabled;
        //2
        //if (boolValue == false)
        //{
        //    base.OnInspectorGUI();
        //    return;
        //}

        doorAni_script = GameObject.FindObjectOfType<DoorAni_cuckoo>();

        player_script = GameObject.FindObjectOfType<Player_HJ>();

        cpManager_script = GameObject.FindObjectOfType<ClockPuzzle_Manager>();

        //AniNameHash = animator.StringToHash(EventEndAnimationName);

        journeyLength = Vector3.Distance(startTransform.position, endTransform.position);
    }

    //void Update()
    //{

    //}

    public void start_cuckooAni()
    {
        if (active)
            return;

        active = true;
        doorAni_script.set_Ani_param(true);

        // - 카메라와 캐릭터 이동 해제
        //fpCam_Script.start_JumpScare();
        //player_script.set_cp_start(true);
        fpCam_Script.enabled = false;
        player_script.enabled = false;

        // - 버튼을 각도 구하기
        mainCam_pos = cpManager_script.get_mainCam_pos();
        ckstart_pos = startTransform.position; //from
        //hit_position = cpManager_script.get_hitinfo_pos(); //to

        float angle = CalculateAngle(ckstart_pos, mainCam_pos);
        if (angle >= 200) // 플레이어가 오른쪽에 있을때
        {
            magicVec = new Vector3(-1, 1, 0);
        }
        else if (angle <= 160)// 왼쪽에 있을때
        {
            magicVec = new Vector3(1, 1, 0);
        }
        else
        {
            magicVec = new Vector3(0, 1, 0);
        }

        //Debug.Log(angle.ToString());

        StartCoroutine(cuckoo_come_camera_front(true));
    }


    IEnumerator cuckoo_come_camera_front(bool d)
    {
        moveAniState = true;

        //이동에 필요한것 초기화
        Vector3 offset;
        float sqrLen = 0.1f;

        startTime = Time.time;
        float closeDistance = 0.05f;


        if (d) // end 지점으로 갈때
        {
            StartCoroutine(check_aniState());
            while (true)
            {
                // - 이동
                offset = transform.position - endTransform.position;
                sqrLen = offset.sqrMagnitude;

                Vector3 center = (transform.position + endTransform.position) * circularDegree; // 1.2F; //0.5f //포물선 원형정도, start와 end 사이의 거리가 멀수록 큰 값을 넣어야함
                center -= magicVec; // new Vector3(0, 1, 0); //매직벡터사용
                Vector3 startRelCenter = transform.position - center;
                Vector3 setRelCenter = endTransform.position - center;

                //Slerp을 지정시간 이동조절 쓰는법
                float distCovered = (Time.time - startTime) * moveSpeed;
                float fracJourney = distCovered / journeyLength;

                transform.position = Vector3.Slerp(startRelCenter, setRelCenter, fracJourney);
                transform.position += center;

                // - 회전
                float step = rotSpeed * Time.deltaTime;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, endTransform.rotation, step);

                yield return new WaitForSeconds(0.01f);

                if (sqrLen <= closeDistance * closeDistance) //0f
                {
                    //StartCoroutine(check_aniState());
                    break;
                }
            }

        }
        else // start 지점으로 갈떄
        {
            while (true)
            {
                // - 이동
                offset = transform.position - startTransform.position;
                sqrLen = offset.sqrMagnitude;

                Vector3 center = (transform.position + startTransform.position) * circularDegree; // 1.2F; //0.5f //포물선 원형정도, start와 end 사이의 거리가 멀수록 큰 값을 넣어야함
                center -= magicVec; // new Vector3(0, 1, 0);
                Vector3 startRelCenter = transform.position - center;
                Vector3 setRelCenter = startTransform.position - center;

                float distCovered = (Time.time - startTime) * moveSpeed;
                float fracJourney = distCovered / journeyLength;

                transform.position = Vector3.Slerp(startRelCenter, setRelCenter, fracJourney); //2
                transform.position += center;

                // - 회전
                float step = moveSpeed * Time.deltaTime;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, startTransform.rotation, step);

                yield return new WaitForSeconds(0.01f);

                if (sqrLen <= closeDistance * closeDistance) //0f
                {
                    //fpCam_Script.end_JumpScare(); //# -> doorAni_script 스크립트로 옮김
                    doorAni_script.set_Ani_param(false);
                    doorAni_script.check_state();

                    break;
                }
            }

            //active = false; //# -> doorAni_script 스크립트로 옮김
            cpManager_script.set_popup_anmu(false);
        }

        moveAniState = false;
    }

    IEnumerator crowSoundStart()
    {
        // - 앵무새 애니메이션 시작
        yield return new WaitForSeconds(2.6f);

        SoundManger.instance.PlaySound(CrowSound); // 이건 약간 3초 뒤에 하거나 해야하는 걸로 안다. 


    }


    IEnumerator check_aniState()
    {
        // - 앵무새 애니메이션 시작
        aniState = true;


        // - 카메라 흔들기 (최초1회 실행)
        if (!once)
        {
            StartCoroutine(crowSoundStart());

            animator.Play(AniName_start, 0, 0f);

            //animator.SetBool("IsStandby", true); //O
            
            Camera.main.GetComponent<UnityStandardAssets.CinematicEffects.MotionBlur>().enabled = true;
            cameraShake_script.set_CameraShake();

            while (!animator.GetCurrentAnimatorStateInfo(0).IsName(EvenStartAnimationName)) //nameHash == AniNameHash -> O
            {
                yield return null;
            }
            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.6f) // exitTime = 0.99f, normalizedTimesms 0~1사이 값
            {
                // - 애니메이션 재생 중
                yield return null;
            }
            //animator.SetBool("IsStandby", false);//O
            //set_AniBool(aniState);//O

            //animator.Play(AniName_end, 0, 0f);

            ////    animator.Play(AniName, 0, 0.4f);
            animator.Play(AniName_end, 0, 0.38f);

        }
        else
        {
            //    //set_AniBool(aniState);
            animator.Play(AniName_end, 0, 0.28f);
        }


        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(EventEndAnimationName))
        {
            yield return null;
        }
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.85f) // exitTime = 0.99f, normalizedTimesms 0~1사이 값
        {
            // - 애니메이션 재생 중
            yield return null;
        }

        float endtime = 0.15f;
        float deltat = 0.0f;
        animator.speed = 0; //멈춤
        while (true)
        {
            deltat += Time.deltaTime;

            if (deltat >= endtime)
            {
                animator.speed = 1;
                break;
            }

            yield return null;
        }

        // - 카메라 흔들기 : 모션블러 해제 / 애니메이션 시간 바꾸기 
        StartCoroutine(cuckoo_come_camera_front(false));

        // - 애니메이션 완료 후, idle로 돌아가기
        aniState = false;

        if (!once)
        {

            once = true;
            Camera.main.GetComponent<UnityStandardAssets.CinematicEffects.MotionBlur>().enabled = false;
            
        //animator.SetFloat("aniStartTime", 0.4f); // 0.3 0.4
        }
        //else
        //{
        //    //animator.Play(AniName_idle, 0, 0.0f);

        //    //set_AniBool(aniState);//O
        //}

        animator.Play(AniName_idle, 0, 0.0f);

    }

    public void set_AniBool(bool btemp)
    {
        //animator.SetBool("IsWrongAnswer", btemp);

        //if (btemp && once)
        //{
        //    animator.Play(AniName, 0, 0.4f);
        //}

        animator.SetBool("IsWrongAnswer", btemp);
        //if (btemp)
        //{
        //    //if (!once)
        //        animator.SetBool("IsStandby", btemp);
        //    else
        //}
    }

    public void set_active()
    {
        active = false;
        //fpCam_Script.end_JumpScare();
        //player_script.set_cp_start(false);
        fpCam_Script.enabled = true;
        player_script.enabled = true;
    }

    // - 두점 사이 각도 : 앵무새 회전하면서 들어가는것때문에
    public static float CalculateAngle(Vector3 from, Vector3 to)
    {
        //transform.rotation = Quaternion.FromToRotation(Vector3.up, transform.forward);
        //return Quaternion.FromToRotation(from - mainCam_pos, to - mainCam_pos).eulerAngles.z;
        return Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.z;
    }
}
