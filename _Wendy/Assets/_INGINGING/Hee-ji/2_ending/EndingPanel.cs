using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingPanel : MonoBehaviour
{
    public VideoPlayer _video;
    //public RawImage _rawImage;

    private float FadeTime = 3f; // Fade효과 재생시간

    Image fadeImg;

    float start;
    float end;

    float time = 0f;

    void Start()
    {
        fadeImg = GetComponent<Image>();
        playVideo();
    }

    void Update()
    {

    }

    public void playVideo()
    {
        //사운드 @

        // - 영상 시작하기
        StartCoroutine(check_videoState());
    }
    IEnumerator check_videoState()
    {
        _video.Prepare();

        WaitForSeconds waitTime = new WaitForSeconds(1.0f);

        while (!_video.isPrepared)
        {
            //Debug.Log("동영상 준비중");
            yield return waitTime;
        }
        
        //_rawImage.texture = _video.texture;
        //_video.Play();

        while (_video.isPlaying)
        {
            //Debug.Log("동영상 재생 시간 : " + Mathf.FloorToInt((float)vidio.time));
            yield return null;
        }

        // - 페이드
        InStartFadeAnim();
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

        StartCoroutine(fadeIntanim());
    }
    IEnumerator fadeIntanim()
    {
        yield return new WaitForSeconds(1f);

        Color fadecolor = fadeImg.color;

        while (fadecolor.a < 1f)
        {
            time += Time.deltaTime / FadeTime;
            fadecolor.a = Mathf.Lerp(start, end, time);
            fadeImg.color = fadecolor;
            //yield return null;
            yield return new WaitForSeconds(0.01f);
        }

        fadecolor.a = 1f;
        fadeImg.color = fadecolor;

        // - 영상이 끝났을때, 타이틀로 돌아가기
        SceneManager.LoadScene("00_Title");
    }
}
