using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class TitleButton : MonoBehaviour
{
    public GameObject _option_window;
    private bool _option_popup = false;
    //[SerializeField]
    //private LayerMask _ui_target_layer;
    //public Camera _ui_cam;

    public GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    public EventSystem m_EventSystem;
    public Transform[] _leaf_pos = new Transform[3];
    public GameObject _leaf_obj;
    public int _cur_index = -1;
    //private bool _title_btn_on = false;

    void Start()
    {
        // - 추후 보완, 변경 필요함
        Screen.SetResolution(1920, 1080, false);

        // - 커서
        Cursor.lockState = CursorLockMode.None; //마우스 해제
        // - 옵션창
        _option_window.SetActive(false);

        //_ui_cam = GameObject.Find("UI Camera").GetComponent<Camera>();

    }

    void Update()
    {
        //RaycastUI();
        if (!_option_popup)
            RaycasterUi();
    }

    public void StartButton()
    {
        // - 로딩씬
        SceneManager.LoadScene("01_Loading");
    }

    public void OptionButton()
    {
        _option_window.SetActive(true);
        _option_popup = true;
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void MainButton_Option()
    {
        _option_window.SetActive(false);
        _option_popup = false;
    }

    //void RaycastUI() //not working
    //{
    //    Ray ray = _ui_cam.ScreenPointToRay(Input.mousePosition); //마우스 위치를 Ray로 반환
    //    RaycastHit hit;
    //    if (Physics.Raycast(ray, out hit, Mathf.Infinity)) //, _ui_target_layer)) //레이캐스팅
    //    {
    //        Debug.Log("dddd");
    //        if (hit.transform.CompareTag("TitleBtn"))
    //        {
    //            Debug.Log("Check");
    //        }
    //    }

    //    //if (EventSystem.current.IsPointerOverGameObject()) //포인터 위치가 타겟 위치에 있을때 뚫지않게
    //    //{
    //    //}
    //    //else
    //    //{//UI 아님
    //    //}
    //}

    void RaycasterUi()
    {
        //if (Input.GetKey(KeyCode.Mouse0))
        if (EventSystem.current.IsPointerOverGameObject())
        {
            //Set up the new Pointer Event
            m_PointerEventData = new PointerEventData(m_EventSystem);
            //Set the Pointer Event Position to that of the mouse position
            m_PointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast(m_PointerEventData, results);

            //if (results.Count != 0) //results.Any())
            {
                //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
                foreach (RaycastResult result in results)
                {
                    if (result.gameObject.tag == "TitleBtn")
                    {
                        Debug.Log("Hit " + result.gameObject.name);
                        int index = result.gameObject.GetComponent<TitleBtn_Indexer>().GetPos_Leaf();

                        if (_cur_index != index)
                        {
                            _cur_index = index;
                            _leaf_obj.SetActive(true);
                            _leaf_obj.transform.position = _leaf_pos[_cur_index].position;
                        }
                    }
                    //else
                    //{
                    //    if (results.Count == 1)
                    //    {
                    //        Debug.Log("Hit " + result.gameObject.name);
                    //        if (_cur_index != -1)
                    //        {
                    //            _leaf_obj.SetActive(false);
                    //            _cur_index = -1;
                    //        }
                    //    }
                    //}
                }
            }
        }
        else // - 안됨..
        {
            if (_cur_index != -1)
            {
                _leaf_obj.SetActive(false);
                _cur_index = -1;
            }
        }
    }
}
