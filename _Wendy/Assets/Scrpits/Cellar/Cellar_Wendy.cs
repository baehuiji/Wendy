using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cellar_Wendy : MonoBehaviour
{

   // public GameObject timeover;
    public GameObject EWall;
    public GameObject SWall;

    GameOverManger gameoverStart;
    Cellar_Manager cellar_;

    public Image timeover;

    private Color aplha;
    float time = 0f;
    public float F_time = 4f;
    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);


    // Start is called before the first frame update
    void Start()
    {
        timeover.gameObject.SetActive(false);
        cellar_ = FindObjectOfType<Cellar_Manager>();
        gameoverStart = FindObjectOfType<GameOverManger>();

    }


    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Wall")
        {
            timeover.gameObject.SetActive(true);
            StartCoroutine(Title());


            cellar_.enabled = false;

        }
    }

    IEnumerator Title()
    {

        yield return new WaitForSeconds(1f);

        // 잠시 기다렸다가
        time = 0f;

        Color alpha = timeover.color;
        time = 0f;

        timeover.gameObject.SetActive(true);

        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            timeover.color = alpha;

            yield return waitTime;
        }

        gameoverStart.GameOver(1); //가랏! 게임오버!

    }

}
