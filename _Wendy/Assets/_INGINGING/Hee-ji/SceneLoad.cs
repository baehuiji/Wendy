using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SceneLoad : MonoBehaviour
{
    public Slider progressbar;
    public Text loadtext;

    public VideoPlayer videoPlayer;
    public bool videoEnd = false;

    public bool once = false;
    Coroutine coroutine_load;
    Coroutine coroutine_play;

    private void Start()
    {
        //if (!once)
        {
            //coroutine_load = StartCoroutine(LoadScene());
            coroutine_play = StartCoroutine(PlayVideo());
        }
    }

    private void Update()
    {
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync("01_Stage");
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            yield return null;
            if (progressbar.value < 0.9f)
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 0.9f, Time.deltaTime);
            }
            else if (operation.progress >= 0.9f)
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 1f, Time.deltaTime);
            }
            else if (progressbar.value >= 1f)
            {
                loadtext.text = "Complete";// "Press SpaceBar";
            }

            if (progressbar.value >= 1f && operation.progress >= 0.9f && videoEnd)
            {
                operation.allowSceneActivation = true;
            }
        }
    }

    IEnumerator PlayVideo()
    {
        while (videoPlayer.isPlaying)
        {
            yield return null;
        }

        videoEnd = true;

        if (coroutine_load == null)
            coroutine_load = StartCoroutine(LoadScene());
    }

    public void set_videoEnd()
    {
        videoEnd = true;
    }
}
