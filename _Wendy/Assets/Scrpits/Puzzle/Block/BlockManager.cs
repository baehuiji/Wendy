//
//2019-11-14
//[완]보드 타일화
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    [SerializeField]
    private string arriveSound;

    private int[] blockArray = new int[36];
    public int[] originArray = new int[36];

    public GameObject blockParent;
    private Block[] block_scripts;

    void Start()
    {
        System.Array.Clear(blockArray, 0, blockArray.Length);

        block_scripts = blockParent.GetComponentsInChildren<Block>();
    }

    void Update()
    {

    }

    public void isBlock(int i)
    {
        blockArray[i] = 1;
    }

    public void isEmpty(int i)
    {
        blockArray[i] = 0;
    }

    public bool BlockIsExist(int l, int bt, int dt) //블럭 이동할때 (젤왼쪽상단블럭위치, 블럭타입, 방향) 값을 가지고 블럭이 있는지 검사
    {
        //블럭타입 bt 종류
        //0, 1, 4 가로
        //2, 3 세로

        //방향타입 dt
        //0 1 = 가로, 2 3 = 세로, 0 = 고정

        int start = l; //처음
        int end; //마지막

        if (bt < 2 || bt == 4) //가로
        {
            if (bt == 1) //3개짜리 블럭
                end = start + 2;
            else //2개짜리 블럭
                end = start + 1;

            if (dt == 0) //왼쪽으로 이동
            {
                int index = start - 1;

                if (index == 11 && bt == 4) //도착지점!! --이 부분에는 다른 사운드를 출력해주는것으로? 
                {
                    SoundManger.instance.PlaySound(arriveSound); // 사운드가 안남. 왜? -해결

                    return false; }

                if ((start % 6) != 0)
                {
                    if (blockArray[index] == 1)
                        return true;
                    else
                        return false;
                }
            }
            else if (dt == 1) //오른쪽으로 이동
            {
                int index = end + 1;

                if ((end + 1) % 6 != 0)
                {
                    if (blockArray[index] == 1)
                        return true;
                    else
                        return false;
                }
            }
        }
        else if (bt == 2 || bt == 3) //세로블럭
        {
            if (bt == 2) //2개짜리블럭
                end = start + 6;
            else //3개짜리블럭
                end = start + 6 * 2;

            if (dt == 2) //위쪽으로 이동
            {
                int index = start - 6;

                if ((start / 6) != 0 && index >= 0)
                {
                    if (blockArray[index] == 1)
                        return true;
                    else
                        return false;
                }
            }
            else if (dt == 3)//아래쪽으로 이동
            {
                int index = end + 6;

                if ((end / 6) != 5 && index < 36)
                {
                    if (blockArray[index] == 1)
                        return true;
                    else
                        return false;
                }
            }
        }

        return true;
    }

    public void Reset()
    {
        //blockParent.transform.Translate(Vector3.forward * 2f);

        for (int i = 0; i < block_scripts.Length; i++)
        {
            block_scripts[i].ResetPosition();
        }

        //blockParent.transform.Translate(Vector3.back * 2f);

        //System.Array.Clear(blockArray, 0, blockArray.Length);
        for (int i = 0; i < blockArray.Length; i++)
        {
            blockArray[i] = originArray[i];
        }
    }
}
