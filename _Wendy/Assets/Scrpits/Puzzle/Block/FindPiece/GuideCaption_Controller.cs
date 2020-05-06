﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideCaption_Controller : MonoBehaviour
{
    Image _subImg;

    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;

    void Start()
    {
        _subImg = GetComponent<Image>();

        change_sprite(1);
    }

    void Update()
    {

    }

    public void change_sprite(int number)
    {
        switch (number)
        {
            case 1:
                _subImg.sprite = sprite1; // 인벤 열기
                break;
            case 2:
                _subImg.sprite = sprite2; // 퍼즐조각 필요
                break;
            case 3:
                _subImg.sprite = sprite3; // 퍼즐조각 습득
                break;
            default:
                _subImg.enabled = false;
                break;
        }

    }
}