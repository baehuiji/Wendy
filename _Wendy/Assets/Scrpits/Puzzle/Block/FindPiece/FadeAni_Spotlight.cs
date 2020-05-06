using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeAni_Spotlight : MonoBehaviour
{
    private float FadeTime = 0.5f; // Fade효과 재생시간

    Image fadeImg;

    float start;
    float end;

    float time = 0f;

    public bool isPlaying = false;

    private Coroutine coroutine;

    void Awake()
    {
        fadeImg = GetComponent<Image>();
    }

    //페이드아웃
    public void InStartFadeAnim()
    {
        if (isPlaying == true) //중복재생방지
        {
            StopCoroutine(coroutine);
        }

        Color fadecolor = fadeImg.color;
        fadecolor.a = 0f;
        fadeImg.color = fadecolor;

        time = 0f;
        start = 0f;
        end = 0.5f;

        coroutine = StartCoroutine(fadeIntanim());
    }
    IEnumerator fadeIntanim()
    {
        isPlaying = true;

        Color fadecolor = fadeImg.color;

        while (fadecolor.a < 0.49f)
        {
            time += Time.deltaTime / FadeTime;
            fadecolor.a = Mathf.Lerp(start, end, time);
            fadeImg.color = fadecolor;
            yield return null;
        }

        fadecolor.a = 0.5f;
        fadeImg.color = fadecolor;

        isPlaying = false;
    }

    public void stop_coroutine()
    {
        if (isPlaying)
        {
            StopCoroutine(coroutine);
        }

        isPlaying = false;

        Color fadecolor = fadeImg.color;
        fadecolor.a = 0f;
        fadeImg.color = fadecolor;
    }
}
