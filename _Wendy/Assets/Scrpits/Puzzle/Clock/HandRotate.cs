using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandRotate : MonoBehaviour
{
    public int dirType = 0; //인스펙터창에 입력, -1은 반시계방향, 1은 시계방향, 이 값은 ClockNum인덱스가 줄어드는건지도 판별한다
    private float rotateValue;

    //시계판에 적힌 단위들
    public int[] ClockNum;

    //인스펙터창에 입력, 시계침이 가르키는 값을 알 수 있는 인덱스
    public int hand1_index;
    public int hand2_index;

    public EnterClockAnswer enterBtn_script;

    // - 회전
    private bool rotState = false;
    private float dirRotY = 0f;
    private Quaternion dirRot_Qur;
    public float speed;
    public Transform avoid_GimbalLockTemp;


    // - 게이지
    //public CustomFan GaugeBar_Script = null; //왜 인식이 안될까?

    void Start()
    {
        if (dirType == -1)
            rotateValue = 30f;
        else if (dirType == 1)
            rotateValue = -30f;

        enterBtn_script = GameObject.FindObjectOfType<EnterClockAnswer>();
    }

    void Update()
    {
    }

    public void start_rotate()
    {
        StartCoroutine(ChangeOfPeterPanTime__());
    }

    IEnumerator ChangeOfPeterPanTime__()
    {
        //yield return new WaitForSeconds(3f); //30초 -> 이걸 게이지로 #

        avoid_GimbalLockTemp.Rotate(rotateValue, 0, 0);
        //avoid_GimbalLockTemp.Rotate(Vector3.right * rotateValue);

        rotState = true;

        float temp;

        while (transform.rotation.eulerAngles.x != avoid_GimbalLockTemp.rotation.eulerAngles.x)
        {
            yield return new WaitForSeconds(0.01f);

            float step = speed * Time.deltaTime;

            transform.rotation = Quaternion.RotateTowards(transform.rotation, avoid_GimbalLockTemp.rotation, step);

            temp = (transform.rotation.eulerAngles.x) - (avoid_GimbalLockTemp.rotation.eulerAngles.x);
            temp = Mathf.Abs(temp);

            if(temp <= 0.25f)
            {
                break;
            }
        }

        rotState = false;

        //index값
        hand1_index += (dirType);
        hand2_index += (dirType);

        checkIndexOver();
        ChangeAnswer();

        //StartCoroutine("ChangeOfPeterPanTime__");
    }

    private void ChangeAnswer()
    {
        if (dirType == -1)
        {
            enterBtn_script.set_inputNum(0, GetHandValue(0));
            enterBtn_script.set_inputNum(1, GetHandValue(1));
        }
        else if (dirType == 1)
        {
            enterBtn_script.set_inputNum(2, GetHandValue(0));
            enterBtn_script.set_inputNum(3, GetHandValue(1));
        }
    }

    private void checkIndexOver()
    {
        //배열인덱스값을 넘어갔을때
        if (hand1_index > 11)
            hand1_index = 0;
        if (hand2_index > 11)
            hand2_index = 0;

        //배열인덱스값이 마이너스일때
        if (hand1_index < 0)
            hand1_index = 11;
        if (hand2_index < 0)
            hand2_index = 11;
    }

    private int GetHandValue(int i) //i는 hand1인지, hand2인지
    {
        if (i == 0) //hand1
            return ClockNum[hand1_index];
        else if (i == 1) //hand2
            return ClockNum[hand2_index];

        return 0; //오류
    }
}
