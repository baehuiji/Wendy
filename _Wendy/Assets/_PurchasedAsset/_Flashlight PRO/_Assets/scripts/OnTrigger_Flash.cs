using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 수정 중! 활성화 노노! 
public class OnTrigger_Flash : MonoBehaviour
{
    [SerializeField]
    private string brokenSound = "bombHandLamp";

    // 도달했을 시 손전등이 꺼지는 연출 -- 원래는 불이 켜지면 해야하는데 지금은 트리거로 하기; 
    public GameObject mainCamera;
    public GameObject Target_Player;
    public GameObject playerModeling;

    public float speedFactor = 0.0f; //보정값 222
    Animator _animator = null;


    public GameObject FlashLamp_Transform;
    public GameObject FlahLamp_EndTrans;

    public GameObject FlashCamera;
    public GameObject FlashPack;

    private bool FlashState = true;
    private bool CoroutineState = false;
    float checkangle;
    public float moveSpeed = 0.7f;
    float step;
    float Setangle = 1f;

    void Start()
    {
        _animator = playerModeling.GetComponent<Animator>();

    }

    // Start is called before the first frame update



     public void FlashLightEnd(int i)
    {
        StartCoroutine(MoveFlash());
    }


    //public void OnTriggerEnter(Collider other)
    //{
    //    if (other.transform.CompareTag("Player"))
    //    {
    //        //정지

    //        if (FlashState == true)
    //        {



    //        }

    //        else
    //        {

    //        }
    //        // 파티클 실행

    //        // 손전등 내림 


    //        //Debug.Log("충돌! 닿았습니까? 그렇다면 손전등을 내놓고 가세요! ");
    //        // 한번 실행하면 그것으로 끝! 이 콜라이더도 비활성화! 



    //    }


    //}
    // 로테이션 이동- 어우 개  빡 치 네! 가 생각나네!
    IEnumerator MoveFlash()
    {
        FlashState = true;


        _animator.SetBool("IsWalking", false);
        Target_Player.gameObject.GetComponent<Player_HJ>().enabled = false;
        //mainCamera.gameObject.GetComponent<FirstPersonCamera>().enabled = false;

        Vector3 StartPoint = FlashLamp_Transform.transform.position;
        Vector3 SetPoint = FlahLamp_EndTrans.transform.position;

        Quaternion StartRotation = FlashLamp_Transform.transform.rotation;
        Quaternion SetRotation = FlahLamp_EndTrans.transform.rotation;


        SoundManger.instance.PlaySound(brokenSound);
        //고장 파티클 추가

        while (true)
        {
            yield return new WaitForSeconds(0.01f);


            step += Time.deltaTime * moveSpeed;
            //FlashLamp_Transform.transform.rotation = Quaternion.Slerp(targetSet, bRotation, step);



            FlashLamp_Transform.transform.position = Vector3.Lerp(StartPoint, SetPoint, step);

            FlashLamp_Transform.transform.rotation = Quaternion.Lerp(StartRotation,SetRotation, step);

            if (Vector3.Distance(FlashLamp_Transform.transform.position, SetPoint) <= 0.1f)
            {

                break;

            }
        
     
    


        }

        yield return new WaitForSeconds(0.05f);
        Target_Player.gameObject.GetComponent<Player_HJ>().enabled = true; //이걸 줄일수 있는게 없을까?
                                                                           // mainCamera.gameObject.GetComponent<FirstPersonCamera>().enabled = true;
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        FlashCamera.SetActive(false);
        FlashPack.SetActive(false);

        FlashState = false;


        //   Quaternion targetSet = FlashLamp_Transform.transform.rotation;
        ////   Quaternion bRotation = FlashLamp_Transform.transform.rotation + new Vector3(0,0,0);
        //   Quaternion bRotation = Quaternion.Euler(targetSet.eulerAngles + new Vector3(0, 0, -20));

        //   yield return new WaitForSeconds(3f);
        //   Debug.Log("왜안돼");

        //   //아래 수정 

        //   while (true)
        //   {
        //       yield return new WaitForSeconds(0.01f);


        //       step +=  Time.deltaTime / moveSpeed ;
        //       //FlashLamp_Transform.transform.rotation = Quaternion.Slerp(targetSet, bRotation, step);

        //       FlashLamp_Transform.transform.rotation = Quaternion.Euler(Setangle, 0, 0);

        //       Setangle++;

        //       checkangle = Quaternion.Angle(bRotation, FlashLamp_Transform.transform.rotation);


        //       if (Setangle >= 10)
        //       {
        //           Target_Player.gameObject.GetComponent<Player_HJ>().enabled = true;
        //          // mainCamera.gameObject.GetComponent<FirstPersonCamera>().enabled = true;
        //           break;

        //       }


        //   }

        //   FlashState = false;



        //Mathf.Lerp ( 시작점, 종료점, 거리비율을 받는데 )
        // 시작점에는 오브젝트의 현재 위치를 받고 - 종료점에는 오브젝트 현재 위치 + 10.
        // 다시 시작점에는 오브젝트의 종료 위치를 받고 - 종료점에는 그 시작점의 + 10.


    }



}
