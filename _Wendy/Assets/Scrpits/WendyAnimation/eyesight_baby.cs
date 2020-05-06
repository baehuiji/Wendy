using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eyesight_baby : MonoBehaviour
{

    private GameObject _underdoor;

    [SerializeField]
    private GameObject go_Camera;

    private Transform CamValue;

 //   private float fTickTime;

    //   private Vector3 rotation;

    private GameObject GetClickedObject()
    {

        RaycastHit hit;
        GameObject _underdoor = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //마우스 포인트 근처 좌표를 만든다. 

        if (true == (Physics.Raycast(ray.origin, ray.direction * 10, out hit)))   //마우스 근처에 오브젝트가 있는지 확인

        {
            //있으면 오브젝트를 저장한다.
            _underdoor = hit.collider.gameObject;
        }


        return _underdoor;
    }



    // Start is called before the first frame update
    void Start()
    {

        // 카메라의 위치 저장 
        CamValue = go_Camera.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _underdoor = GetClickedObject();

                if (_underdoor.Equals(gameObject))
                {

                    go_Camera.transform.position = go_Camera.transform.position
                                                        + new Vector3(0, -7, 0);



            }
                ///카메라를 움직이는 코드를 넣자
                // rotation = rotation + new Vector3(0, -180, 0);


            }
    }

}
