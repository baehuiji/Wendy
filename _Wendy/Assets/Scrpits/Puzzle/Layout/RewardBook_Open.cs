using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardBook_Open : MonoBehaviour
{
    // - 상태
    bool popup = false;
    bool state = false; //애니메이션 플레이(코루틴 실행) 상태
    int step = 0;

    bool isOpen = false;
    bool isFlip = false;

    // - 속성
    public float moveSpeed;
    public float rotSpeed;
    public float moveSpeed_return;
    public float rotSpeed_return;
    private float speedFactor = 0.0f; //보정값
    public float customFactor;

    // - 애니메이션
    private Animator animator;
    private static string EventOpenAnimationName = "Base Layer.book_open";
    private static string EventFilpAnimationName = "Base Layer.book_flip";

    //private static string AniName_idle = "book_idle";
    private static string AniName_open = "book_open";
    private static string AniName_filp = "book_flip";

    public Transform startTarget;
    public Transform endTarget;

    //public Collider flip_colider; // 페이지 넘길때의 콜라이더 -> 화면 반을 잘라서 오른쪽을 클릭하는것으로 변경
    private Collider book_colider; // 책 팝업시 콜라이더

    private ActionController_GetKey getKey_script;

    public GameObject _outlineObj;
    public GameObject _aniFbx;

    private Coroutine _coroutine;
    bool cor_state = false; //이동 코루틴 실행상태일떄


    void Start()
    {
        animator = GetComponent<Animator>();
        book_colider = GetComponent<BoxCollider>();

        getKey_script = Camera.main.GetComponent<ActionController_GetKey>();
    }

    void Update()
    {

    }

    public void move_BookAni()
    {
        //if (popup)
        //    return;

        if (cor_state)
            return;

        _coroutine = StartCoroutine(MoveBook());
    }

    public bool play_BookAni(int s)
    {
        if (state || !popup)
            return false;

        state = true;
        step = s;

        select_step();

        return true;
    }

    private void select_step()
    {
        switch (step)
        {
            case 1: // 책 펼치기
                isOpen = true;
                StartCoroutine(bookOpenAni());
                break;

            case 2: // 책 덮기
                //1
                //isOpen = false;
                //StartCoroutine(bookOpenAni());
                //break;

                //2
                isOpen = false;
                isFlip = false;
                //flip_colider.enabled = false;

                StartCoroutine(bookCloseAni());
                break;

            case 3: // 한장 넘기기 (책 펼쳐져있을때)
                isFlip = true;
                StartCoroutine(bookFlipAni());
                break;

            //case 4: // 넘기기 취소 (책 펼쳐져있을때)
            //    isFlip = false;
            //    StartCoroutine(bookFlipAni());
            //    break;

            default:
                break;
        }
    }

    void SetNewSpeedFactor()
    {
        float distance = (endTarget.position - startTarget.position).magnitude;
        speedFactor = (distance / customFactor);
    }

    IEnumerator MoveBook()
    {
        cor_state = true;

        SetNewSpeedFactor();

        if (!popup) // end 지점으로 갈때
        {
            _outlineObj.SetActive(false);
            _aniFbx.SetActive(true);

            Cursor.lockState = CursorLockMode.None; //커서 고정 해제
            book_colider.enabled = false;

            while (true)
            {
                yield return new WaitForSeconds(0.01f);

                // - 이동
                float step_m = moveSpeed * speedFactor * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, endTarget.position, step_m);

                // - 회전
                float step_r = rotSpeed * Time.deltaTime;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, endTarget.rotation, step_r);

                if (Vector3.Distance(transform.position, endTarget.position) < 0.01f)
                {
                    //float angle = Quaternion.Angle(transform.rotation, endTarget.rotation);
                    //if (angle >= 179f)
                    //{
                    //    break;
                    //}
                    if (Vector3.Angle(transform.forward, endTarget.forward) < 1f)
                    {
                        break;
                    }
                }
            }

            popup = true;
            state = false;
            //flip_colider.enabled = true;
        }
        else // start 지점으로 갈떄
        {
            while (true)
            {
                yield return new WaitForSeconds(0.01f);

                // - 이동
                float step_m = moveSpeed_return * speedFactor * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, startTarget.position, step_m);

                // - 회전
                float step_r = rotSpeed_return * Time.deltaTime;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, startTarget.rotation, step_r);

                if (Vector3.Distance(transform.position, startTarget.position) < 0.01f)
                {
                    if (Vector3.Angle(transform.forward, startTarget.forward) < 1f)
                    {
                        break;
                    }
                }
            }

            _outlineObj.SetActive(true);
            _aniFbx.SetActive(false);

            book_colider.enabled = true;

            popup = false;

            Cursor.lockState = CursorLockMode.Locked; //커서 고정

            getKey_script.reset_BookState();
        }

        cor_state = false;
    }

    IEnumerator bookOpenAni()
    {
        // - 애니메이션 시작
        float nTime;

        //if (isOpen)
        //{
        nTime = 0.75f;
        animator.SetFloat("speed", 1f);
        animator.Play(AniName_open, 0, 0.0f);
        //}
        //else
        //{
        //    nTime = 0.01f;
        //    animator.SetFloat("speed", -1f);
        //    animator.Play(AniName_open, 0, 1.0f);
        //}

        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(EventOpenAnimationName))
        {
            yield return null;
        }

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < nTime)
        {
            // - 애니메이션 재생 중
            yield return null;
        }

        // - 애니메이션 완료
        state = false;
    }

    IEnumerator bookCloseAni()
    {
        // - 애니메이션 시작
        float nTime;

        nTime = 0.01f;
        animator.SetFloat("speed", -3f);
        animator.Play(AniName_filp, 0, 0.85f);

        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(EventFilpAnimationName))
        {
            yield return null;
        }

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > nTime)
        {
            // - 애니메이션 재생 중
            yield return null;
        }

        //nTime = 0.01f;
        animator.SetFloat("speed", -2f);
        animator.Play(AniName_open, 0, 0.75f);

        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(EventOpenAnimationName))
        {
            yield return null;
        }

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > nTime)
        {
            // - 애니메이션 재생 중
            yield return null;
        }

        // - 애니메이션 완료
        state = false;

        // - 이전으로 돌아가기
        StartCoroutine(MoveBook());
    }

    IEnumerator bookFlipAni()
    {
        // - 애니메이션 시작
        float nTime;

        //if (isFlip)
        //{
        nTime = 0.85f;
        animator.SetFloat("speed", 1f);
        animator.Play(AniName_filp, 0, 0.0f);
        //}
        //else
        //{
        //    nTime = 0.01f;
        //    animator.SetFloat("speed", -1f);
        //    animator.Play(AniName_filp, 0, 1.0f);
        //}

        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(EventFilpAnimationName))
        {
            yield return null;
        }

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < nTime)
        {
            // - 애니메이션 재생 중
            yield return null;
        }

        // - 애니메이션 완료
        state = false;

        getKey_script.set_isLastPage(); // 열쇠를 얻을수있는 상태
    }
}
