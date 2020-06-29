using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_HJ : MonoBehaviour
{
    private Vector3 min, max;
    private float h, v;

    public float movementSpeed = 10;
    public float turningSpeed = 60;

    Animator _animator = null;

    public bool gameEnd;

    public GameObject playerObj; //벡터
    public GameObject playerObj_normal; //법선벡터

    private Vector3 targetForward;
    private Vector3 targetNormal_Forward;

    public GameObject playerModeling;

    private bool cp_start = false;


    void Start()
    {
        //_animator = GameObject.Find("wendy_umuni_rigging").GetComponent<Animator>();
        _animator = playerModeling.GetComponent<Animator>();

        gameEnd = false;
    }

    //protected override void Update() //활동제한
    //{
    //    //Executes the GetInput function
    //    GetInput();

    //    float xMinClamp = Mathf.Clamp(transform.position.x, min.x, max.x);
    //    float yMinClamp = Mathf.Clamp(transform.position.y, min.y, max.y);

    //    transform.position = new Vector3(xMinClamp, yMinClamp, transform.position.z);
    //}
    void Update()
    {
        if (gameEnd)
            return;

        GetInput();
    }

    private void GetInput()
    {
        targetForward = playerObj.transform.rotation * Vector3.forward; //localRotation -> rotation
        targetNormal_Forward = playerObj_normal.transform.rotation * Vector3.forward;


        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        // - Animation transition
        if (h == 0 && v == 0)
        {
            _animator.SetBool("IsWalking", false);//변경해서 끄는 것이 아닌 이렇게 애니메이션을 비활성화하는걸루다가....
            //Debug.Log("Arrow Button Released");
        }
        else
        {
            _animator.SetBool("IsWalking", true);  

            targetForward = playerObj.transform.rotation * Vector3.forward; //localRotation -> rotation
        }

        // - 디버그
        //Debug.DrawRay(playerObj.transform.position, targetForward, Color.blue);

        // - 움직임

        float moveX = 0f;
        float moveZ = 0f;
        float speed = 0f;

        //앞뒤
        if (h == 1)
        {
            transform.Translate(-targetForward.x * movementSpeed * Time.deltaTime, 0, -targetForward.z * movementSpeed * Time.deltaTime);

            //moveX = -targetForward.x * Time.deltaTime;
            //moveZ = -targetForward.z * Time.deltaTime;
            //speed = movementSpeed;
        }
        else if (h == -1)
        {
            transform.Translate(targetForward.x * movementSpeed * Time.deltaTime, 0, targetForward.z * movementSpeed * Time.deltaTime);

            //moveX = targetForward.x * Time.deltaTime;
            //moveZ = targetForward.z * Time.deltaTime;
            //speed = movementSpeed;
        }
        //좌우
        if (v == -1)
        {
            transform.Translate(-targetNormal_Forward.x * movementSpeed * Time.deltaTime, 0, -targetNormal_Forward.z * movementSpeed * Time.deltaTime);

            //moveX = -targetNormal_Forward.x * Time.deltaTime;
            //moveZ = -targetNormal_Forward.z * Time.deltaTime;
            //if (speed == 0)
            //    speed = movementSpeed;
            //else //(speed != 0)
            //    speed = movementSpeed / 2;
        }
        else if (v == 1)
        {
            transform.Translate(targetNormal_Forward.x * movementSpeed * Time.deltaTime, 0, targetNormal_Forward.z * movementSpeed * Time.deltaTime);

           // moveX = targetNormal_Forward.x * Time.deltaTime;
           // moveZ = targetNormal_Forward.z * Time.deltaTime;
           // if (speed == 0)
           //    speed = movementSpeed;
           //else
           //     speed = movementSpeed / 2;
        }

        //transform.Translate(moveX * movementSpeed, 0, moveZ * movementSpeed);
    }

    public void SetLimits(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;
    }

    public void set_cp_start(bool b)
    {
        cp_start = b;
    }

    public void SetDeActiveAni()
    {
        _animator.SetBool("IsWalking", false);
    }
}
