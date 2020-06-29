// 안씀

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PrologueController : MonoBehaviour
{
    private VideoPlayer m_VideoPlayer;
    SceneLoad sceneLoad_script;

    void Awake()
    {
        m_VideoPlayer = GetComponent<VideoPlayer>();
        m_VideoPlayer.loopPointReached += OnMovieFinished;

        sceneLoad_script = GameObject.FindObjectOfType<SceneLoad>();
    }

    void OnMovieFinished(VideoPlayer player)
    {
        sceneLoad_script.set_videoEnd();

        player.Stop();

        //1
        //if ((ulong)videoPlayer.frame == videoPlayer.frameCount)
        //{
        //    //Video Finished
        //}

        //2
        //while (videoPlayer.isPlaying)
        //{
        //    yield return null;
        //}
    }
}
