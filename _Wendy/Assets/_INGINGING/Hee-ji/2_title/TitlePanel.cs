using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitlePanel : MonoBehaviour
{
    private float FadeTime = 3f; // Fade효과 재생시간

    Image fadeImg;

    float start;
    float end;

    float time = 0f;

    void Start()
    {
        fadeImg = GetComponent<Image>();

        InStartFadeAnim();
    }

    void Update()
    {

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
        yield return new WaitForSeconds(1f);

        Color fadecolor = fadeImg.color;

        while (fadecolor.a > 0.01f)
        {
            time += Time.deltaTime / FadeTime;
            fadecolor.a = Mathf.Lerp(start, end, time);
            fadeImg.color = fadecolor;
            //yield return null;
            yield return new WaitForSeconds(0.01f);
        }

        fadecolor.a = 0f;
        fadeImg.color = fadecolor;        
    }
}
