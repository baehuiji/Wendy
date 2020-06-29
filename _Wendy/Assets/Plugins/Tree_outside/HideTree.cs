using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideTree : MonoBehaviour
{
    public GameObject Tree = null; //fake
    public GameObject real_Tree = null;

    private bool check = false;
    private bool check_cr = false;

    void OnTriggerEnter(Collider collider) //콜라이더로바꾸기
    {
        if (IsCharacter(collider))//if(currentCharacter == character)
        {
            Debug.Log("asf");

            //- 투명해짐(렌더링모드바꿈)
            real_Tree.SetActive(false);
            //SetMaterialTransparent();

            //- 페이드아웃
            iTween.FadeTo(Tree, 0.2f, 1);

            //- 코루틴 확인용
            check = false;
        }
    }

    private bool IsCharacter(Collider collider)
    {
        if (collider.transform.CompareTag("Player"))
            return true;
        else
            return false;
    }

    //private void SetMaterialTransparent()
    //{
    //    foreach (Material m in Tree.GetComponent<Renderer>().materials)
    //    {
    //        m.SetFloat("_Mode", 2);
    //        m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
    //        m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
    //        m.SetInt("_ZWrite", 0);
    //        m.DisableKeyword("_ALPHATEST_ON");
    //        m.EnableKeyword("_ALPHABLEND_ON");
    //        m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
    //        m.renderQueue = 3000;
    //    }
    //}

    //private void SetMaterialOpaque()
    //{
    //    foreach (Material m in Tree.GetComponent<Renderer>().materials)
    //    {
    //        m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
    //        m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
    //        m.SetInt("_ZWrite", 1);
    //        m.DisableKeyword("_ALPHATEST_ON");
    //        m.DisableKeyword("_ALPHABLEND_ON");
    //        m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
    //        m.renderQueue = -1;
    //    }
    //}
    private void Set_unActive()
    {
        real_Tree.SetActive(true);
    }

    void OnTriggerExit(Collider collider)
    {
        if (IsCharacter(collider))
        {
            //페이드인
            iTween.FadeTo(Tree, 1f, 0.2f);

            //- 선명해짐(렌더링모드바꿈), X
            //바로 됨.
            //SetMaterialOpaque();
            //시간을 준 후 바뀌게 설정, 근데 바로됨
            //Invoke("SetMaterialOpaque", 1.0f);
            //Invoke("Set_unActive", 1.0f);

            if (!check_cr)
                StartCoroutine(MyCoroutine());
        }
    }
    //void OnCollisionEnter(Collision coll) //콜라이더로바꾸기
    //{

    //}


    IEnumerator MyCoroutine()
    {
        //ver 1
        //while (!check)
        //{
        //    Material temp = Tree.GetComponent<Renderer>().material;
        //    float alpha = temp.color.a;

        //    if (alpha <= 0.99f)
        //        yield return null;
        //    else
        //        check = true;
        //}
        //real_Tree.SetActive(true);
        //check_cr = false;


        //ver 2
        //if (!check_cr) //트리거쪽에서 검사
        {
            check_cr = true;
            Material temp = Tree.GetComponent<Renderer>().material;

            while (!check)
            {
                float alpha = temp.color.a;

                if (alpha <= 0.99f)
                    yield return null;
                else
                    check = true;
            }

            //코루틴 끝
            real_Tree.SetActive(true);
            check_cr = false;
        }
    }
}
