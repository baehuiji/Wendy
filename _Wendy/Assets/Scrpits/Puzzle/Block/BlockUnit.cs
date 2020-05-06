//
//2019-11-14
//[완]보드 타일화 유닛
//블럭배열을 초기화할시에 필요한 bool 변수 제거, 블럭으로 위치 변경.
//기존의 트리거로 블럭 위치 초기화한 부분을 사용 X, Block.cs 로 이동
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockUnit : MonoBehaviour
{
    //public GameObject _blockManager;
    private BlockManager script;
    public int unitNumber;

    void Start()
    {
        script = GameObject.Find("BlockManager").GetComponent<BlockManager>();
        string name = gameObject.name;
        unitNumber = int.Parse(name); //System.Convert.ToInt32(a);
    }

    void Update()
    {
    }

    void OnTriggerEnter(Collider coll) //이동했다는것
    {
        if (coll.gameObject.tag == "Block")
        {
            script.isBlock(unitNumber);

            //coll.gameObject.GetComponent<Block>().initializeLocation(unitNumber);
        }
    }

    void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.tag == "Block")
        {
            script.isEmpty(unitNumber);
        }
    }
}
