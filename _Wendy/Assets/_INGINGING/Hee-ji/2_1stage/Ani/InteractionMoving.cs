using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InteractionMoving : MonoBehaviour
{
    //private UnityEngine.AI.NavMeshAgent agent;
    private NavMeshAgent agent;
    public Transform centerTrans; //Center
    //public Transform playerTrans; //
    public Animator animator;
    public float rotSpeed = 1f;

    public Transform endTrans; //목표위치

    private Coroutine coroutine;
    bool once = false; //1회만 실행하도록
    public bool isPlaying = false;

    InteractionAnimation interactAni_script;

    public float meleeRange = 3f;

    Quaternion tempRotCenter;

    Player_1stage playerCtrler_script;
    ActionController_01 actionCtrler;

    // 방향
    //public Transform standard;

    // 모델링 피봇 -> 원회전을 다시 기본상태로 돌리기 위해
    public Transform _ani_pivot;

    // 콜라이더
    private Collider collider;

    //Vector3 from_center;
    //Vector3 to_endRot;

    //
    public float _angle = 0.0f; // 0 : 앞, 180 : 뒤를 바라봄, 90 : 왼쪽, -90 : 오른쪽

    public bool _answer = false;
    public bool _isTree = false;
    public int _answer_index = -1;
    InteracttAniManager interAniManager_script;

    public HideTree_ver2 hideTree_script;
    public Collider hideColl;

    void Start()
    {
        agent = GameObject.FindWithTag("Player").GetComponent<NavMeshAgent>();

        playerCtrler_script = GameObject.FindObjectOfType<Player_1stage>();
        actionCtrler = GameObject.FindObjectOfType<ActionController_01>();

        interactAni_script = GetComponent<InteractionAnimation>();

        collider = GetComponent<Collider>();

        interAniManager_script = GameObject.FindObjectOfType<InteracttAniManager>();
    }

    void Update()
    {
    }

    public void StartToMove()
    {
        if (once == true || isPlaying == true) //중복재생방지
        {
            return;
        }

        if (hideTree_script != null)
        {
            hideTree_script.enabled = false;
            hideColl.enabled = false;
        }

        coroutine = StartCoroutine(MoveAgent());
    }

    IEnumerator MoveAgent()
    {
        collider.enabled = false; //콜라이더먼저 없애야지 클릭버튼 사라짐 는 아니였다

        playerCtrler_script.enabled = false;
        actionCtrler.enabled = false;

        once = true;
        isPlaying = true;

        animator.SetBool("IsWalking", true);

        float step = 0f;
        agent.enabled = true;
        agent.isStopped = false;
        agent.SetDestination(endTrans.position);

        playerCtrler_script.ReturnPivotRot();

        while (true)
        {
            if (!agent.pathPending)
            {
                //이동               
                step += rotSpeed * Time.deltaTime;
                if (agent.velocity.sqrMagnitude > Mathf.Epsilon)
                {
                    Vector3 temp = agent.velocity.normalized;
                    Vector3 rot_dir = new Vector3(temp.x, 0f, temp.z);
                    centerTrans.rotation = Quaternion.LookRotation(
                                         Vector3.RotateTowards(centerTrans.forward, rot_dir, step, 0.0f));
                }

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        agent.ResetPath();
                        agent.velocity = Vector3.zero;
                        agent.isStopped = true;
                        //agent.enabled = false;
                        break;
                    }
                }

                yield return null;
            }
            else
                yield return null;
        }

        //도착
        animator.SetBool("IsWalking", false);

        if (_answer)
        {
            interactAni_script.set_aIndex(_answer_index);
            if (!_isTree) //정답 상자일때
                interAniManager_script.Active_Piece(_answer_index);
        }

        //회전 : 나무를 향하도록 회전
        //0
        float step2 = 0.0f;
        while (true)
        {
            //1
            step2 += 2f * Time.deltaTime;
            centerTrans.rotation = Quaternion.LookRotation(
                     Vector3.RotateTowards(centerTrans.forward, endTrans.forward, step2, 0.0f));

            float dot = Vector3.Dot(centerTrans.forward, endTrans.forward);
            float angle = Mathf.Acos(dot);

            //회전이 끝낫는지 확인
            //if (dot == 0) //dot < 1 && dot > -1)
            if (Quaternion.Angle(centerTrans.rotation, endTrans.rotation) <= 0.01f)
            {
                break;
            }

            yield return null;
        }

        // @@
        //1
        //float newAngle = Vector3.Dot(standard.forward, centerTrans.forward);
        //interactAni_script.set_angle(newAngle);

        //2
        //float newAngle = Vector3.Dot(-standard.forward, centerTrans.forward);
        //if (newAngle >= 0)
        //    interactAni_script.transAngle = 180f - newAngle;

        //3
        //float newAngle = Vector3.Dot(-standard.forward, centerTrans.forward);
        //if (newAngle >= 0f && newAngle <= 90f) //newAngle >= 0
        //    interactAni_script.transAngle =  - newAngle;
        //else if(newAngle < 0f && newAngle <= -90f)
        //    interactAni_script.transAngle = - newAngle + 180f;

        //4
        interactAni_script.set_angle(_angle);

        //애니메이션 실행
        interactAni_script.PlayAni();

        isPlaying = false;
        this.enabled = false;

    }
}
