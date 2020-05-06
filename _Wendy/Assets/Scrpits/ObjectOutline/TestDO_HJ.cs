using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDO_HJ : MonoBehaviour
{
    public Camera Camera;

    private GameObject target = null;

    //public LayerMask layerMask;
    private int layermask;

    public DrawOutline_HJ OutlineController;
    public int index;
    private bool isPointing = false; //아이템을 마우스포인터로 가리키는 상태
    private bool isSelected = false; //아이템을 클릭한 상태

    void Start()
    {
        layermask = 1 << LayerMask.NameToLayer("Item_HJ");
    }

    void Update()
    {
        //포인팅
        if (TargetCollideTouchpoint())
            isPointing = true;
        else
        {
            if (isPointing)
            {
                OutlineController.set_enabled(index, false);
                isPointing = false;
                target = null;
            }
        }


        //클릭
        if (Input.GetMouseButtonDown(0))
        {
            isSelected = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isSelected = false;
        }
    }

    public bool TargetCollideTouchpoint()
    {
        GameObject hitObj = null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 30, layermask))
        {
            Debug.DrawLine(ray.origin, hit.point);
            hitObj = hit.collider.gameObject;

            if (!GameObject.Equals(target, hitObj))
            {
                target = hitObj;

                TestItem_HJ testScript = target.GetComponent<TestItem_HJ>(); ;
                index = testScript.ItemType;

                //Debug.Log(index.ToString());
                //Debug.Log("===");

                OutlineController.set_enabled(index, true);
            }

            return true;
        }

        return false;
    }
}
