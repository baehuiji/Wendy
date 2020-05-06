//
//2019-11-14
//[완]블럭 이동 관련 o 
//추가할 내용 : 도착지점 트리거에 오면 도착위치에 알맞게 좀더 이동
//블럭 위치를 저장한 배열 location을 public 으로 변경,
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    enum BlockType { H2, H3, V2, V3, Me };
    enum Axis { X, Y, Z };
    enum DirType { left, right, up, down, same };

    public float speed;

    private bool isMoving;

    private Vector3 myPos;
    private Vector3 direction;

    //[Range(-2.6f, 2.6f)]
    private Vector3 destination;

    private Axis axis;
    private BlockType blockType;
    public int bType; //유니티 인스펙터상 설정

    private bool clear;

    public int[] location; //private->public, 인스펙터상 설정
    private bool init;
    DirType dirType;

    private BlockManager blockManager;

    private MouseController_CarPuzzle mouseController;

    void Start()
    {
        speed = 10f;

        isMoving = false;
        direction = Vector3.zero;
        destination = Vector3.zero;

        blockType = (BlockType)bType;

        clear = false;

        //BlockUnit의 트리거로 초기화를 했을때 필요
        //if ((int)blockType % 2 == 0)
        //    location = new int[2];
        //else
        //    location = new int[3];
        //System.Array.Clear(location, 0, location.Length);
        //init = false;

        blockManager = GameObject.Find("BlockManager").GetComponent<BlockManager>();

        mouseController = GameObject.Find("MouseCotroller").GetComponent<MouseController_CarPuzzle>();
    }

    void Update()
    {
        if (clear)
            return; //전체로 바꾸기

        if (isMoving)
        {
            float step = speed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, destination, step);
            //transform.Translate(direction * step);

            if (transform.position == destination)
            {
                isMoving = false;

                //if (transform.position.x == -3.9f &&
                //    transform.position.y == 0.6500001f)       //클리어****************
                //    clear = true;
            }
        }
    }

    public void Move(Vector3 dir, int a, int d) //인자 값은 Vector3방향, 움직이는 축의 방향
    {
        axis = (Axis)a;
        dirType = (DirType)d;

        if (axis == Axis.X)
        {
            if (blockManager.BlockIsExist(location[0], (int)blockType, d)) //움직일수있는 위치인지 검사함
                return;

            direction = dir;
            //direction = dir.normalized;

            //if (!BeBlockedByWall())
            //    return;

            destination = transform.position + (direction * 1.3f);

            if (d == 0) //왼쪽으로 이동
            {
                if (location[0] == 12 && blockType == BlockType.Me) //도착지점!!
                {
                    destination = transform.position + (direction * 2.6f);
                    speed = 15f;

                    //destination += (direction * 1.3f);
                    mouseController.GameClear = true;
                }
            }

            Set_location();

            isMoving = true;
        }
        else //(axis == Axis.Y)
        {
            if (blockManager.BlockIsExist(location[0], (int)blockType, d))
                return;

            direction = dir;

            destination = transform.position + (direction * 1.3f);

            Set_location();

            isMoving = true;
        }
    }

    private bool BeBlockedByWall() //Mathf.Clamp //벽인지를 position 으로 검사하는 함수, 도착지점 위치설정 안되어있다.
    {
        if (axis == Axis.X)
        {
            if ((int)blockType < 2 || blockType == BlockType.Me)
            {
                if (destination.x >= -2.6f && destination.x <= 2.6f)
                    return true;
            }

            //if (blockType == BlockType.Me && direction.x == -1f) //도착지점
            //{
            //    if (transform.position.x == -2.6f &&
            //        transform.position.y == 0.6500001f)
            //        return true;
            //}
        }
        else //axis == Axis.Y
        {
            if (blockType == BlockType.V2 || blockType == BlockType.V3)
            {
                if (destination.y >= -3.25f && destination.y <= 3.25f)
                    return true;
            }
        }

        return false;
    }

    public int GetBlockType()
    {
        return (int)blockType;
    }

    public void initializeLocation(int l) //블럭위치 초기화
    {
        if (init)
            return;

        init = true;

        int _l = l;

        for (int i = 0; i < location.Length; i++)
        {
            if ((int)blockType < 2 || blockType == BlockType.Me)
            {
                location[i] = _l;
                _l++;
            }
            else if (blockType == BlockType.V2 || blockType == BlockType.V3)
            {
                location[i] = _l;
                _l += 6;
            }
        }
    }

    private void Set_location()
    {
        int bArry_index = location[0];

        if ((int)blockType < 2 || blockType == BlockType.Me)
        {
            //location 배열의 시작점
            if (dirType == DirType.left)    //0
                bArry_index -= 1;
            else if (dirType == DirType.right) //1
                bArry_index += 1;

            //이동한 위치에 대한 값을 location 배열에 채우기
            for (int i = 0; i < location.Length; i++)
            {
                location[i] = bArry_index;
                bArry_index++;
            }
        }
        else
        {
            if (dirType == DirType.up) //2
                bArry_index -= 6;
            else if (dirType == DirType.down) //3
                bArry_index += 6;

            for (int i = 0; i < location.Length; i++)
            {
                location[i] = bArry_index;
                bArry_index += 6;
            }
        }
    }
}
