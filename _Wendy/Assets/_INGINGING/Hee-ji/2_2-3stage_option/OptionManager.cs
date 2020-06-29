using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    public static OptionManager _option_manager = null;

    // * 중앙값 채워 넣기
    public float _music_volume;
    public float _fx_volume;
    public float _brightness_volume = 1f;

    void Awake()
    {
        if (_option_manager == null)
        {
            _option_manager = this;
        }
        else if (_option_manager != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

    }

    void Update()
    {

    }

    // - 값을 바꿀때
    public void SetMusicVolume(float value)
    {
        _music_volume = value;
    }
    public void SetFXVolume(float value)
    {
        _fx_volume = value;
    }
    public void SetBrightnessVolume(float value)
    {
        _brightness_volume = value;
    }

    // - 
}
