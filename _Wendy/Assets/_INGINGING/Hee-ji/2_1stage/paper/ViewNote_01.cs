using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewNote_01 : MonoBehaviour
{
    [SerializeField]
    private string NoteSound;

    public Transform endTrans;
    public Transform startTrans;

    public bool popup = false; //완벽히 띄어져있는가
    public bool stateC = false; //코루틴이 한번만 실행되도록

    // - 애니메이션
    // 없다
    public float moveSpeed;
    public float rotSpeed;
    public float moveSpeed_return;
    public float rotSpeed_return;
    private float speedFactor = 0.0f; //보정값
    public float customFactor;

    // - 해제해야할 스크립트
    ActionController_01 actionCtrler_script; //해제하면 X
    Player_1stage playerCtrler_script; //해제해야함

    void Start()
    {
        actionCtrler_script = GameObject.FindObjectOfType<ActionController_01>();
        playerCtrler_script = GameObject.FindObjectOfType<Player_1stage>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartAni_Note()
    {
        if (stateC)
        {
            return;
        }

        // - 쪽지 습득 -> 한번만 습득?
        //~

        playerCtrler_script.SetunActive();
        playerCtrler_script.enabled = false;

        SoundManger.instance.PlaySound(NoteSound);
        StartCoroutine(MoveNote_Start());
    }
    public bool EndAni_Note()
    {
        if (stateC)
        {
            return false;
        }

        playerCtrler_script.enabled = true;

        StartCoroutine(MoveNote_End());

        return true;
    }

    void SetNewSpeedFactor()
    {
        float distance = (endTrans.position - startTrans.position).magnitude;
        speedFactor = (distance / customFactor);
    }

    IEnumerator MoveNote_Start()
    {
        stateC = true;


        SetNewSpeedFactor();

        if (!popup) // 즉, pop 상태가 아닐때는 
        {
            while (true)
            {
                yield return new WaitForSeconds(0.01f);

                // - 이동
                float step_m = moveSpeed * speedFactor * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, endTrans.position, step_m);

                // - 회전
                float step_r = rotSpeed * Time.deltaTime;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, endTrans.rotation, step_r);

                if (Vector3.Distance(transform.position, endTrans.position) < 0.1f)
                {
                    float angle = Quaternion.Angle(transform.rotation, endTrans.rotation);
                    if (angle >= 179f)
                    {
                        break;
                    }
                    if (Vector3.Angle(transform.forward, endTrans.forward) < 1f)
                    {
                        break;
                    }
                }
            }

            popup = true;
        }

        stateC = false;
    }

    IEnumerator MoveNote_End()
    {
        stateC = true;

        if (popup) // start 지점으로 갈떄
        {
            while (true)
            {
                yield return new WaitForSeconds(0.01f);

                // - 이동
                float step_m = moveSpeed_return * speedFactor * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, startTrans.position, step_m);

                // - 회전
                float step_r = rotSpeed_return * Time.deltaTime;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, startTrans.rotation, step_r);

                if (Vector3.Distance(transform.position, startTrans.position) < 0.1f)
                {
                    if (Vector3.Angle(transform.forward, startTrans.forward) < 1f)
                    {
                        break;
                    }
                }
            }

            popup = false;
        }

        stateC = false;
    }
}
