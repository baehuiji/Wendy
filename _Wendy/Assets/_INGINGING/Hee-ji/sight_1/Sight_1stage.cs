using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight_1stage : MonoBehaviour
{
    public float ViewAngle;    //시야각
    public float ViewDistance; //시야거리

    public LayerMask TargetMask;    //Enemy 레이어마스크 지정을 위한 변수
    public LayerMask ObstacleMask;  //Obstacle 레이어마스크 지정 위한 변수

    private Transform _transform;

    private float minDist = 9f;

    private Camera mainCam;
    private int pre_ol_index = 0; //이전 아웃라인 인덱스 ***초기화
    private DrawOutline_HJ OutlineController;

    private GameObject _target_obj;

    private bool _check = false;

    void Awake()
    {
        _transform = GetComponent<Transform>();

        mainCam = Camera.main.GetComponent<Camera>();//Camera.main;
        OutlineController = GameObject.FindObjectOfType<DrawOutline_HJ>();
    }

    void Update()
    {
        DrawView();             //Scene뷰에 시야범위 그리기
        FindVisibleTargets();   //Enemy인지 Obstacle인지 판별

        //Find_();   //Enemy인지 Obstacle인지 판별
    }

    public Vector3 DirFromAngle(float angleInDegrees)
    {
        //탱크의 좌우 회전값 갱신
        angleInDegrees += transform.eulerAngles.y;
        //경계 벡터값 반환
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public void DrawView()
    {
        Vector3 leftBoundary = DirFromAngle(-ViewAngle / 2);
        Vector3 rightBoundary = DirFromAngle(ViewAngle / 2);
        Debug.DrawLine(_transform.position, _transform.position + leftBoundary * ViewDistance, Color.blue);
        Debug.DrawLine(_transform.position, _transform.position + rightBoundary * ViewDistance, Color.blue);
    }

    public void Find_()
    {
        //시야거리 내에 존재하는 모든 컬라이더 받아오기
        Collider[] targets = Physics.OverlapSphere(_transform.position, ViewDistance, TargetMask);
        //- 콜라이더 아이템인거로 추가
        if (targets.Length > 0)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                Transform target = targets[i].transform;
                //탱크로부터 타겟까지의 단위벡터
                Vector3 dirToTarget = (target.position - _transform.position).normalized;
                //_transform.forward와 dirToTarget은 모두 단위벡터이므로 내적값은 두 벡터가 이루는 각의 Cos값과 같다.
                //내적값이 시야각/2의 Cos값보다 크면 시야에 들어온 것이다.
                if (Vector3.Dot(_transform.forward, dirToTarget) > Mathf.Cos((ViewAngle / 2) * Mathf.Deg2Rad)) //시야각 범위 안인지
                //if (Vector3.Angle(_transform.forward, dirToTarget) < ViewAngle/2)
                {
                    float distToTarget = Vector3.Distance(_transform.position, target.position);
                    if (!Physics.Raycast(_transform.position, dirToTarget, distToTarget, ObstacleMask)) //시야 사이에 장애물이 있으면 통과 X
                    {
                        Debug.DrawLine(_transform.position, target.position, Color.red);
                    }
                }
            }
        }
    }

    public void FindVisibleTargets()
    {
        if (!_check)
            return;

        //360도 시야거리 내에 존재하는 모든 콜라이더 받아오기
        Collider[] targets = Physics.OverlapSphere(_transform.position, ViewDistance, TargetMask);


        //- 콜라이더 아이템인거로 추가
        if (targets.Length > 0)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                Transform target = targets[i].transform;

                //탱크로부터 타겟까지의 단위벡터
                Vector3 dirToTarget = (target.position - _transform.position).normalized;

                //내적값이 시야각/2의 Cos값보다 크면 시야에 들어온 것이다.
                if (Vector3.Dot(_transform.forward, dirToTarget) > Mathf.Cos((ViewAngle / 2) * Mathf.Deg2Rad)) //시야각 범위 안인지
                {
                    float distToTarget = Vector3.Distance(_transform.position, target.position); //플레이어 기준으로 검사타겟 사이의 거리
                    //Debug.DrawLine(_transform.position, target.position, Color.red);

                    if (_target_obj == null)
                    {
                        if (distToTarget < minDist) //타겟 오브젝트가 없을때, 최소 거리보다 가까울 경우
                        {
                            FindObstacle(dirToTarget, distToTarget, target, targets[i].gameObject);
                        }
                    }
                    else
                    {
                        float distTo_curTarget = Vector3.Distance(_transform.position, _target_obj.transform.position); //플레이어 기준, 지정한 타겟 사이의 거리 재설정

                        if (distToTarget < distTo_curTarget) //타겟 오브젝트가 있을때, 현재 타겟의 거리를 업데이트, 그리고 그것을 비교함
                        {
                            FindObstacle(dirToTarget, distToTarget, target, targets[i].gameObject);
                        }
                    }
                }
                else
                {
                    ItemPickUp pieceItem_script = targets[i].transform.GetComponent<ItemPickUp>();
                    if(pieceItem_script.outlineIndex == pre_ol_index)
                    {
                        _target_obj = null;
                        OutlineController.set_enabled(pre_ol_index, false);
                        //pre_ol_index = 0; //***초기화
                    }
                }
            }
        }
        else
        {
            _target_obj = null;
            minDist = 9f;

            //퍼픕릭 타겟이 널이 아니면 아웃라인업애기
            OutlineController.set_enabled(pre_ol_index, false);

            _check = false;

        }
    }

    void FindObstacle(Vector3 dir, float maxdist, Transform tr, GameObject newTarget)
    {
        if (!Physics.Raycast(_transform.position, dir, maxdist, ObstacleMask)) //시야 사이에 장애물이 있으면 통과 X
        {
            // Debug.DrawLine(_transform.position, tr.position, Color.red);

            // - 타겟고정
            _target_obj = newTarget;

            //이전거 아웃라인 없애고 새로 띄우기, 타겟퍼블릭으로
            // - 외곽선 ***
            ItemPickUp pieceItem_script = newTarget.transform.GetComponent<ItemPickUp>();
            OutlineController.set_enabled(pieceItem_script.outlineIndex, true);
            pre_ol_index = pieceItem_script.outlineIndex;
        }
    }

    public void setCheck(bool b)
    {
        _check = b;
    }
    public bool getCheck()
    {
        return _check;
    }
}