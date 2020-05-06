using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;

public class OntriggerLamp : MonoBehaviour
{
    public GameObject GameMgr;

    public GameObject PathObject;

    public GameObject Pathmodel;


    public GameObject PlayerMove;

    Renderer[] renderer;

    Color alphaColor;


    //한번만 실행될 코루틴... 어.. 걍 끄면 될듯?
    private bool ColliderStay;

    private bool State = true;

    public float Set = 6f; // 등불이 켜지는 시간
    public float FadeSet = 4f; //팅커벨이 사라지기 전까지 걸리는 딜레이 시간 

    float time = 0f;


    void Start()
    {

        renderer = Pathmodel.GetComponentsInChildren<Renderer>();

        // renderer.material.color = new Color(0, 0, 0, 0);
    }


    public void OnTriggerEnter(Collider other)
    {
        if(State == true)
        {
            if (other.transform.CompareTag("Player"))
            {
                //Debug.Log("충돌! 액자 퍼즐 깻는가 안 깻는가에 대해서!");

                ColliderStay = true;
                PathObject.GetComponent<PathFollower>().enabled = true;

                SartDelay();
                Fadein();
                // 등불 퍼즐 애니메이션 실행. 램프 키고 끄기 -- 추가하기 
                //  Tinkerbell_ani.Play();

                State = false;
            }

            else
                ColliderStay = false;
        }
    }

    void SartDelay()
    {
        StartCoroutine(LampLightDelay(Set));

    }

    IEnumerator LampLightDelay(float Set)
    {
        PlayerMove.GetComponent<Player_HJ>().enabled = false;
        PlayerMove.GetComponent<Collider>().enabled = false;

        // StartCoroutine(FadeInTinkerBell());


        yield return new WaitForSeconds(Set);
      //  PathObject.SetActive(false);
        GameMgr.GetComponent<ColliderMgr>().enabled = true;

        yield return new WaitForSeconds(1f);

        PlayerMove.GetComponent<Player_HJ>().enabled = true;
        PlayerMove.GetComponent<Collider>().enabled = true;
    }

    void Fadein()
    {
        StartCoroutine(FadeInTinkerBell(FadeSet));
    }



    IEnumerator FadeInTinkerBell(float FadeSet)
    {

        foreach (Renderer rend in renderer)
            alphaColor = rend.material.color ;
        // yield return new WaitForSeconds(Set/2);

        // Color alpha = renderer.material.color;

        time = 0f;

        yield return new WaitForSeconds(FadeSet);


        while (alphaColor.a > 0f)
        {
            time += Time.deltaTime / 1f;
            alphaColor.a = Mathf.Lerp(1, 0, time);
            //Debug.Log("줄어드는중");

            foreach (Renderer rend in renderer)
               rend.material.color = alphaColor ;
            //alpha = Fadetime;
            // renderer.material.color = alpha;
            yield return new WaitForSeconds(0.01f);

        }
        Pathmodel.gameObject.SetActive(false);
    }


}
