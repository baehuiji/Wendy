using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Camera_Babysee : MonoBehaviour
{

    private GameObject _ClickDoor;
    public GameObject CameraMove;

    private bool check;

// Start is called before the first frame update
void Start()
    {
        check = true;

    }

    private GameObject GetClickedObject()
    {
        RaycastHit hitObj;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //마우스위치를 ray좌표에 넣는다.

        if (true == (Physics.Raycast(ray.origin, ray.direction * 10, out hitObj)))   //마우스 근처에 오브젝트가 있는지 확인

        {
            //있으면 오브젝트를 저장한다.
            _ClickDoor = hitObj.collider.gameObject;

        }

        return _ClickDoor;

    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _ClickDoor = GetClickedObject();

            if (_ClickDoor.Equals(gameObject))
            {
                Debug.Log("문 클릭됨 ");
                Debug.Log("애니메이션 실행코드진입 ");

                See_Wendy(true);

            }



            if (check == true)
            {
                See_Wendy(false);
                
            }
        }  
    }


    public void See_Wendy(bool is2Open)
    {
        Animator animator = CameraMove.GetComponent<Animator>();

        if (animator != null)
        {
            bool isOpen = animator.GetBool("isStart");

            animator.SetBool("isStart", !isOpen);
        }

    }

}
