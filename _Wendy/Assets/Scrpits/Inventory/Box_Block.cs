using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box_Block : MonoBehaviour
{

    private GameObject _BlockBox;
     Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("버튼은 클릭됨");

            _BlockBox = GetClickedObject();

            if (_BlockBox.Equals(gameObject))
            {
                Debug.Log("상자도 클릭됨");

               // anim.Play("box_open");

            }
        }

    }


    private GameObject GetClickedObject()
    {
        RaycastHit hitObj;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //새로 생성한 씬에 자동으로 들어있는 기본 카메라를 지우고 새 카메라를 만들었을 때 주로 발생한다


        if (true == (Physics.Raycast(ray.origin, ray.direction * 10, out hitObj)))    //마우스 근처에 오브젝트가 있는지 확
        {
            //있으면 오브젝트를 저장한다.
            _BlockBox = hitObj.collider.gameObject;

        }

        return _BlockBox;

    }

}


/*
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Babysee : MonoBehaviour
{

    public Animation anim;
    private GameObject _ClickDoor;



// Start is called before the first frame update
void Start()
    {
        // 다른 오브젝트의 애니메이션을 가지고 온다
        anim = GameObject.Find("Main Camera").GetComponent<Animation>();
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


        //rayInstance = Camera.main.ScreenPointToRay(Input.mousePosition);

        
        if (Input.GetMouseButtonDown(0))
        {
            _ClickDoor = GetClickedObject();



            if (_ClickDoor.Equals(gameObject))
            {
                Debug.Log("문 클릭됨 ");
                Debug.Log("애니메이션 실행코드진입 ");

                anim.Play("See_Baby");

            }
        }
        
    }


}
*/
