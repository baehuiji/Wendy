using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewNote_Ani_02 : MonoBehaviour
{
    public Transform endTrans;
    public Transform startTrans;

    public bool popup = false; //완벽히 띄어져있는가
    private bool play = false; //애니메이션 플레이 상태

    private Coroutine coroutine;
    private Coroutine cAni;
    public bool stateC = false; //코루틴이 한번만 실행되도록

    // - 애니메이션
    public Animator animator;
    private static string EventOpenAnimationName = "Base Layer.paper_unfold";
    private static string AniName_unfold = "paper_unfold";

    public float moveSpeed;
    public float rotSpeed;
    public float moveSpeed_return;
    public float rotSpeed_return;
    private float speedFactor = 0.0f; //보정값
    public float customFactor;

    // - 해제해야할 스크립트
    public FirstPersonCamera CamMove_Script;
    public Player_HJ Player_script;

    private FlodNote clockNote_script; // 외곽선 떄문에, 모델링 교체를 위해서
    private ActionController_TestNote clockNoteCtrler_script;

    void Start()
    {
        animator = GetComponent<Animator>();

        CamMove_Script = Camera.main.GetComponent<FirstPersonCamera>();
        Player_script = GameObject.FindObjectOfType<Player_HJ>();

        clockNote_script = GameObject.FindObjectOfType<FlodNote>();

        clockNoteCtrler_script = GameObject.FindObjectOfType<ActionController_TestNote>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartAni_Note() //이동
    {
        if (stateC)
        {
            return;
        }

        // - 스크립트 해제
        Player_script.SetDeActiveAni();
        Player_script.enabled = false;
        CamMove_Script.enabled = false;

        coroutine = StartCoroutine(MoveNote_Start());
    }
    public bool OpenAni_Note() //애니메이션
    {
        if (stateC)
        {
            return false;
        }

        coroutine = StartCoroutine(OpenNote_Start());

        return true;
    }
    public bool EndAni_Note() //이동
    {
        if (stateC)
        {
            return false;
        }

        coroutine = StartCoroutine(MoveNote_End());

        return true;
    }

    void SetNewSpeedFactor()
    {
        float distance = (endTrans.position - startTrans.position).magnitude;
        speedFactor = (distance / customFactor);
    }

    IEnumerator MoveNote_Start()
    {
        stateC = true;

        SetNewSpeedFactor();

        if (!popup) // 즉, pop 상태가 아닐때는 
        {
            Cursor.lockState = CursorLockMode.None; //커서 고정 해제

            while (true)
            {
                yield return new WaitForSeconds(0.01f);


                // - 이동
                float step_m = moveSpeed * speedFactor * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, endTrans.position, step_m);

                // - 회전
                float step_r = rotSpeed * Time.deltaTime;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, endTrans.rotation, step_r);

                if (Vector3.Distance(transform.position, endTrans.position) < 0.1f)
                {
                    float angle = Quaternion.Angle(transform.rotation, endTrans.rotation);
                    if (angle >= 179f)
                    {
                        break;
                    }
                    if (Vector3.Angle(transform.forward, endTrans.forward) < 1f)
                    {
                        break;
                    }
                }

            }

            popup = true;
        }

        stateC = false;
    }

    IEnumerator OpenNote_Start()
    {
        stateC = true;

        //animator.SetBool("IsUnfolding", true);
        animator.SetFloat("speed", 1f);
        animator.Play(AniName_unfold, 0, 0.1f);

        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(EventOpenAnimationName))
        {
            yield return null;
        }

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9f)
        {
            // - 애니메이션 재생 중
            yield return null;
        }

        stateC = false;
    }

    IEnumerator MoveNote_End()
    {
        stateC = true;

        //animator.SetBool("IsUnfolding", false);
        animator.SetFloat("speed", -2f);
        animator.Play(AniName_unfold, 0, 0.4f);
        cAni = StartCoroutine(UnfoldAni_End());

        // - 스크립트 회복
        Player_script.enabled = true;
        CamMove_Script.enabled = true;

        if (popup) // start 지점으로 갈떄
        {
            while (true)
            {
                yield return new WaitForSeconds(0.01f);

                // - 이동
                float step_m = moveSpeed_return * speedFactor * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, startTrans.position, step_m);

                // - 회전
                float step_r = rotSpeed_return * Time.deltaTime;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, startTrans.rotation, step_r);

                if (Vector3.Distance(transform.position, startTrans.position) < 0.1f)
                {
                    if (Vector3.Angle(transform.forward, startTrans.forward) < 1f)
                    {
                        break;
                    }
                }
            }

            popup = false;

            Cursor.lockState = CursorLockMode.Locked; //커서 고정
        }

        // - 
        {
            clockNoteCtrler_script = GameObject.FindObjectOfType<ActionController_TestNote>();

            if (clockNoteCtrler_script != null)
            //if (clockNoteCtrler_script.activeSelf == true) //활성화 상태이면
            {
                clockNoteCtrler_script.reset_NoteState();
            }
        }

        stateC = false;
    }

    IEnumerator UnfoldAni_End()
    {
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(EventOpenAnimationName))
        {
            yield return null;
        }

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.001f)
        {
            // - 애니메이션 재생 중
            yield return null;
        }

        clockNote_script.SetActive_Ani(false);
        clockNote_script.SetActive_Outline(true);
    }
}
