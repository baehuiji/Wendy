using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCam_1stage : MonoBehaviour
{
    private Camera mainCamera;
    private Camera bpCamera;

    private AudioListener mainListener;
    private AudioListener fpListener;

    public bool PuzzlPlay = false;

    FadeAni_Spotlight spotlight_script;
    ActionController_01 actionController;
    MouseController_CarPuzzle carPuzzle_script;
    Player_1stage playerController;
    void Start()
    {
        mainCamera = Camera.main;
        bpCamera = GetComponent<Camera>();

        mainListener = mainCamera.GetComponent<AudioListener>();
        fpListener = GetComponent<AudioListener>();

        //초기화
        mainListener.enabled = true;
        fpListener.enabled = false;

        spotlight_script = GameObject.FindObjectOfType<FadeAni_Spotlight>();
        actionController = mainCamera.GetComponent<ActionController_01>();
        carPuzzle_script = GameObject.FindObjectOfType<MouseController_CarPuzzle>();

        playerController = GameObject.FindObjectOfType<Player_1stage>();
    }


    public void change_Camera(int type)
    {
        if (type == 1) //퍼즐 카메라 on
        {
            actionController.enabled = false;
            carPuzzle_script.enabled = true;

            spotlight_script.InStartFadeAnim();

            PuzzlPlay = true;

            bpCamera.enabled = true;
            fpListener.enabled = true;

            //mainCamera.enabled = false;
            mainListener.enabled = false;

            playerController.enabled = false;
        }
        else //if(type == 0) //다시 돌아가기
        {
            carPuzzle_script.enabled = false;

            spotlight_script.stop_coroutine();

            PuzzlPlay = false;

            //mainCamera.enabled = true;
            mainListener.enabled = true;

            bpCamera.enabled = false;
            fpListener.enabled = false;

            StartCoroutine(TimetoWaitFor());
        }
    }

    public bool get_PuzzlPlay()
    {
        return PuzzlPlay;
    }

    IEnumerator TimetoWaitFor()
    {
        yield return new WaitForSeconds(0.1f);

        playerController.enabled = true;
        actionController.enabled = true;
    }
}
