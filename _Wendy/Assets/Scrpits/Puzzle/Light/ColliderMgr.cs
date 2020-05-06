using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ColliderMgr : MonoBehaviour
{
    //  public Animation Tinkerbell_ani;
    // Start is called before the first frame update

    public GameObject[] lamp;

    public GameObject bell_Doll;

    public GameObject MCamera;

    [SerializeField]
    private float range;

    [SerializeField]
    private LayerMask layer;

    public Text actionText;

    private bool pickupActivated = false;//false;
    private RaycastHit puzzleInfo;


    private int CheckOnL = 0;
    LampLight lamplight;

    //public GameObject[] _Pointstate;


    void Start()
    {

        bell_Doll.SetActive(false);

        //   Animation Tinkerbell_ani = gameObject.GetComponent<Animation>();
        lamplight = FindObjectOfType<LampLight>();
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
    }

    // Update is called once per frame
    void Update()
    {
        CheckLamp();
        TryAction();
        End_LampP();

    }
    private void TryAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CanLocation();
        }
    }

    private void End_LampP()
    {
        if (CheckOnL == 8)
        {
            //Debug.Log("램프가 끝났지롱");
            bell_Doll.SetActive(true);
            InfoDisappear();
            this.gameObject.GetComponent<ColliderMgr>().enabled = false;

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

                    if (Lampnumber != 9)
                    {
                        if (Lampnumber - 1 < 0)
                        {
                            if (lamp[7].activeInHierarchy == true)
                            {

                                lamp[7].gameObject.SetActive(false);
                                CheckOnL--;

                            }
                            else
                            {
                                lamp[7].gameObject.SetActive(true);
                                CheckOnL++;
                            }

                        }
                        else
                        {

                            if (lamp[Lampnumber - 1].activeInHierarchy == true)
                            {

                                lamp[Lampnumber - 1].gameObject.SetActive(false);
                                CheckOnL--;

                            }
                            else
                            {
                                lamp[Lampnumber - 1].gameObject.SetActive(true);
                                CheckOnL++;

                            }
                        }



                        if (lamp[Lampnumber].activeInHierarchy == true)
                        {
                            lamp[Lampnumber].gameObject.SetActive(false);
                            CheckOnL--;

                        }
                        else
                        {
                            lamp[Lampnumber].gameObject.SetActive(true);
                            CheckOnL++;

                        }


                        if (Lampnumber + 1 > 7)
                        {
                            if (lamp[0].activeInHierarchy == true)
                            {
                                lamp[0].gameObject.SetActive(false);
                                CheckOnL--;

                            }
                            else
                            {
                                lamp[0].gameObject.SetActive(true);
                                CheckOnL++;

                            }

                        }
                        else
                        {
                            if (lamp[Lampnumber + 1].activeInHierarchy == true)
                            {
                                lamp[Lampnumber + 1].gameObject.SetActive(false);
                                CheckOnL--;

                            }
                            else
                            {
                                lamp[Lampnumber + 1].gameObject.SetActive(true);
                                CheckOnL++;

                            }


                        }
                    }

                    else
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            lamp[i].SetActive(false);
                        }

                        lamp[0].SetActive(true);
                        lamp[7].SetActive(true);
                        lamp[1].SetActive(true);
                        CheckOnL = 3;

                    }




                    /*
                    if (puzzleInfo.transform.name == "LampColider0")
                    {
                        Debug.Log("램프가 충돌되었습니다");

                        lamplight.SetLight(1);
                    }

                    if (puzzleInfo.transform.name == "LampColider1")
                    {
                        // lamplight.SetLight(1);
                        lamplight.SetLight(1);

                    }

                    if (puzzleInfo.transform.name == "LampColider2")
                    {
                        lamplight.SetLight(1);

                    }

                    if (puzzleInfo.transform.name == "LampColider3")
                    {
                        lamplight.SetLight(1);

                    }

                    if (puzzleInfo.transform.name == "LampColider4")
                    {
                        lamplight.SetLight(1);

                    }

                    if (puzzleInfo.transform.name == "LampColider5")
                    {
                        lamplight.SetLight(1);

                    }

                    if (puzzleInfo.transform.name == "LampColider6")
                    {
                        lamplight.SetLight(1);

                    }

                    if (puzzleInfo.transform.name == "LampColider7")
                    {
                        lamplight.SetLight(1);

                    }

    */

                    // 흠.... 어느 특정 콜라이더가 


                }
                //if(Light1.activeSelf == true)
                //{
                //    Light1.SetActive(false);
                //    Light2.SetActive(true);
                //    Light3.SetActive(true);
                //}
                //else
                //{
                //    Light1.SetActive(true);
                //    Light2.SetActive(false);
                //    Light3.SetActive(false);
                //}

            }

        }

    }









    private void CheckLamp()
    {
        if (Physics.Raycast(MCamera.transform.position, MCamera.transform.TransformDirection(Vector3.forward), out puzzleInfo, range, layer))
        {
            if (puzzleInfo.transform.CompareTag("Puzzle")) //compare @
            {
                LampInfoAppear();
            }
        }
        else
        {
            InfoDisappear();
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
}
