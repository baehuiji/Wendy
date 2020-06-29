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

            _BlockBox = GetClickedObject();

            if (_BlockBox.Equals(gameObject))
            {

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

