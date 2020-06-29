using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SeeEnding : MonoBehaviour
{
    //public GameObject _Inventory;
    public Camera _uiCam;
    private Camera _mainCam;
    private Camera _endingCam;

    private VideoPlayer _video;
    public RawImage _rawImage;

    void Start()
    {
        _mainCam = Camera.main;

        _endingCam = GetComponent<Camera>();
        _video = GetComponent<VideoPlayer>();
    }

    void Update()
    {

    }

    public void playVideo()
    {
        // - 해제하기 (인벤토리,카메라,게임사운드매니저)     // *엔딩로딩스크립트에도 일부 스크립트 해제가 있다   
        //_uiCam.enabled = false; //_Inventory.SetActive(false);
        _mainCam.enabled = false;
        //_endingCam.enabled = true;

        // - 다른 스크립트 해제
        //~

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

        _rawImage.gameObject.SetActive(true);
        _rawImage.texture = _video.texture;
        _video.Play();

        while (_video.isPlaying)
        {
            //Debug.Log("동영상 재생 시간 : " + Mathf.FloorToInt((float)vidio.time));
            yield return null;
        }

        // - 영상이 끝났을때, 타이틀로 돌아가기
        SceneManager.LoadScene("00_Title");
    }
}
