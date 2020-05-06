//
//2019-10-21
//플레이어 이동
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Transform[] exitPoints;
    private int exitIndex = 0;

    private Vector3 min, max;

    [SerializeField]
    public Transform myTarget { get; set; } //마우스로 선택한 오브젝트

    float h, v;

    public float movementSpeed = 10;
    public float turningSpeed = 60;

    Animator _animator = null;

    public bool gameEnd;

    void Start()
    {
        _animator = GameObject.Find("wendy_umuni_rigging").GetComponent<Animator>();

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
        //Vector3 moveVector;

        //moveVector.x = Input.GetAxisRaw("Horizontal");
        //moveVector.y = Input.GetAxisRaw("Vertical");

        //direction = moveVector;

        //if (moveVector.x != 0 || moveVector.y != 0)
        //{
        //    if (moveVector.y > 0) exitIndex = 0;         // 위쪽
        //    else if (moveVector.y < 0) exitIndex = 2;    // 아래
        //    else if (moveVector.x > 0) exitIndex = 1;    // 오른족
        //    else exitIndex = 3;                         // 왼쪽
        //}

        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        //Animation transition
        if (h == 0 && v == 0)
        {
            _animator.SetBool("IsWalking", false);
            //Debug.Log("Right Arrow Button Released");
        }
        else
            _animator.SetBool("IsWalking", true);

        //Move
        float horizontal = h * turningSpeed * Time.deltaTime;
        transform.Rotate(0, horizontal, 0);
        float vertical = v * movementSpeed * Time.deltaTime;
        transform.Translate(0, 0, vertical);
    }

    public void SetLimits(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;
    }
}
