using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewScene_Loading : MonoBehaviour
{
    private float FadeTime = 3f; // Fade효과 재생시간

    Image fadeImg;

    float start;
    float end;

    float time = 0f;

    //private Coroutine coroutine;

    //카메라,플레이어,인벤토리
    ActionController_02_VER2 actionCtrler_script;
    FirstPersonCamera fpCam_script;
    Player_HJ playerCtrler_script;
    FramePuzzle_ChangeCam fpChangeCam_script;
    GameMgr invenCtrler_script;
    FramePuzzle_Enter fpEnter_script;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked; //커서 고정

        fadeImg = GetComponent<Image>();

        actionCtrler_script = GameObject.FindObjectOfType<ActionController_02_VER2>();
        fpCam_script = GameObject.FindObjectOfType<FirstPersonCamera>();
        fpEnter_script = GameObject.FindObjectOfType<FramePuzzle_Enter>();

        playerCtrler_script = GameObject.FindObjectOfType<Player_HJ>();

        fpChangeCam_script = GameObject.FindObjectOfType<FramePuzzle_ChangeCam>();

        invenCtrler_script = GameObject.FindObjectOfType<GameMgr>();


        // - 카메라(3인칭/퍼즐),플레이어,인벤토리 작동불가
        actionCtrler_script.enabled = false;
        fpCam_script.enabled = false;
        playerCtrler_script.enabled = false;
        fpChangeCam_script.enabled = false;
        invenCtrler_script.enabled = false;
        fpEnter_script.enabled = false;

        InStartFadeAnim();
    }

    //페이드아웃
    public void InStartFadeAnim()
    {
        Color fadecolor = fadeImg.color;
        fadecolor.a = 1f;
        fadeImg.color = fadecolor;

        time = 0f;
        start = 1f;
        end = 0f;

        StartCoroutine(fadeIntanim());
    }

    IEnumerator fadeIntanim()
    {
        Color fadecolor = fadeImg.color;

        while (fadecolor.a > 0f)
        {
            time += Time.deltaTime / FadeTime;
            fadecolor.a = Mathf.Lerp(start, end, time);
            fadeImg.color = fadecolor;
            yield return null;
        }

        fadecolor.a = 0f;
        fadeImg.color = fadecolor;

        //고정해제
        actionCtrler_script.enabled = true;
        fpCam_script.enabled = true;
        playerCtrler_script.enabled = true;
        fpChangeCam_script.enabled = true;
        invenCtrler_script.enabled = true;
        fpEnter_script.enabled = true;
    }
}
