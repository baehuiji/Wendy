using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager_proto : MonoBehaviour
{
    public GameObject Operationkey_d;
    public GameObject GameRules;
    public GameObject GameClear;
    public GameObject GameOver;
    public GameObject GamePlay;

    public Text timeText;
    private float timer;

    int r_min;
    int r_sec;

    bool gamestart;
    int count;

    bool gameclear;
    bool gameover;

    public bool[] LightPuzzleClearCheck;

    MouseAimCamera_temp mouseAimCamera;
    Player playerScript;

    private void Awake()
    {
        timer = 120f; //120

        r_min = Mathf.CeilToInt(timer) / 60;
        r_sec = Mathf.CeilToInt(timer) % 60;

        gamestart = false;
        gameclear = false;
        gameover = false;

        count = 0;

        LightPuzzleClearCheck = new bool[8];
        System.Array.Clear(LightPuzzleClearCheck, 0, LightPuzzleClearCheck.Length);

        mouseAimCamera = GameObject.Find("MainCamera").GetComponent<MouseAimCamera_temp>();
        playerScript = GameObject.Find("Player").GetComponent<Player>();

        Screen.SetResolution(1920, 1080, false);
    }

    private void Update()
    {
        if (gameover || gameclear)
            return;

        if (gamestart)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;

                r_sec = Mathf.CeilToInt(timer) % 60; // Mathf.CeilToInt(Time.deltaTime);

                if (r_sec == 59)
                {
                    if (r_min > 0)
                        r_min = Mathf.CeilToInt(timer) / 60;
                }
            }
            else
            {
                //Time.timeScale = 0;
                gameover = true;

                GameOver.gameObject.SetActive(true);

                mouseAimCamera.gameEnd = true;
                playerScript.gameEnd = true;
            }

            //출력
            timeText.text = string.Format("{0:0} : {1:00}", r_min, r_sec);
            //timeText.text = Mathf.Ceil(timer).ToString();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (count == 0)
                {
                    Operationkey_d.gameObject.SetActive(false);
                    GameRules.gameObject.SetActive(true);
                    count++;
                }
                else
                {
                    GameRules.gameObject.SetActive(false);
                    GamePlay.gameObject.SetActive(true);
                    gamestart = true;
                }
            }
        }
    }

    public void Go_TitleScene()
    {
        SceneManager.LoadScene("00_Title copy");
    }

    public void ClearCheck()
    {
        for (int i = 0; i < 8; i++)
        {
            if (!LightPuzzleClearCheck[i])
                return;
        }

        //Time.timeScale = 0;
        gameclear = true;

        GameClear.gameObject.SetActive(true);

        mouseAimCamera.gameEnd = true;
        playerScript.gameEnd = true;
    }
}


