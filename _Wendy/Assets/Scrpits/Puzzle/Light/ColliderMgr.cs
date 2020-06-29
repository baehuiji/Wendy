using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColliderMgr : MonoBehaviour
{
    //  public Animation Tinkerbell_ani;
    // Start is called before the first frame update

    public GameObject[] lamp;

    public GameObject[] PointLampModl;
    //Material[] PointLamp_Material;
    Renderer[] PointLamp_Renderer;
    Color alphaColor;

    public GameObject bell_Doll;

    public GameObject MCamera;

    [SerializeField]
    private float range;

    [SerializeField]
    private LayerMask layer;

    //public Text actionText;

    [SerializeField]
    private string Lamp_Sound;

    [SerializeField]
    private string ResetLamp_Sound;

    private bool pickupActivated = false;//false;
    private RaycastHit puzzleInfo;

    private int CheckOnL = 0;

    public GameObject ResetLeber;
    LampLight lamplight;
    Animator animator;
    private bool LaberOn = false;

    //public GameObject[] _Pointstate;

    // - 외곽선
    private DrawOutline_HJ OutlineController;
    public int pre_ol_index = -1; //이전 아웃라인 인덱스
    private bool outline_active = false;

    // - 클릭버튼
    public GameObject actionCaption;

    // - 장애물, 벽
    ObstacleReader obstacleReader_script;
    bool coverCheck = false; //막고잇으면 TRUE
    int obstacle_layer;

    void Start()
    {
        animator = ResetLeber.GetComponent<Animator>();
        //  renderer = PointLampModl.GetComponent<Renderer>();

        PointLamp_Renderer = new Renderer[PointLampModl.Length];

        //PointLamp_Material = new Material[PointLampModl.Length];

      //  alphaColor = new Color[PointLampModl.Length];

        for (int i = 0; i< PointLampModl.Length; i++)
        {
            PointLamp_Renderer[i] = PointLampModl[i].GetComponent<Renderer>(); //메탈릭
            //PointLamp_Material[i] = PointLampModl[i].GetComponent<Renderer>().material;
           // alphaColor[i] = PointLamp_Material[i].GetColor("MainColor");
        }
       // PointLamp_Material[5].SetColor("_Color", Color.red);
        //     alphaColor[5].a = 0.2f;
        //      PointLamp_Renderer[5].material.color = alphaColor[5];


        //alphaColor[5].a = 0.3f;
        //PointLamp_Renderer[5].material.color = alphaColor[5];

        //PointLamp_Renderer[0] = PointLampModl[0].GetComponent<Renderer>();

        //alphaColor[5] = PointLampModl[5].GetComponent<Renderer>().material.color;
        //alphaColor[5].a = 0.3f;
        // PointLampModl[5].GetComponent<Renderer>().material.color = Color.red;
        //PointLamp_Material[0] = PointLampModl[0].GetComponent<Renderer>().material; //메탈릭
        //PointLamp_Renderer[0] = PointLampModl[0].GetComponent<Renderer>();

        //PointLamp_Material[5] = PointLampModl.GetComponents<Renderer>().material; //메탈릭
        //PointLamp_Renderer[5] = PointLampModl.GetComponents<Renderer>();

        //PointLamp_Renderer[5].material.color = alphaColor[5];
        //alphaColor[5].a = 0.3f;
        //PointLamp_Renderer[5].material.color = alphaColor[5];
        //for (int count = 0; count < 9; count++)
        //{
        //    //8PointLamp_Material[count] = PointLampModl[count].GetComponent<Renderer>().material; //메탈릭

        //}

        //pointlamp_material[7].color = 0.6f;


        //for (int value = 0; value < 9; value++)
        //{
        //    PointLamp_Renderer[value].material.color = alphaColor[value];
        //}



        //   Animation Tinkerbell_ani = gameObject.GetComponent<Animation>();
        lamplight = FindObjectOfType<LampLight>();

        bell_Doll.SetActive(false);



        //foreach (Renderer rend in PointLamp_Renderer)
        //    alphaColor = rend.material.color;

        lamp[0].SetActive(true);
        lamp[7].SetActive(true);
        lamp[1].SetActive(true);

        for (int i = 0; i < 8; i++)
        {
            lamp[i].SetActive(false);
        }

        lamp[0].SetActive(true);
        lamp[7].SetActive(true);
        lamp[1].SetActive(true);

        CheckOnL = 3;

        //외곽선
        OutlineController = GameObject.FindObjectOfType<DrawOutline_HJ>();

        //장애물,벽
        obstacleReader_script = GameObject.FindObjectOfType<ObstacleReader>();
        obstacle_layer = (1 << LayerMask.NameToLayer("Light")) + (1 << LayerMask.NameToLayer("Obstacle"));
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckObstacle())
            return;





        CheckLamp();
        TryAction();
        End_LampP();

    }
    private void TryAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!outline_active)
                return;

            CanLocation();
        }
    }

    private void End_LampP()
    {
        if (CheckOnL == 8)
        {
            bell_Doll.SetActive(true);

            InfoDisappear();

            if (pre_ol_index != -1)
            {
                // - 외곽선 해제
                OutlineController.set_enabled(pre_ol_index, false);
                pre_ol_index = -1;
                OutlineController.set_check(false);
                outline_active = false;

                // - 클릭버튼 해제
                actionCaption.SetActive(false);
            }

            this.gameObject.GetComponent<ColliderMgr>().enabled = false; // this.enabled = false;
        }
    }

    private void CanLocation()
    {
        if (pickupActivated)
        {
            if (puzzleInfo.transform != null)
            {
                if (puzzleInfo.transform.CompareTag("Puzzle"))
                {
                    int Lampnumber = puzzleInfo.transform.GetComponent<LampLight>().LampNum;

                    puzzleInfo.transform.GetComponent<AudioSource>().Play();

                    if (Lampnumber != 9)
                    {
                        SoundManger.instance.PlaySound(Lamp_Sound);



                        if (Lampnumber - 1 < 0)
                        {
                            if (lamp[7].activeInHierarchy == true)
                            {
                                lamp[7].gameObject.SetActive(false);
                                CheckOnL--;


                                alphaColor.a = 0.2f;
                                PointLamp_Renderer[7].material.color = alphaColor;
                            }
                            else
                            {
                                lamp[7].gameObject.SetActive(true);
                                CheckOnL++;

                                alphaColor.a = 0.6f;
                                PointLamp_Renderer[7].material.color = alphaColor;
                            }
                        }
                        else
                        {
                            if (lamp[Lampnumber - 1].activeInHierarchy == true)
                            {
                                lamp[Lampnumber - 1].gameObject.SetActive(false);
                                CheckOnL--;

                                alphaColor.a = 0.6f;
                                PointLamp_Renderer[Lampnumber - 1].material.color = alphaColor;
                            }
                            else
                            {
                                lamp[Lampnumber - 1].gameObject.SetActive(true);
                                CheckOnL++;

                                alphaColor.a = 0.2f;
                                PointLamp_Renderer[Lampnumber - 1].material.color = alphaColor;
                            }
                        }

                        if (lamp[Lampnumber].activeInHierarchy == true)
                        {
                            lamp[Lampnumber].gameObject.SetActive(false);
                            CheckOnL--;

                            alphaColor.a = 0.2f;

                            //  foreach (Renderer rend in PointLamp_Renderer)
                            PointLamp_Renderer[Lampnumber].material.color = alphaColor;

                        }
                        else
                        {
                            lamp[Lampnumber].gameObject.SetActive(true);
                            CheckOnL++;

                            alphaColor.a = 0.6f;
                                PointLamp_Renderer[Lampnumber].material.color = alphaColor;
                        }

                        if (Lampnumber + 1 > 7)
                        {
                            if (lamp[0].activeInHierarchy == true)
                            {
                                lamp[0].gameObject.SetActive(false);
                                CheckOnL--;
                                
                                alphaColor.a = 0.2f;
                                PointLamp_Renderer[0].material.color = alphaColor;


                            }
                            else
                            {
                                lamp[0].gameObject.SetActive(true);
                                CheckOnL++;

                                alphaColor.a = 0.6f;
                                PointLamp_Renderer[0].material.color = alphaColor;
                            }
                        }
                        else
                        {
                            if (lamp[Lampnumber + 1].activeInHierarchy == true)
                            {
                                lamp[Lampnumber + 1].gameObject.SetActive(false);
                                CheckOnL--;

                                alphaColor.a = 0.2f;
                                PointLamp_Renderer[Lampnumber + 1].material.color = alphaColor;
                            }
                            else
                            {
                                lamp[Lampnumber + 1].gameObject.SetActive(true);
                                CheckOnL++;

                                alphaColor.a = 0.6f;
                                PointLamp_Renderer[Lampnumber + 1].material.color = alphaColor;
                            }
                        }
                    }
                    else // 두꺼비집
                    {
                        StartCoroutine(CheckAnimationState());

                        if (LaberOn == false)
                        {
                            SoundManger.instance.PlaySound(ResetLamp_Sound);
                            animator.SetTrigger("On");
                        }
                        else
                        {
                            //Debug.Log("현재 실행가능한 상태가 아닙니다.");
                        }

                        for (int i = 0; i < 8; i++)
                        {
                            lamp[i].SetActive(false);
                        }

                        lamp[0].SetActive(true);
                        lamp[7].SetActive(true);
                        lamp[1].SetActive(true);
                        CheckOnL = 3;
                    }
                }
            }

        }

    }

    IEnumerator CheckAnimationState()
    {
        while (!animator.GetCurrentAnimatorStateInfo(0)
        .IsName("New State"))
        {
            //전환 중일 때 실행되는 부분
            LaberOn = true;
            yield return null;
        }

        while (animator.GetCurrentAnimatorStateInfo(0)
                .IsName("New State"))
        {
            //전환 중일 때 실행되는 부분
            LaberOn = false;
            yield return null;
        }

        //애니메이션 완료 후 실행되는 부분

    }

    private void CheckLamp()
    {
        if (Physics.Raycast(MCamera.transform.position, MCamera.transform.TransformDirection(Vector3.forward), out puzzleInfo, range, layer))
        {
            if (OutlineController.get_outline_okay())
                return;

            if (puzzleInfo.transform.CompareTag("Puzzle")) //compare @
            {
                LampInfoAppear();

                // - 클릭버튼 활성화
                actionCaption.SetActive(true);

                // - 외곽선 그리기
                if (pre_ol_index == -1)
                {
                    SetOutline setoutlin_script = puzzleInfo.transform.GetComponent<SetOutline>();

                    if (setoutlin_script != null) // 두꺼비집이 아닐 경우
                    {
                        OutlineController.set_check(true);
                        OutlineController.set_enabled(setoutlin_script._index, true);
                        pre_ol_index = setoutlin_script._index;
                        outline_active = true;
                    }
                    else
                    {
                        pre_ol_index = -2;
                        OutlineController.set_check(true);
                        outline_active = true;
                    }
                }
            }
        }
        else
        {
            InfoDisappear();

            if (pre_ol_index != -1)
            {
                if (pre_ol_index != -2)
                {
                    // - 외곽선 해제
                    OutlineController.set_enabled(pre_ol_index, false);
                    pre_ol_index = -1;
                    OutlineController.set_check(false);
                    outline_active = false;
                }
                else
                {
                    pre_ol_index = -1;
                    OutlineController.set_check(false);
                    outline_active = false;
                }

                // - 클릭버튼 해제
                actionCaption.SetActive(false);
            }
        }
    }

    // Need to modify
    private void LampInfoAppear()
    {
        pickupActivated = true;

        //info
        //actionText.gameObject.SetActive(true);
        //actionText.text = "등불 끄기, 켜기 [Click]";
    }

    public void InfoDisappear()
    {
        pickupActivated = false;

        //info
        //actionText.gameObject.SetActive(false);
    }

    private bool CheckObstacle()
    {
        // - 장애물 검사하기
        coverCheck = obstacleReader_script.LookAtFrame(obstacle_layer);
        if (coverCheck)
        {
            pickupActivated = false;

            if (pre_ol_index != -1)
            {
                // - 외곽선 해제
                OutlineController.set_enabled(pre_ol_index, false);
                pre_ol_index = -1;
                OutlineController.set_check(false);
                outline_active = false;

                // - 클릭버튼 해제
                actionCaption.SetActive(false);
            }

            return true;
        }

        return false;
    }
}
