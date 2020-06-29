using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WendyAI : MonoBehaviour
{
    private NavMeshAgent _agent; //에이전트
    private Animator _animator;
    public GameObject _modeling;

    private IState _current_state;

    // - Wendy 설정값
    public Transform _playerTrans; // 플레이어 위치
    private Vector3 _offset;
    private Vector3 _spherePos;
    public float _radius;

    // 플레이어 근방의 위치

    [SerializeField]
    private bool _contact = false; // 플레이어와 처음으로 접촉

    private bool _clear2stage = false; // 2스테이지 클리어

    private Vector3 _rot_dir;
    public float speed;

    private Collider boxcoll; //게임오버를 위한 콜라이더
    private Cellar_Wendy gameOver_script;
    private Cellar_Manager gameOver_script2;
    private Collider spherecoll; //플레이어를 쫓아가기 위한 콜라이더


    //- 지하실과 2층 길목에 있을 가이드 링
    public GameObject Ring_Particle;
   // ParticleSystem event_ringParticle;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = _modeling.GetComponent<Animator>();
      //  event_ringParticle = Ring_Particle.GetComponentInChildren<ParticleSystem>();
        // - 상태 초기화 : 지하실에서 놀고 있는 애니메이션
        SetState(new Wendy_PlayState());
        _agent.updateRotation = false;

        _rot_dir = Vector3.zero;

        boxcoll = GetComponent<BoxCollider>();
        spherecoll = GetComponent<SphereCollider>();

        gameOver_script = GameObject.FindObjectOfType<Cellar_Wendy>();
    }

    private void Update()
    {
        _current_state.Update();
    }

    public void SetState(IState nextState)
    {
        if (_current_state != null)
        {
            _current_state.OnExit();
        }

        _current_state = nextState;
        _current_state.OnEnter(this);
    }

    // - 애니메이션
    public void SetClearAni()
    {
        // Play 애니메이션에서 Idle 애니메이션으로
        _animator.SetBool("Clear2Stage", true);
    }
    public void SetIdleAni()
    {
        _animator.SetBool("IsWalking", false);
    }
    public void SetWalkAni()
    {
        _animator.SetBool("IsWalking", true);
    }

    // - 2스테이지 배치퍼즐 클리어 직후
    public void ClearLayoutPuzzle()
    {
        if (_current_state.GetStateNum() == 1)
        {
            //Debug.Log("ddd");

            SetClearAni();  //_current_state.ChangeAniFromPlay();
        }
    }
    // - 상태전이 : play -> idle : 플레이어가 웬디범위 안에 들어왔을때(지하실)
    public void SetClear2Stage()
    {
        if (_current_state.GetStateNum() == 1)
        {
            _clear2stage = true;
            SetState(new Wendy_IdleState()); //_current_state.SetStateFromPlay();
        }
    }
    public bool GetClear2Stage()
    {
        return _clear2stage;
    }

    // - 상태전이 : idle -> move
    public void SetContactWithPlayer(bool b)
    {
        _contact = b;
    }
    public bool GetContact()
    {
        return _contact;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SetContactWithPlayer(true);

            //_current_state.SetContact();

            //코루틴
            if (_current_state.GetStateNum() == 1)
                ChangeState();



        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SetContactWithPlayer(false);

            //if (_current_state.GetStateNum() == 2)
            //    ChangeState();
        }
    }

    private Coroutine _CWPcoroutine;
    bool _once = false;
    IEnumerator ChangeFromIdleState()
    {
        _once = true;

        yield return new WaitForSeconds(3f);

        //Debug.Log("~idle");
        SetState(new Wendy_MoveState());
        _once = false;
        _CWPcoroutine = null;
    }
    public void ChangeState()
    {
        if (_once)
        {
            return;
        }
        _CWPcoroutine = StartCoroutine(ChangeFromIdleState());
    }

    public bool GetCorB222()
    {
        return _once;
    }
    public void StopCWPCoroutine()
    {
        if (_once) //if (_CWPcoroutine != null)
        {
            StopCoroutine(_CWPcoroutine);

            _once = false;
            _CWPcoroutine = null;

            SetState(new Wendy_MoveState());
        }
    }

    // - 상태전이 : move -> idle

    Vector3 Range;
    float originalSpeed = 2;
    private Coroutine movementCoroutine;
    int cost;
    bool _mmnt_crutin_state = false;
    public void SetDestination()
    {
        // 이동
        //_offset = transform.TransformDirection(Vector3.forward);
        _spherePos = _playerTrans.position;
        Vector3 newPos = RandomNavSphere(_spherePos, _radius, -1);
        _agent.SetDestination(newPos);

        //// 회전 
        //Vector3 dirToTarget = newPos - transform.position;
        //transform.rotation = Quaternion.LookRotation(dirToTarget, Vector3.up);
    }

    private static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        //Vector3 randDirection = Random.insideUnitSphere * dist;

        Vector3 tempDir = Random.insideUnitSphere;
        float range = Random.Range(1, dist);
        Vector3 randDirection = tempDir.normalized * range;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, 10, layermask); //
        //cost = 1; //IndexFromMask(navHit.mask);

        Debug.Log(navHit.position.ToString());


        return navHit.position;
    }

    public void StartMovemntCoroutine()
    {
        if (_mmnt_crutin_state)
        {
            return;
        }

        movementCoroutine = StartCoroutine(MovementCoroutine());
    }
    public void StopMovemntCoroutine()
    {
        if (_mmnt_crutin_state) //if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);

            //_agent.SetDestination(transform.position);

            //gameObject.GetComponent<NavMeshAgent>().enabled = false;
            _agent.isStopped = true;
            _agent.velocity = Vector3.zero;

            _mmnt_crutin_state = false;
            movementCoroutine = null;

            SetState(new Wendy_IdleState());
        }
    }
    public void CollideWithPlayer()
    {
        if (_current_state.GetStateNum() == 3)
        {
            StopMovemntCoroutine();
        }
    }

    IEnumerator MovementCoroutine()
    {
        _mmnt_crutin_state = true;

        gameObject.GetComponent<NavMeshAgent>().enabled = true;
        _agent.isStopped = false;
        Vector3 unpausedSpeed = Vector3.zero;
        Range = transform.position;

        _spherePos = _playerTrans.position;
        Vector3 newPos = RandomNavSphere(_spherePos, _radius, -1);

        //회전
        //Vector3 dir = _playerTrans.position - transform.position;
        //target.rotation = Quaternion.Euler(0, x, 0);
        //Quaternion qtemp = Quaternion.Euler(0, x, 0);
        //transform.rotation = Quaternion.Slerp(from.rotation, to.rotation, step);
        float step = speed * Time.deltaTime;

        //도착지
        _agent.SetDestination(newPos);

        // - 끼임 오류 수정
        float trappedTime = 0.0f;
        Vector3 prePos = Vector3.zero;
        prePos = transform.position;

        while (!WithinRange(newPos, Range))
        {
            //회전
            if (_agent.velocity.sqrMagnitude > Mathf.Epsilon)
            {
                Vector3 temp = _agent.velocity.normalized;
                _rot_dir = new Vector3(temp.x, 0f, temp.z);

                //transform.rotation = Quaternion.LookRotation(_agent.velocity.normalized); //X, 이상
                //transform.rotation = Quaternion.LookRotation(_rot_dir); //O

                transform.rotation = Quaternion.LookRotation(
                                     Vector3.RotateTowards(transform.forward, _rot_dir, step, 0.0f));
            }


            Range = transform.position;

            //if (GameTime.Paused)
            //{
            //    // emulate effect of Time.timeScale = 0
            //    _agent.velocity = Vector3.zero;
            //    _agent.isStopped = true;
            //}
            //else
            {
                // restore velocity from before pause
                //if (_agent.isStopped)
                //{
                //    _agent.isStopped = false;
                //    _agent.velocity = unpausedSpeed;
                //}
                //_agent.SetDestination(target.position);
                _agent.speed = originalSpeed; // / GetNavMeshCost();
                unpausedSpeed = _agent.velocity;
            }

            // - 끼임 오류 수정
            if (transform.position == prePos)
            {
                trappedTime += Time.deltaTime;
                if (trappedTime > 1f)
                {
                    break;
                }
            }
            else
            {
                trappedTime = 0f;
            }
            prePos = transform.position;

            yield return null;
        }

        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;

        _mmnt_crutin_state = false;
        movementCoroutine = null;

        SetState(new Wendy_IdleState());
    }
    bool WithinRange(Vector3 to, Vector3 from)
    {
        float distanceToTarget = Vector3.Distance(to, from);
        float distanceThreshold = 0.1f;
        if (distanceToTarget <= distanceThreshold)
        {
            return true;
        }
        return false;
    }

    // int IndexFromMask(int mask)
    //{
    //    for (int i = 0; i < 32; ++i)
    //    {
    //        if ((1 << i & mask) != 0)
    //        {
    //            return i;
    //        }
    //    }
    //    return -1;
    //}
    float GetNavMeshCost()
    {
        return cost;
    }

    private Coroutine _CFNDcoroutine;
    bool once = false;
    public void ChangeDestination()
    {
        if (once)
        {
            return;
        }

        _CFNDcoroutine = StartCoroutine(ChangeFromNavDestination());
    }
    IEnumerator ChangeFromNavDestination()
    {
        once = true;
        yield return new WaitForSeconds(2f);

        SetState(new Wendy_IdleState());
        once = false;

        _CFNDcoroutine = null;
    }

    public void StopCDCoroutine()
    {
        if (once)
        {
            StopCoroutine(_CFNDcoroutine);
            _CFNDcoroutine = null;
        }
    }

    public bool GetCorB()
    {
        return once;
    }

    public void colliderChange()
    {
        boxcoll.enabled = false;
        gameOver_script.enabled = false;
        gameOver_script2.enabled = false;
        spherecoll.enabled = true;
    }

    // - 플레이어 쳐다보기


    // - 움직이기전 방향으로 회전
}
