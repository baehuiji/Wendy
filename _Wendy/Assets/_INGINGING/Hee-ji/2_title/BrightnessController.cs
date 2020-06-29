using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrightnessController : MonoBehaviour
{
    private Slider _slider;

    // - 밝기 조절
    float intensityValue;  //게임매니저로 옮기기
    private float _sliderSize;
    public RectTransform _fillRect;

    // - 다른 씬에도 적용하기 
    //public static GameManager _instance;

    void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    void Start()
    {
        //_instance = null;
    }

    public void BrightnessSlider()
    {
        intensityValue = _slider.value;
        RenderSettings.ambientIntensity = intensityValue;
    }
    public void UpdateSliderSense()
    {
        if (_sliderSize == 0)
        {
            //_sliderSize = GetComponent<RectTransform>().rect.width;
            //_sliderSize = 1; // _fillRect.rect.width; //260 _sliderSize는 초기 슬라이더 사이즈 크기
            //_sliderSize = _sliderSize / (_slider.maxValue - _slider.minValue);
        }

        // - 1
        //_slider.fillRect.rotation = new Quaternion(0, 0, 0, 0);
        //_slider.fillRect.pivot = new Vector2(_slider.fillRect.transform.parent.localPosition.x, _slider.fillRect.pivot.y);
        //if (_slider.value > 0)
        //{
        //    _slider.fillRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _sliderSize * _slider.value);
        //}
        //else
        //{
        //    _slider.fillRect.Rotate(0, 0, 180);
        //    _slider.fillRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, -1 * _sliderSize * _slider.value);
        //}
        //_slider.fillRect.localPosition = new Vector3(0, 0, 0);

        // - 2
        _fillRect.rotation = new Quaternion(0, 0, 0, 0);
        _fillRect.pivot = new Vector2(_fillRect.transform.localPosition.x, _fillRect.pivot.y); // new Vector2(_fillRect.transform.parent.localPosition.x, _fillRect.pivot.y);
        if (_slider.value > 1)
        {
            _fillRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _sliderSize * _slider.value);
        }
        else
        {
            _fillRect.Rotate(0, 0, 180);
            //_fillRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, -1 * _sliderSize * _slider.value);
            _fillRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _sliderSize * -_slider.value);
        }
        _fillRect.localPosition = new Vector3(0, 0, 0);
    }


    //public float GammaCorrection;
    //public Rect SliderLocation;
    //float rgbValue = 0.5f;
    //void Update()
    //{
    //    RenderSettings.ambientLight = new Color(GammaCorrection, GammaCorrection, GammaCorrection, 1.0f);
    //}
    //void OnGUI()
    //{
    //    //1
    //    GammaCorrection = GUI.HorizontalSlider(SliderLocation, GammaCorrection, 0, 1.0f);
    //    //2
    //    rgbValue = GUI.HorizontalSlider(new Rect(Screen.width / 2 - 50, 90, 100, 30), rgbValue, 0f, 1f);
    //    RenderSettings.ambientLight = new Color(rgbValue, rgbValue, rgbValue, 1);
    //}
}
