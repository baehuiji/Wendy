using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardNote_Check : MonoBehaviour
{
    [SerializeField]
    private string NoteCheckSound = "CheckNote";

    // - 상태
    bool popup = false; //애니메이션 팝업 
    bool state = false; //애니메이션 플레이 상태
    int step = 0;

    bool isOpen = false;
    bool isFlip = false;

    //public GameObject _pointlight;

    // - 속성
    public float moveSpeed;
    public float rotSpeed;
    public float moveSpeed_return;
    public float rotSpeed_return;
    private float speedFactor = 0.0f; //보정값
    public float customFactor;

    // - 애니메이션 -- 필요없어요.
    //private Animator animator;
    //private static string EventOpenAnimationName = "Base Layer.book_open";
    //private static string EventFilpAnimationName = "Base Layer.book_flip";
    //private static string AniName_idle = "book_idle";
    //private static string AniName_open = "book_open";
    //private static string AniName_filp = "book_flip";

    public Transform startTarget;
    public Transform endTarget;

    //public Collider flip_colider; // 페이지 넘길때의 콜라이더 -> 화면 반을 잘라서 오른쪽을 클릭하는것으로 변경
    //private Collider book_colider; // 책 팝업시 콜라이더

    ActionController_GetNote getNote_script;

    // public GameObject FlashLight_Pro;

    // 코루틴 한번만 호출 보장
    private bool _cor_active = false;

    // - 콜라이더 활성화, 비활성화 하기
    Collider _note_collider;

    void Start()
    {
        //animator = GetComponent<Animator>();
        //book_colider = GetComponent<BoxCollider>();

        getNote_script = Camera.main.GetComponent<ActionController_GetNote>();
        //_pointlight.SetActive(false);

        _note_collider = GetComponent<BoxCollider>();
    }


    public void move_NoteAni()
    {
        if (_cor_active)
            return;

        StartCoroutine(MoveNote());
    }

    void SetNewSpeedFactor()
    {
        float distance = (endTarget.position - startTarget.position).magnitude;
        speedFactor = (distance / customFactor);
    }

    IEnumerator MoveNote()
    {
        _cor_active = true;

        SetNewSpeedFactor();

        if (!popup) // 즉, pop 상태가 아닐때는 
        {
            // 커서 고정 해제
            Cursor.lockState = CursorLockMode.None; 

            // 콜라이더
            _note_collider.enabled = false;

            // 사운드
            SoundManger.instance.PlaySound(NoteCheckSound);

            while (true)
            {
               // _pointlight.SetActive(true);

                yield return new WaitForSeconds(0.01f);

                // - 이동
                float step_m = moveSpeed * speedFactor * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, endTarget.position, step_m);

                //FlashLight_Pro.SetActive(false);

                // - 회전
                float step_r = rotSpeed * Time.deltaTime;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, endTarget.rotation, step_r);


                if (Vector3.Distance(transform.position, endTarget.position) < 0.01f)
                {
                    float angle = Quaternion.Angle(transform.rotation, endTarget.rotation);
                    if (angle >= 179f)
                    {                     
                        break;
                    }
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

            // 콜라이더
            //book_colider.enabled = true;
            _note_collider.enabled = true;

            yield return new WaitForSeconds(0.05f);

            popup = false;
            // _pointlight.SetActive(false);

            // FlashLight_Pro.SetActive(true);

            //커서 고정
            Cursor.lockState = CursorLockMode.Locked; 


            getNote_script.reset_NoteState();
        }

        _cor_active = false;
    }
    
    public bool get_corPossible()
    {
        return !_cor_active;
    }

}
