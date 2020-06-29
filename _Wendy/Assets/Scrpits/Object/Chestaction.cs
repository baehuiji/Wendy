using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chestaction : MonoBehaviour
{
    //private Quaternion Right = Quaternion.identity;


    [SerializeField]
    private string openSound = "OpenDesk";

    [SerializeField]
    private string closeSound = "CloseDesk";

    [SerializeField]
    private string openSlideSound = "OpenSlideDesk";

    [SerializeField]
    private string closeSlideSound = "CloseSlideDesk";


    public int Chest_number;
    //public GameObject MoveChest;

    // - 서랍 타입
    public int type = 0;

    //여닫이문
    public float angle;
    Quaternion targetSet;
    Quaternion bRotation;
    //미닫이문
    Vector3 start_t;
    public Transform end_t;
    public GameObject target;

    // - 상태
    bool CheckState = false;
    bool moveOnState = false;
    bool moveOffState = false;

    float time = 0f;
    float F_time = 0.5f;
    float SetTimeState = 0f;
    float checkangle;

    // 해제해야하는것
    Collider _collider;

    // - 코루틴
    private Coroutine coroutine;

    void Start()
    {
        targetSet = Quaternion.Euler(0, 0, 0);
        bRotation = Quaternion.Euler(new Vector3(0, angle, 0));
        SetTimeState = time;

        _collider = GetComponentInChildren<Collider>();
    }

    void Update()
    {
        //if (moveOnState)
        //{
        //    time += Time.deltaTime / F_time;
        //    this.transform.rotation = Quaternion.Slerp(targetSet, bRotation, time);
        //    checkangle = Quaternion.Angle(targetSet, MoveChest.transform.rotation);

        //    if (checkangle >= Mathf.Abs(angle))
        //    {
        //        //moveOnState = false;
        //        //time = 0f;

        //        // - 해제
        //        _collider.enabled = false;
        //        this.enabled = false;

        //        return;
        //    }
        //}
        //if(moveOffState)
        //{
        //    time += Time.deltaTime / F_time;
        //    this.transform.rotation = Quaternion.Slerp(bRotation, targetSet, time);
        //    checkangle = Quaternion.Angle(targetSet, MoveChest.transform.rotation);

        //    if (checkangle <= 0)
        //    {
        //        CheckState = false;
        //        moveOffState = false;
        //        time = 0f;
        //        return;
        //    }
        //}


        /*
        while (SetTimeState < 1f)
        {
            //Debug.Log("실행중");
            time += Time.deltaTime / F_time;
            this.transform.rotation = Quaternion.Slerp(targetSet, bRotation, time);
            SetTimeState += Time.deltaTime / F_time;
        }
        */
    }

    public void Start_action(int type)
    {
        if (type == 1)
        {
            if (!moveOnState)
            {
                //회전
                coroutine = StartCoroutine(RotateDrawer());

                return;
            }
            else
            {
                return;
            }
        }
        else if (type == 2)
        {
            if (!moveOnState)
            {
                //이동
                start_t = target.transform.position;
                coroutine = StartCoroutine(MoveDrawer());

                return;
            }
            else
            {
                return;
            }
        }
    }

    IEnumerator MoveDrawer()
    {
        moveOnState = true;

        // - 해제
        _collider.enabled = false;

        SoundManger.instance.PlaySound(openSlideSound);

        while (true)
        {
            // - 이동
            time += Time.deltaTime / (F_time * 2);
            //float step_m = moveSpeed * speedFactor * Time.deltaTime;
            target.transform.position = Vector3.MoveTowards(start_t, end_t.position, time);

            //if (checkangle >= Mathf.Abs(angle))
            if (Vector3.Distance(target.transform.position, end_t.position) < 0.001f)
            {
                this.enabled = false;

                break;
            }

            yield return new WaitForSeconds(0.01f);
        }
        //moveOnState = false;
    }

    IEnumerator RotateDrawer()
    {
        moveOnState = true;

        // - 해제
        _collider.enabled = false;

        SoundManger.instance.PlaySound(openSound);

        while (true)
        {
            time += Time.deltaTime / F_time;
            this.transform.rotation = Quaternion.Slerp(targetSet, bRotation, time);
            checkangle = Quaternion.Angle(targetSet, transform.rotation);

            if (checkangle >= Mathf.Abs(angle))
            {
                this.enabled = false;

                break;
            }

            yield return new WaitForSeconds(0.01f);
        }
        //moveOnState = false;
    }


}



