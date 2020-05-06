using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingVideo_Loading : MonoBehaviour
{
    private float FadeTime = 3f; // Fade효과 재생시간

    Image fadeImg;

    float start;
    float end;

    float time = 0f;

    private SeeEnding seeEnding_script;

    private GameMgr _GMgr;
    private FirstPersonCamera _fpCamCtrl;

    void Awake()
    {
        fadeImg = GetComponent<Image>();
        //InStartFadeAnim(); //Test

        seeEnding_script = GameObject.FindObjectOfType<SeeEnding>();

        _GMgr = GameObject.FindObjectOfType<GameMgr>();
        _fpCamCtrl = GameObject.FindObjectOfType<FirstPersonCamera>();
    }

    //페이드아웃
    public void InStartFadeAnim()
    {
        // - 스크립트해제 (키보드/카메라이동매니저)
        _fpCamCtrl.enabled = false;
        _GMgr.enabled = false;

        Color fadecolor = fadeImg.color;
        fadecolor.a = 0f;
        fadeImg.color = fadecolor;

        time = 0f;
        start = 0f;
        end = 1f;

        StartCoroutine(fadeIntanim());
    }

    IEnumerator fadeIntanim()
    {
        yield return 1f;

        Color fadecolor = fadeImg.color;

        while (fadecolor.a < 1f)
        {
            time += Time.deltaTime / FadeTime;
            fadecolor.a = Mathf.Lerp(start, end, time);
            fadeImg.color = fadecolor;
            yield return null;
        }

        fadecolor.a = 1f;
        fadeImg.color = fadecolor;

        seeEnding_script.playVideo();
    }
}
