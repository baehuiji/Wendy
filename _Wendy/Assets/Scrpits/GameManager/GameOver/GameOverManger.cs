using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManger : MonoBehaviour
{

    public GameObject PlayerObj; //게임 오브젝트- 플레이어. 여기서 수정 
    public GameObject scriptModle;
    public GameObject mainCamera;

    public RectTransform LPanel;
    public RectTransform RPanel;

    public Image SheGone;
    private Color aplha;
    float time = 0f;
    public float F_time = 4f;
    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    Vector2 LPanelPos;
    Vector2 RPanelPos;

    Animator _animator = null;
    bool SetClick = false;
    // Start is called before the first frame update


    // Update is called once per frame
    // 특정 조건을 만족하면  뜨는데 그걸 바꿔서.. (에휴 히믇ㄹ다
    // 만약 게임 오버가 되면 패널이 뜹니다.
    // 그리고 몇초 딜레이 뒤 
    // 두개의 하얀 패널이 움직이더니 쾅! 조금 세심히 움직이는걸로? - 아냐 이건 그냥 빠르게 쾅하는게 나을듯 그냥 코드로....
    // 그리고 She's gone 패널의 투명도를 조절해서! 헤헤! 이러면 금방 끝이군!


    void Start()
    {
        _animator = PlayerObj.GetComponent<Animator>();

    }
    void Update()
    {
        if (SetClick)
        {
            if (Input.GetMouseButton(0))
            {
                SceneManager.LoadScene("00_Title");
            }
        }
        else
            return;
    }


    public void GameOver(int a)
    {
        StartCoroutine(gameover());
    }

    IEnumerator gameover()
    {

        _animator.SetBool("IsWalking", false);
        scriptModle.GetComponent<Player_HJ>().enabled = false;
        //     scriptModle.GetComponent<Collider>().enabled = false;
        mainCamera.gameObject.GetComponent<FirstPersonCamera>().enabled = false;


        yield return new WaitForSeconds(2.5f);

        // 잠시 기다렸다가
        time = 0f;

        while (true) // 선형보간이 진행됩니다. 선형보간의 이동이 끝날때까지! 
        {
            time += Time.deltaTime / F_time;

            LPanel.anchoredPosition = Vector2.Lerp(LPanel.anchoredPosition, new Vector2(-480, 0), time);
            RPanel.anchoredPosition = Vector2.Lerp(RPanel.anchoredPosition, new Vector2(480, 0), time);
            //   Wall_E.transform.position = Vector3.Lerp(Wall_E.transform.position, E_WallStop, t);

            yield return waitTime;

            // 조금 있다가, break! 



            if (Vector3.Distance(LPanel.anchoredPosition, new Vector2(-480, 0)) < 0.1f)
            {

                break;

            }



        }
        yield return new WaitForSeconds(1f);

        Color alpha = SheGone.color;
        time = 0f;

        SheGone.gameObject.SetActive(true);

        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            SheGone.color = alpha;

            yield return waitTime;
        }

        SetClick = true;
    }



    //일시 정지 
    //흠... 이상태에서 클릭을 하면 메인화면으로 되돌아감 


}