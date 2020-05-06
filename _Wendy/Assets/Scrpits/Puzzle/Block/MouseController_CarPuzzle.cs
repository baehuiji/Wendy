//
//2019-11-05
//[완]자동차 퍼즐 조작
//바뀐 블럭 움직임 : 블럭을 클릭, 드래그한 방향으로 130. 
//                -> (기존의 블럭 움직임 : 블럭을 클릭하고 끌어다 움직이는 대로)
//Move부분 인자 추가
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController_CarPuzzle : MonoBehaviour
{
    public Camera _PuzzleCam = null;

    private GameObject target = null; //mouse target

    //private GameObject _blockPrefab;
    private GameObject[] _blocks;
    public GameObject _exit; //없어도 됨.
    public GameObject _me;

    private Vector3 MousePos;
    private bool _targetState;

    private Vector3 startPos;
    private Vector3 lastPos;
    private bool mouseUp;
    private bool mouseDrag;
    private Vector3 direction;

    private Block blockScript;

    private Vector3 subVecs;
    private Vector3 absVec;
    private float subXY;

    public bool GameClear;

    int layerMask;

    ChangeCam_1stage ChangeCam_script;

    void Start()
    {
        if (_blocks == null)
            _blocks = GameObject.FindGameObjectsWithTag("Block");

        _targetState = false;

        startPos = Vector3.zero;
        lastPos = Vector3.zero;
        direction = Vector3.zero;
        mouseUp = false;
        mouseDrag = false;

        blockScript = null;

        subVecs = Vector3.zero;
        absVec = Vector3.zero;
        subXY = 0f;

        GameClear = false;

        layerMask = 1 << LayerMask.NameToLayer("BlockPuzzle");

        ChangeCam_script = GameObject.FindObjectOfType<ChangeCam_1stage>();
    }

    void Update()
    {
        if (GameClear)
            return;

        if (Input.GetMouseButtonDown(0)) //OnMouseDown()
        {
            target = GetClickedObject();

            if (target != null)
            {
              if (target.transform.CompareTag("Exit"))
                {
                    // - 카메라 이동
                    ChangeCam_script.change_Camera(0);
                    return;
                }

                if (target.tag == "Block")
                {
                    //Debug.Log(target.GetComponent<Collider>().name);

                    _targetState = true; //타겟 선택
                    mouseDrag = true;

                    blockScript = target.GetComponent<Block>();

                    startPos = _PuzzleCam.ScreenToWorldPoint(Input.mousePosition); //혹은 target 의 위치
                }
            }
        }
        else if (true == Input.GetMouseButtonUp(0)) //OnMouseUp(), 단한
        {
            if (_targetState)
            {
                mouseUp = true;

                mouseDrag = false;
                lastPos = MousePos;

                _targetState = false;
            }
        }

        if (true == mouseDrag) //드래그를 한 상태
        {
            MouseDrag();
        }

        if (true == mouseUp)
        {
            subVecs = lastPos - startPos;

            absVec = subVecs;
            ChangeAbsolutevalue(ref absVec);

            subXY = absVec.x - absVec.y;

            if (subXY >= 0) //x축 이동
            {
                if (subVecs.x >= 0) //right
                {
                    direction = new Vector3(1f, 0f, 0f);
                    blockScript.Move(direction, 0, 1);
                }
                else //left
                {
                    direction = new Vector3(-1f, 0f, 0f);
                    blockScript.Move(direction, 0, 0);
                }
            }
            else //y축 이동
            {
                if (subVecs.y > 0) //up
                {
                    direction = new Vector3(0f, 1f, 0f);
                    blockScript.Move(direction, 1, 2);
                }
                else //down
                {
                    direction = new Vector3(0f, -1f, 0f);
                    blockScript.Move(direction, 1, 3);
                }
            }

            //각도 구하기
            //AngleInDeg(startPos, myPos);

            mouseUp = false;
        }
    }
    //void OnMouseDown() {}

    private GameObject GetClickedObject()
    {
        RaycastHit hit;

        GameObject target = null;

        Ray ray = _PuzzleCam.ScreenPointToRay(Input.mousePosition);

        if (true == (Physics.Raycast(ray.origin, ray.direction, out hit, 10f, layerMask)))
        {
            target = hit.collider.gameObject;
        }

        return target;
    }

    void MouseDrag()
    {
        //Debug.Log("Drag!!"); //print("Drag!!");
        //print(MousePos);

        //마우스 포인터 위치
        MousePos = _PuzzleCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                                                              Input.mousePosition.y,
                                                              -_PuzzleCam.transform.position.z));
        //Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z);

        //이동
        //target.transform.position = new Vector3(MousePos.x, MousePos.y, target.transform.position.z);
    }

    public void ChangeAbsolutevalue(ref Vector3 vec) //Mathf.Abs(n)
    {
        if (vec.x < 0)
            vec.x = vec.x * -1;

        if (vec.y < 0)
            vec.y = vec.y * -1;

        //z는 안 해도됨.
    }

    public static float AngleInRad(Vector3 vec1, Vector3 vec2)
    {
        return Mathf.Atan2(vec2.y - vec1.y, vec2.x - vec1.x);
    }

    public static float AngleInDeg(Vector3 vec1, Vector3 vec2)
    {
        return AngleInRad(vec1, vec2) * 180 / Mathf.PI;
    }
}
