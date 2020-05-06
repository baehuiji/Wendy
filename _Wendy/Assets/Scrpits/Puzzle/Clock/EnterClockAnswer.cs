using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterClockAnswer : MonoBehaviour
{
    // - 인스펙터창에 초기화
    //public int AnswerPN0; //삼각형, 역방향
    //public int AnswerPN1; //원,     역방향
    //public int AnswerPN2; //사각형, 정방향
    //public int AnswerPN3; //별      정방향

    public int[] answer_cp;

    public InputClockAnswer[] InputPNumArry;

    //private bool IsRightAnswer = false;

    void Start()
    {

    }

    void Update()
    {

    }

    public bool get_result()
    {
        // - 값비교
        //삼각형
        if (InputPNumArry[0].InputNum != answer_cp[0])
        {
            return false;
        }
        //원
        else if (InputPNumArry[1].InputNum != answer_cp[1])
        {
            return false;
        }
        //사각형
        else if (InputPNumArry[2].InputNum != answer_cp[2])
        {
            return false;
        }
        //별
        else if (InputPNumArry[3].InputNum != answer_cp[3])
        {
            return false;
        }

        // - 클리어!
        return true;
    }

    public void set_inputNum(int index, int num)
    {
        answer_cp[index] = num;
    }
}
