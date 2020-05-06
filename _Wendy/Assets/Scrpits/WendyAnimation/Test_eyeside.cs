using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_eyeside : MonoBehaviour
{

    // private GameObject _underdoor;

    public Transform target_see;
    public float speed;


    public Transform target_baby;

    public Transform First_see;
    public Transform Last_see;

    private Transform tr;//카메라 자신의 Transform변수 

    /*
    public Camera go_Camera;
    public Transform target;

    private Vector3 StartPosition;
    private Vector3 EndPostion;

    private float speed = 100f;
    private float startTime;
    private float distanceLength;
    */




    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>(); // 카메라 자신의 Transform 할당
        target_see.position = new Vector3(0.45f, 7.11f, 11.55f);
        // OneState = this.GetComponent<Transform>();
        First_see.transform.rotation = Quaternion.Euler(4.5f, -95.9f, -0.47f);
        //  StartPosition = go_Camera.transform.position;
        //   EndPostion = new Vector3(0.45f, 7.11f, 11.55f);

        // startTime = Time.time;
        // distanceLength = Vector3.Distance(StartPosition, EndPostion);


    }


    /*
        void Reback()
        {
        float step = speed * Time.deltaTime;
        this.transform.position = Vector3.MoveTowards( target_see.position,tr.position, step);

        // CancelInvoke(“LaunchProjectile”); // 필요할 경루 Invoke 취소처리
    }
    */
    // Update is called once per frame
    void Update()
    {



        float step = speed * Time.deltaTime;

        // this.transform.position = Vector3.MoveTowards(target_see.position, tr.position, step);
        this.transform.position = Vector3.MoveTowards(tr.position, target_see.position, step);
        //  transform.rotate = Vector3.MoveTowards(transform.position, target_see.position, step);
        tr.LookAt(target_baby);





        //Vector3 vec = target_baby.position - transform.position; // 배워야함 
        //vec.Normalize();

        // Quaternion targetRot = Quaternion.LookRotation(vec); // 위치값에서 회전값 따로 추출하여 넣음 
        //  Quaternion qua = Quaternion.LookRotation(vec);
        //this.transform.rotation = Quaternion.Lerp(First_see.transform.rotation,, step);

        //  transform.rotation = qua;


        //Invoke("Reback", 5);

    }
}