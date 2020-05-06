using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    public float FadeTime = 2f; // Fade효과 재생시간

    Image fadeImg;

    float start;
    float end;

    float time = 0f;

    bool isPlaying = false;

    void Awake()
    {
        fadeImg = GetComponent<Image>();

        //InStartFadeAnim();
    }

    //페이드인아웃
    public void InStartFadeAnim()
    {
        if (isPlaying == true) //중복재생방지
        {
            return;
        }

        Color fadecolor = fadeImg.color;
        fadecolor.a = 1f;
        fadeImg.color = fadecolor;

        start = 1f;
        end = 0f;

        StartCoroutine("fadeIntanim");
    }
    IEnumerator fadeIntanim()
    {
        isPlaying = true;

        Color fadecolor = fadeImg.color;

        time = 0f;
        while (fadecolor.a < 0.99f)
        {
            time += Time.deltaTime / FadeTime;

            fadecolor.a = Mathf.Lerp(start, end, time);

            fadeImg.color = fadecolor;

            yield return null;
        }

        time = 0f;
        fadecolor.a = 1f;
        fadeImg.color = fadecolor;

        start = 1f;
        end = 0f;

        yield return new WaitForSeconds(2f);

        while (fadecolor.a > 0.01f)
        {
            time += Time.deltaTime / FadeTime;

            fadecolor.a = Mathf.Lerp(start, end, time);
            //fadeImg.color = fadecolor;
            fadeImg.color = fadecolor;

            yield return null;
        }

        fadecolor.a = 0f;
        fadeImg.color = fadecolor;

        isPlaying = false;
    }
}
