using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light_test : MonoBehaviour
{

    private GameObject target;

    public GameObject[] _Pointstate;

    public LayerMask layerMask; //추가***
    int layermask; //추가***
    public float maxdistance = 10f; //추가***

    private bool state; 

    public int LightNum; //추가***

    GameManager_proto gameManager; //추가***

    SoundController_proto soundController;

    ///[SerializeField] private로 받으면오류가 생긴다. 왜?
    ///->원래 오류생깁니다!, 
    ///[SerializeField] 쓰는 이유는 private 접근자를 유니티 인스펙터 창으로 볼수있게 만드는건데,
    ///유니티가 공식적으로 왠만하면 public을 사용하라고 했어요.
    ///그런데도 사람들이 쓰는 이유는 클래스에 public을 많이 쓰는건 객체지향적이지 않으면서 보안상 취약하기도 하고,
    ///개발 과정 중에 private 접근자에서 오류가 생기면 찾기 어렵기 때문에 오류를 찾기 위해서 검수용으로 많이 쓰여요.
    ///그런데 중요한건 public이 속도가 훨씬 빠릅니다..

    // Use this for initialization
    void Start()
    {
        state = false; //추가***

        layermask = 1 << LayerMask.NameToLayer("Light"); //추가***

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager_proto>();

        soundController = FindObjectOfType<SoundController_proto>();
    }

    private GameObject GetClickedObject()
    {
        RaycastHit hit;
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //마우스 포인트 근처 좌표를 만든다. 

        state = false; //추가***

        //if (true == (Physics.Raycast(ray.origin, ray.direction * 10, out hit)))   //마우스 근처에 오브젝트가 있는지 확인
        if (Physics.Raycast(ray.origin, ray.direction, out hit, maxdistance, layermask)) //추가***
        {
            //있으면 오브젝트를 저장한다.
            target = hit.collider.gameObject;
            state = true;  //추가***
        }

        //추가된 코드 설명***
        //유니티는 태그와 레이어로 오브젝트를 구분할 수 있습니다.
        //리셋버튼도 동일하게 추가되었습니다.
        //state와 layermask가 없었다면 마우스 커서에 맞닿은 젤 첫번째 콜라이더와 부딪치게 되는데 이때, 필요없는 오브젝트의 정보도 불러들입니다.
        //제가 카메라 움직임을 위해 충돌 영역만 있는 투명한 천막을 천장처럼 얹어놨어요 
        //      그래서 이전에 마우스에 닿는 부분은 전부 그 천막을 target 오브젝트로 받아서 리턴해줬을 거예요.
        //state는 이미 적어놓으신거를 조금만 손봤는데 state는 내가 지정한 레이어 상에 있는 오브젝트를 알맞게 얻었는지 체크하는 변수이고,
        //layermask는 말그래도 내가 지정한 레이어 외에는 충돌오브젝트를 얻지않게 제한하는 변수입니다.
        //Physics.Raycast함수가 유니티 상에 디게 많이 오버로딩되어있어요. 인자값을 무얼 받는지 나중에 확인해보세요!!

        return target;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            target = GetClickedObject();

            if (state == true) //주석해제***
            {
                if (target.Equals(gameObject))
                {
                    //print("마우스 입력 받았음");
                    //   if (state == true)
                    // {
                    if (_Pointstate[0].activeInHierarchy == true)
                    {
                        _Pointstate[0].gameObject.SetActive(false);

                        int index = 0;
                        if (LightNum <= 0)
                            index = 7;
                        else
                            index = LightNum - 1;
                        gameManager.LightPuzzleClearCheck[index] = false;
                    }
                    else
                    {
                        _Pointstate[0].gameObject.SetActive(true);

                        int index = 0;
                        if (LightNum <= 0)
                            index = 7;
                        else
                            index = LightNum - 1;
                        gameManager.LightPuzzleClearCheck[index] = true;
                    }

                    if (_Pointstate[1].activeInHierarchy == true)
                    {
                        _Pointstate[1].gameObject.SetActive(false);
                        gameManager.LightPuzzleClearCheck[LightNum] = false;
                        soundController.play_btnOff();
                    }
                    else
                    {
                        _Pointstate[1].gameObject.SetActive(true);
                        gameManager.LightPuzzleClearCheck[LightNum] = true;
                        soundController.play_btnOn();
                    }

                    if (_Pointstate[2].activeInHierarchy == true)
                    {
                        _Pointstate[2].gameObject.SetActive(false);

                        int index = 0;
                        if (LightNum >= 7)
                            index = 0;
                        else
                            index = LightNum + 1;
                        gameManager.LightPuzzleClearCheck[index] = false;
                    }
                    else
                    {
                        _Pointstate[2].gameObject.SetActive(true);

                        int index = 0;
                        if (LightNum >= 7)
                            index = 0;
                        else
                            index = LightNum + 1;
                        gameManager.LightPuzzleClearCheck[index] = true;
                    }

                    gameManager.ClearCheck();

                    /*
                    else { _Pointstate[0].gameObject.SetActive(true); }
                    print("사라져");
                    state = false;
                    */
                }
            }
        }

        /*
        else
        {
            if (_Pointstate[0].activeInHierarchy == true)
            {
                _Pointstate[0].gameObject.SetActive(false);
            }
            else { _Pointstate[0].gameObject.SetActive(true); }

            if (_Pointstate[1].activeInHierarchy == true)
            {
                _Pointstate[1].gameObject.SetActive(false);
            }
            else { _Pointstate[1].gameObject.SetActive(true); }

            if (_Pointstate[2].activeInHierarchy == true)
            {
                _Pointstate[2].gameObject.SetActive(false);
            }
            else { _Pointstate[2].gameObject.SetActive(true); }

            state = true;
        }
        */
    }
}


