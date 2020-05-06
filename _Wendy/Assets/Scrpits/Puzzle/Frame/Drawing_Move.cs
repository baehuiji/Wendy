using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Drawing_Move : MonoBehaviour
{
    //이미지 관리

    public GameObject dtargetParent;
    private DrawingTarget[] dtarget;
    private Transform startMoveTarget; // startTarget   
    private Transform endMoveTarget; // endTarget

    private Transform start2; // startTarget   
    private Transform end2; // endTarget

    private Transform start3; // startTarget //-> 들어갈때 

    private Transform curTarget = null;

    public int drawingNumber; //인덱스

    public float speed = 3f;

    void Start()
    {

        dtarget = dtargetParent.GetComponentsInChildren<DrawingTarget>();
        //Arrays.sort(dtarget);

        startMoveTarget = dtarget[0].gameObject.transform;
        endMoveTarget = dtarget[1].gameObject.transform;

        start2 = dtarget[2].gameObject.transform;
        end2 = dtarget[3].gameObject.transform;

        start3 = dtarget[4].gameObject.transform;
    }

    void Update()
    {

    }

    IEnumerator drawing_Move(bool state, bool first)
    {
        //타겟 정하기
        if (state)
        {
            if (first)
                curTarget = endMoveTarget;
            else
                curTarget = end2;
        }
        else
        {
            //if (first)
            //    curTarget = startMoveTarget;
            //else
            //    curTarget = start2;

            if (first)
                curTarget = start2;
            else
                curTarget = start3;
        }

        //이동
        while (transform.position.y != curTarget.position.y)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, curTarget.position, step);

            yield return new WaitForSeconds(0.01f);
        }

        if (!state)
            transform.position = startMoveTarget.position;
    }

    public void Play(bool s, bool first)
    {
        StartCoroutine(drawing_Move(s, first));
    }
}
