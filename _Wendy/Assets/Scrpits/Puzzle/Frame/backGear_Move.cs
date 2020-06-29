using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backGear_Move : MonoBehaviour
{
    [SerializeField]
    private string gearBackSound = "PP_gear2";

    private bool gearOn = false;
    private bool inRotation = false;
    public int backgearNumber; //인덱스

    float speed = 190f;
    public Transform RotTarget; // 회전 타겟
    //public Transform startTarget; // 시작점

    public float cir_dir; //-1, 1 , 0 

    bool firstTime = false;

    private Coroutine coroutine;

    private bool once = false;

    void Start()
    {

    }

    public void set_gearNumber(int num)
    {
        backgearNumber = num;
    }

    void Update()
    {

    }

    public void start_Rotation(bool state, bool first)
    {
        once = first;
        gearOn = state;

        if (inRotation) //중복재생방지
        {
            StopCoroutine(coroutine);
            inRotation = false;

            //if (gearOn)
            //    return;
        }

        coroutine = StartCoroutine(backGear_Rotate());
    }

    IEnumerator backGear_Rotate()
    {
        inRotation = true;

        if (gearOn)
        {
            SoundManger.instance.PlaySound(gearBackSound);

            //RotTarget.rotation = Quaternion.Euler(new Vector3(180f, 0, 0));

            if (once)
            {
                if (cir_dir > 0f)
                {
                    RotTarget.Rotate(Vector3.left * 90f);

                    //RotTarget.rotation = Quaternion.Euler(new Vector3(180f, 0, 0));
                }
                else if (cir_dir < 0f)
                {
                    RotTarget.Rotate(Vector3.right * 90f);
                    //RotTarget.rotation = Quaternion.Euler(new Vector3(-180f, 0, 0));
                }
            }
            else
            {
                if (cir_dir > 0f)
                {
                    RotTarget.Rotate(Vector3.left * 90f);
                }
                else if (cir_dir < 0f)
                {
                    RotTarget.Rotate(Vector3.right * 90f);
                }
            }
        }
        else
        {
            SoundManger.instance.PlaySound(gearBackSound);

            //RotTarget.rotation = Quaternion.Euler(new Vector3(0f, 0, 0));

            if (cir_dir > 0f)
            {
                RotTarget.Rotate(Vector3.right * 179f);
            }
            else if (cir_dir < 0f)
            {
                RotTarget.Rotate(Vector3.left * 179f);
            }
        }

        while (transform.rotation.eulerAngles.x != RotTarget.rotation.eulerAngles.x)
        {
            float step = 1f * speed * Time.deltaTime;

            transform.rotation = Quaternion.RotateTowards(transform.rotation, RotTarget.rotation, step);

            yield return new WaitForSeconds(0.01f);
        }

        inRotation = false;
    }
}
