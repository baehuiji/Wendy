using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButton : MonoBehaviour
{

    public GameObject[] _LightButton;
    private GameObject _ResetButton;

    public LayerMask layerMask; //추가***
    int layermask; //추가***
    public float maxdistance = 10f; //추가***

    private bool state; //추가***

    SoundController_proto soundController;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i <= 7; i++)
        {
            // Debug.Log(i*2);
            _LightButton[i].gameObject.SetActive(false);
            // 전부다 꺼져있던 걸로 시작
        }

        state = false; //추가***
        layermask = 1 << LayerMask.NameToLayer("Light"); //추가***

        soundController = FindObjectOfType<SoundController_proto>();
    }

    private GameObject GetClickedObject()
    {
        RaycastHit hit;
        GameObject _ResetButton = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //마우스 포인트 근처 좌표를 만든다. 

        state = false; //추가***

        //if (true == (Physics.Raycast(ray.origin, ray.direction * 10, out hit)))   //마우스 근처에 오브젝트가 있는지 확인
        if (Physics.Raycast(ray.origin, ray.direction, out hit, maxdistance, layermask)) //추가***
        {
            //있으면 오브젝트를 저장한다.
            _ResetButton = hit.collider.gameObject;
            state = true;  //추가***
        }

        return _ResetButton;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _ResetButton = GetClickedObject();

            if (state == true) //추가***
            {
                if (_ResetButton.Equals(gameObject))
                {
                    soundController.play_reset();

                    for (int i = 0; i <= 7; i++)
                    {
                        //  Debug.Log(i);
                        _LightButton[i].gameObject.SetActive(false);
                        // Debug로 다 정상적으로 돌아가는 것을 확인
                        // 다만 비활성화/활성화 오브젝트가 섞여있을시
                        // 강제로 비활성화된 오브젝트는 활성화, 활성화된 오브젝트는 활성화 되는 오류 존재 
                        // !!!오류해결!!!
                    }
                }
            }
        }
    }
}
