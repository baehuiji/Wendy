using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeScene01 : MonoBehaviour
{

    private GameObject _Cleardoor;

    // Start is called before the first frame update
    void Start()
    {

    }

    private GameObject GetClickedObject()
    {

        RaycastHit hit;
        GameObject _Cleardoor = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //마우스 포인트 근처 좌표를 만든다. 

        if (true == (Physics.Raycast(ray.origin, ray.direction * 10, out hit)))   //마우스 근처에 오브젝트가 있는지 확인

        {
            //있으면 오브젝트를 저장한다.
            _Cleardoor = hit.collider.gameObject;

        }
        return _Cleardoor;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _Cleardoor = GetClickedObject();


            //if (_Cleardoor.Equals(gameObject))
            //{
            //    SceneManager.LoadScene("02_3_Stage");
            //}
        }
    }
}
