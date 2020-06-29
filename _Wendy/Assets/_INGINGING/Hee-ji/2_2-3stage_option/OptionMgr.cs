using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 타이틀화면에서는 TitleButton과 비슷한 역할을 함
public class OptionMgr : MonoBehaviour
{
    OptionManager _option_manager;

    Camera _mainCam;
    public Camera _optionCam;

    void Start()
    {
        _option_manager = FindObjectOfType<OptionManager>();

        _mainCam = Camera.main;
    }

    void Update()
    {
        
    }

    // - 씬 초반 초기화
    public void Init()
    {

    }

    // - 팝업되었을때
    public void PopUpPanel()
    {
        _mainCam.gameObject.SetActive(false);
        _optionCam.gameObject.SetActive(true);
    }
    public void ReturnOrigin() //원상태복귀
    {
        _mainCam.gameObject.SetActive(true);
        _optionCam.gameObject.SetActive(false);
    }
}
