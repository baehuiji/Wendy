using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlodNote : MonoBehaviour
{
    public GameObject _aniFbx = null;
    public ViewNote_Ani_02 viewNote_script_clock;

    public GameObject _outlineObj = null; //외곽선을 위한 모델링  

    void Start()
    {
        _aniFbx.SetActive(false);
    }

    public void SetActive_Ani(bool active)
    {
        _aniFbx.SetActive(active);
    }

    public void SetActive_Outline(bool active)
    {
        _outlineObj.SetActive(active);
    }

    public bool openAni_Note()
    {
        return viewNote_script_clock.OpenAni_Note();
    }
    public bool endAni_Note()
    {
        return viewNote_script_clock.EndAni_Note();
    }
    public void startAni_Note()
    {
        viewNote_script_clock.StartAni_Note();
    }
}
