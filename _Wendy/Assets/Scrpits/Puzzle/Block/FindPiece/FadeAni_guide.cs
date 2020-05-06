using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeAni_guide : MonoBehaviour
{
    private float FadeTime = 1f; // Fade효과 재생시간

    Image fadeImg;

    float start;
    float end;

    float time = 0f;

    bool isPlaying = false;
    
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

        isPlaying = false;

        Color fadecolor = fadeImg.color;
        fadecolor.a = 1f;
        fadeImg.color = fadecolor;

        time = 0f;
        start = 1f;
        end = 0f;
        
        coroutine = StartCoroutine(fadeIntanim());
    }
    IEnumerator fadeIntanim()
    {
        isPlaying = true;

        Color fadecolor = fadeImg.color;

        yield return new WaitForSeconds(1f);

        while (fadecolor.a > 0.01f)
        {
            time += Time.deltaTime / FadeTime;
            fadecolor.a = Mathf.Lerp(start, end, time);
            fadeImg.color = fadecolor;
            yield return null;
        }

        fadecolor.a = 0f;
        fadeImg.color = fadecolor;

        isPlaying = false;
    }

    public void stop_coroutine()
    {
        StopCoroutine(coroutine);
        isPlaying = false;

        Color fadecolor = fadeImg.color;
        fadecolor.a = 0f;
        fadeImg.color = fadecolor;
    }
}
