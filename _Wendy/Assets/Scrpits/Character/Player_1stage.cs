using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_1stage : MonoBehaviour
{
    private Vector3 min, max;

    float h, v;

    public float movementSpeed = 5;
    public float turningSpeed = 10;

    Animator _animator = null;

    //int rudder;
    bool check;

    float MaxDistance = 8f;

    private float currentAngle;
    private float desiredAngle;
    private float angle;
    public GameObject target;

    Ray ray;

    public GameObject standard;

    private float atanAngle;

    int anglecheck;

    private Vector3 dir;

    private float zangle;
    private float zMin;
    private float zMax;
    private float desiredZAngle;
    private float currentZAngle;
    public GameObject center;
    public float ZturningSpeed = 20;

    private float xangle;
    private float xMin;
    private float xMax;
    private float desiredXAngle;
    private float currentXAngle;

    public Transform target_modeling;
    public GameObject modelingParent;
    private bool cr_check = false;
    private bool cr_in_check = false;
    private IEnumerator coroutine = null; //Coroutine


    //footprint를 사용하기 위한 컨트롤러
    CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;

    void Start()
    {
        //_animator = GameObject.Find("wendy_umuni_rigging").GetComponent<Animator>(); 
        _animator = modelingParent.GetComponent<Animator>();
        //_animator = GetComponent<Animator>();

        check = false;

        atanAngle = 0.0f;
        anglecheck = 0;

        zangle = 0.0f;
        zMin = -7f;
        zMax = 7f;

        xangle = 0.0f;
        xMin = -7f;
        xMax = 7f;

        dir = Vector3.zero;

        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        //standard.transform.position = new Vector3(transform.position.x, 0, transform.position.z); 

        if (v == 0 & h == 0)
        {
            if (!check)
            {
                _animator.SetBool("IsWalking", false);
                check = true;

                dir = Vector3.zero;

                desiredXAngle = 0f;
                desiredZAngle = 0f;
            }

            //lerp때문에
            {
                //타겟모델링 기준
                currentXAngle = target_modeling.transform.eulerAngles.x;
                xangle = Mathf.LerpAngle(currentXAngle, desiredXAngle, Time.deltaTime * ZturningSpeed);
                currentZAngle = target_modeling.transform.eulerAngles.z;
                zangle = Mathf.LerpAngle(currentZAngle, desiredZAngle, Time.deltaTime * ZturningSpeed);
            }

        }
        else //if (v != 0)
        {
            // - 이전 방향키나 방향이 같으면? => 동일한 값일경우 , 쓸데없는 연산을 줄여야함 @ 수정필요

            //애니메이션
            if (check)
            {
                _animator.SetBool("IsWalking", true);
                check = false;
            }

            //이동
            //ver1
            dir = h * Vector3.left + v * Vector3.back; // 회전방향
            transform.position = transform.position + dir * movementSpeed * Time.deltaTime;

            //캐릭터의 원회전
            {
                if (v == -1) //앞
                {
                    desiredXAngle = 7f;
                }
                else if (v == 1)
                {
                    desiredXAngle = 7f;
                }
                else if (v == 0)
                {
                    desiredXAngle = 0;
                }

                if (h == -1) //left
                {
                    if (v == -1) //(v != 0)
                    {
                        desiredXAngle = 7f;
                        desiredZAngle = -3f;
                    }
                    if (v == 1)
                    {
                        desiredXAngle = 7f;
                        desiredZAngle = 3f;
                    }
                    else
                    {
                        desiredXAngle = 7f;
                    }
                }
                else if (h == 1)
                {
                    if (v == -1) //(v != 0)
                    {
                        desiredXAngle = 7f;
                        desiredZAngle = 3f;
                    }
                    if (v == 1)
                    {
                        desiredXAngle = 7f;
                        desiredZAngle = -3f;
                    }
                    else
                    {
                        desiredXAngle = 7f;
                    }
                }
                else if (h == 0) //여긴 안들어감
                {
                    desiredZAngle = 0f;
                }
            }

            // y , 캐릭터가 향하는방향
            atanAngle = CalculateAngle(-standard.transform.forward, dir);
            currentAngle = target.transform.eulerAngles.y;
            desiredAngle = atanAngle;
            angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * turningSpeed);

            //원 회전
            currentXAngle = target_modeling.transform.eulerAngles.x; //center
            xangle = Mathf.LerpAngle(currentXAngle, desiredXAngle, Time.deltaTime * ZturningSpeed);
            // z
            currentZAngle = target_modeling.transform.eulerAngles.z; //center
            zangle = Mathf.LerpAngle(currentZAngle, desiredZAngle, Time.deltaTime * ZturningSpeed);

            //디버그
            //Debug.DrawRay(standard.transform.position, dir * MaxDistance, Color.blue, 0.3f);
        }

        // - 회전 적용
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        target.transform.rotation = rotation; 

        Quaternion rotation2 = Quaternion.Euler(xangle, 0, zangle);
        target_modeling.localRotation = rotation2;
    }

    public void SetLimits(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;
    }

    public static float GetAngle(Vector3 vStart, Vector3 vEnd)
    {
        Vector3 v = vEnd - vStart;

        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }

    public static float CalculateAngle(Vector3 from, Vector3 to)
    {
        return Quaternion.FromToRotation(from, to).eulerAngles.y;
    }

    float ClampZAngle(float angle, float min, float max)
    {
        if (angle < min)
            angle = min;
        if (angle > max)
            angle = max;
        return Mathf.Clamp(angle, min, max);
    }

    IEnumerator RotateAround_modeling() //코루틴이 진행되는 동안만 원회전 된다
    {
        cr_check = true; //코루틴 시작
        cr_in_check = true;

        while (cr_in_check) //코루틴 진행
        {
            if (Mathf.Abs(zangle) == 5f)
            {
                cr_in_check = false;

                desiredZAngle = 0f;
            }
            else
            {
                yield return null;
            }
        }

        cr_check = false;
    }
}
