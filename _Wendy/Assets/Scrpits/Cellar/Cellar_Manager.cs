using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cellar_Manager : MonoBehaviour
{
    [SerializeField]
    private string MoveWallSound = "Cellar_moveWall";

    [SerializeField]
    private string RockDown = "Cellar_rockDown";

    [SerializeField]
    private string BreakonSound = "Cellar_breakon";

    public GameObject Wall_E;
    public GameObject Wall_S;

    float time = 0f;
    float movetime = 0f;

    public float moveStreet = 1f;
    public float addStreet = 1f;

    // Start is called before the first frame update
    void Start()
    {
        // 특정 상황 반복 시 코루틴 실행. 현재는 플레이어가 집에 들어오자마자 시작되는 것으로.
        // Vector3 E_WallStart = Wall_E.transform.position; // E_Wall 의 시작 위치

        Vector3 S_WallStop = Wall_S.transform.position + new Vector3(0, 0, 2); // E_Wall의 움직이고 난 끝위치


        StartCoroutine(deley());


    }

    // Update is called once per frame
    //void Update()
    //{
    //    // t += Time.deltaTime / 2.0f;

    //    //  Wall_E.transform.position = new Vector3(Mathf.Lerp(0f, 2f, t), Wall_E.transform.position.y , Wall_E.transform.position.z);


    //}

    IEnumerator deley()
    {


        yield return new WaitForSeconds(5);


        Vector3 EWallstartPos = Wall_E.transform.localPosition;
        Vector3 SWallstartPos = Wall_S.transform.position;


        SoundManger.instance.PlaySound(RockDown);
        SoundManger.instance.PlaySound(MoveWallSound);
        StartCoroutine(EWall_CountDown(EWallstartPos + new Vector3(10, 0, 0), 400f));   // 시작부터 카운트 다운이 진행된다. 
        StartCoroutine(SWall_CountDown(SWallstartPos + new Vector3(0, 0, 10), 400f));   // 시작부터 카운트 다운이 진행된다. 




        //yield return new WaitForSeconds(50);


        //Vector3 EWallstartPos = Wall_E.transform.localPosition;
        //Vector3 SWallstartPos = Wall_S.transform.position;

        //SoundManger.instance.PlaySound(RockDown);
        //SoundManger.instance.PlaySound(MoveWallSound);
        //StartCoroutine(EWall_CountDown(EWallstartPos + new Vector3(moveStreet, 0, 0), 3f));   // 시작부터 카운트 다운이 진행된다. 
        //StartCoroutine(SWall_CountDown(SWallstartPos + new Vector3(0, 0, moveStreet), 3f));   // 시작부터 카운트 다운이 진행된다. 

        //yield return new WaitForSeconds(50);

        //SoundManger.instance.PlaySound(BreakonSound);
        //SoundManger.instance.PlaySound(MoveWallSound);
        //StartCoroutine(EWall_CountDown(EWallstartPos + new Vector3(moveStreet + (addStreet), 0, 0), 3f));   // 시작부터 카운트 다운이 진행된다. 
        //StartCoroutine(SWall_CountDown(SWallstartPos + new Vector3(0, 0, moveStreet+(addStreet)), 3f));   // 시작부터 카운트 다운이 진행된다. 


        //yield return new WaitForSeconds(50);
        //SoundManger.instance.PlaySound(MoveWallSound);
        //SoundManger.instance.PlaySound(BreakonSound);
        //StartCoroutine(EWall_CountDown(EWallstartPos + new Vector3((moveStreet + (addStreet * 2)), 0, 0), 3f));   // 시작부터 카운트 다운이 진행된다. 

        //StartCoroutine(SWall_CountDown(SWallstartPos + new Vector3(0, 0, moveStreet + (addStreet *2)), 3f));   // 시작부터 카운트 다운이 진행된다. 


        //yield return new WaitForSeconds(50);
        //SoundManger.instance.PlaySound(MoveWallSound);
        //SoundManger.instance.PlaySound(BreakonSound);
        //StartCoroutine(EWall_CountDown(EWallstartPos + new Vector3((moveStreet + (addStreet * 3)), 0, 0), 3f));   // 시작부터 카운트 다운이 진행된다. 
        //StartCoroutine(SWall_CountDown(SWallstartPos + new Vector3(0, 0, moveStreet + (addStreet*3)), 3f));   // 시작부터 카운트 다운이 진행된다. 


        //yield return new WaitForSeconds(50);
        //SoundManger.instance.PlaySound(MoveWallSound);
        //SoundManger.instance.PlaySound(BreakonSound);
        //StartCoroutine(EWall_CountDown(EWallstartPos + new Vector3((moveStreet + (addStreet * 5)), 0, 0), 3f));   // 시작부터 카운트 다운이 진행된다. 
        //StartCoroutine(SWall_CountDown(SWallstartPos + new Vector3(0, 0, moveStreet + (addStreet * 5)), 3f));   // 시작부터 카운트 다운이 진행된다. 




    }

    IEnumerator EWall_CountDown(Vector3 endPos, float duration)
    {
        time = 0f;

        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        // Vector2 EWallEndPos = Wall_E.transform.localPosition + new Vector3(2, 0, 0);
        // 위치를 움직일 위치를 다시 잡습니다! 시작위치랑, 끝위치를 다시 잡습니다!!!! 후유! 

        Vector3 EWallstartPos = Wall_E.transform.localPosition;
        movetime = duration;

        while (duration > 0.0f) // 선형보간이 진행됩니다. 선형보간의 이동이 끝날때까지! 
        {
            duration -= Time.deltaTime; // 벽 2개가 움직입니다! 천천히 움직입니다!!  현재 5초로 입력했을시 5초동안 움직이는 것을 확인완료했습니다!
            time += Time.deltaTime;
            Wall_E.transform.localPosition = Vector3.Lerp(EWallstartPos, endPos,  time / movetime);
            //   Wall_E.transform.position = Vector3.Lerp(Wall_E.transform.position, E_WallStop, t);

            yield return waitForEndOfFrame;
        }


        yield return new WaitForSeconds(2);


        //Mathf.Lerp ( 시작점, 종료점, 거리비율을 받는데 )
        // 시작점에는 오브젝트의 현재 위치를 받고 - 종료점에는 오브젝트 현재 위치 + 10.
        // 다시 시작점에는 오브젝트의 종료 위치를 받고 - 종료점에는 그 시작점의 + 10.


    }

    IEnumerator SWall_CountDown(Vector3 endPos, float duration)
    {
        //time = 0f;

        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        // Vector2 EWallEndPos = Wall_E.transform.localPosition + new Vector3(2, 0, 0);
        // 위치를 움직일 위치를 다시 잡습니다! 시작위치랑, 끝위치를 다시 잡습니다!!!! 후유! 

        Vector3 SWallstartPos = Wall_S.transform.localPosition;
        movetime = duration;

        while (duration > 0.0f) // 선형보간이 진행됩니다. 선형보간의 이동이 끝날때까지! 
        {
            duration -= Time.deltaTime; // 벽 2개가 움직입니다! 천천히 움직입니다!!  현재 5초로 입력했을시 5초동안 움직이는 것을 확인완료했습니다!
            time += Time.deltaTime;
            Wall_S.transform.localPosition = Vector3.Lerp(SWallstartPos, endPos, time / movetime);
            //   Wall_E.transform.position = Vector3.Lerp(Wall_E.transform.position, E_WallStop, t);

            yield return waitForEndOfFrame;
        }


        yield return new WaitForSeconds(2);


        //이게 끝나면 알아서 중지됨


        Vector3 E_WallStop = Wall_E.transform.position + new Vector3(2, 0, 0); // E_Wall의 움직이고 난 끝위치



        //Mathf.Lerp ( 시작점, 종료점, 거리비율을 받는데 )
        // 시작점에는 오브젝트의 현재 위치를 받고 - 종료점에는 오브젝트 현재 위치 + 10.
        // 다시 시작점에는 오브젝트의 종료 위치를 받고 - 종료점에는 그 시작점의 + 10.


    }


    void EWallMove()
    {
        //Mathf.Lerp ( 시작점, 종료점, 거리비율을 받는데 )
        // 시작점에는 오브젝트의 현재 위치를 받고 - 종료점에는 오브젝트 현재 위치 + 10.
        // 다시 시작점에는 오브젝트의 종료 위치를 받고 - 종료점에는 그 시작점의 + 10.
        //코루딘은 특정시간마다 부르는걸로.............
        //코루딘은 숫자를 셉니다. 몇초후에 실행, 몇초후에 실행...

        // 코루딘 하나를 실행 시켜 두면... -- 코루딘은 반복합니다. 총 5번을 
    }

}
