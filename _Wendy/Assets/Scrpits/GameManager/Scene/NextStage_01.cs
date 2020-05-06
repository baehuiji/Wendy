using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NextStage_01 : MonoBehaviour
{
    private float FadeTime = 3f; // Fade효과 재생시간

    Image fadeImg;

    float start;
    float end;

    float time = 0f;

    //private Coroutine coroutine;
    GameMgr invenCtrler_script;

    void Awake()
    {
        fadeImg = GetComponent<Image>();

        invenCtrler_script = GameObject.FindObjectOfType<GameMgr>();
    }

    //페이드아웃
    public void InStartFadeAnim()
    {
        Color fadecolor = fadeImg.color;
        fadecolor.a = 0f;
        fadeImg.color = fadecolor;

        time = 0f;
        start = 0f;
        end = 1f;

        invenCtrler_script.enabled = false;

        StartCoroutine(fadeIntanim());
    }

    IEnumerator fadeIntanim()
    {
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

        SceneManager.LoadScene("02_Stage");
    }
}
