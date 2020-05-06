using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear_Move : MonoBehaviour
{
    private bool gearOn = false;
    private bool inRotation = false;
    public int gearNumber; //인덱스

    public int matchNumber; // 1, 2, 3, 4
    public int matchType; // 1(이미지), 2(사운드)

    public GameObject linkedObjet;

    float speed = 200f;
    public Transform RotTarget; // 회전 타겟

    FramePuzzle_Controller fpController;

    bool delayOn = false; //맞추거나틀렸을때 톱니회전의 유예시간동악 true, 아닐땐 false //true일때

    Drawing_Move drawingMove_script = null;

    bool firstTime = false;

    void Start()
    {
        fpController = GameObject.FindObjectOfType<FramePuzzle_Controller>();

        //연결 오브젝트
        if (matchType == 1)
        {
            //drawingMove_script = linkedObjet.FindObjectOfType<Drawing_Move>(); //X , GameObject에만 가능한 find 문인갑다
            drawingMove_script = linkedObjet.GetComponent<Drawing_Move>();
        }
        else
        {

        }
    }

    void Update()
    {

    }

    public bool get_inRotation()
    {
        return inRotation;
    }

    private void set_gearOn(bool bOn)
    {
        gearOn = bOn;
    }
    public bool get_gearOn()
    {
        //if (!gearOn) //기어 활성화
        //{
        //    gearOn = true;



        //}
        //else //비활성화
        //{
        //    gearOn = false;
        //}

        return !gearOn;
    }

    public void set_gearNumber(int num)
    {
        gearNumber = num;
    }
    public int get_gearNumber()
    {
        return gearNumber;
    }
    public int get_matchNumber()
    {
        return matchNumber;
    }
    public int get_matchType()
    {
        return matchType;
    }

    public void set_delayOn(bool bdOn)
    {
        delayOn = bdOn;
    }
    public bool get_delayOn()
    {
        return delayOn;
    }

    public void gear_Active(bool first) //연결오브제 활성화
    {
        //1회인지
        firstTime = first;

        set_gearOn(true);

        StartCoroutine(gear_Rotate(1f));
        //StartCoroutine("GearRotate", 0.1f);

        if (matchType == 1) //이미지
        {
            //이미지 내리기 *************
            drawingMove_script.Play(true, firstTime); //안됨?
        }
        else if (matchType == 2) //사운드
        {
            //sound On
        }
    }
    public void gear_unActive() //연결오브제 비활성화
    {
        set_gearOn(false);
        StartCoroutine(gear_Rotate(-1f));
        //StartCoroutine("GearRotate", -0.1f);

        if (matchType == 1) //이미지
        {
            //이미지 올리기
            drawingMove_script.Play(false, firstTime);
        }
        else if (matchType == 2) //사운드
        {
            //sound Off
        }
    }

    IEnumerator gear_Rotate(float DirofRot) //DirofRot
    {
        inRotation = true;

        //DirofRot 
        if (DirofRot > 0f) //상수면
        {
            //O
            RotTarget.rotation = Quaternion.Euler(new Vector3(180f, 0, 0));

            //세모
            //RotTarget.Rotate(180f, 0f, 0f);

            //X
            //RotTarget.Rotate(Vector3.right, 180f); //X, 아예안먹힘
        }
        else if (DirofRot <= 0f) //음수면
        {
            //O
            RotTarget.rotation = Quaternion.Euler(new Vector3(0f, 0, 0));

            //세모
            //RotTarget.Rotate(1f, 0f, 0f);

            //X
            //RotTarget.Rotate(Vector3.right, 0f); //X
        }

        //RotTarget.rotation.eulerAngles = new Vector3(0, 90f * DirofRot, 0);
        while (transform.rotation.eulerAngles.x != RotTarget.rotation.eulerAngles.x)
        {
            //float step = DirofRot * speed * Time.deltaTime; //이렇게안해도됨
            float step = 1f * speed * Time.deltaTime;

            transform.rotation = Quaternion.RotateTowards(transform.rotation, RotTarget.rotation, step);

            yield return new WaitForSeconds(0.01f);
            //yield return new WaitForSecondsRealtime(0.000001f); //X
        }

        inRotation = false;

        if (delayOn)
        {
            fpController.moveAfter();
        }
    }
}
