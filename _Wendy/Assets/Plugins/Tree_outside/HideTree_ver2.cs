using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideTree_ver2 : MonoBehaviour
{
    public GameObject _real_Tree = null;

    public GameObject _fake_Tree = null; //투명 나무 Parent
    public Transform[] _tree_element; //투명 나무 Parent
    public Material[] _tree_mats; //투명 나무 Parent

    private Coroutine _coroutine;
    private bool check_cr = false; //코루틴 한번

    private bool check_order = false;

    private bool check_ani = false; 

    //private Material[] _Marts;

    void Start()
    {
        //_Marts = _fake_Tree.GetComponentsInChildren<Material>();

        //GameObject[] temps = _fake_Tree.GetComponentsInChildren<GameObject>();
        //_tree_element = new GameObject[temps.Length - 1];
        //int j = 0;
        //for (int i = 0; i < temps.Length; i++)
        //{
        //    if (!GameObject.ReferenceEquals(temps[i], _fake_Tree))
        //    {
        //        _tree_element[j] = temps[i];
        //        j++;
        //    }
        //}
        _tree_element = _fake_Tree.GetComponentsInChildren<Transform>();
        _tree_mats = new Material[_tree_element.Length];
        for(int i = 0; i < _tree_mats.Length;i++)
        {
            _tree_mats[i] = _tree_element[1].gameObject.GetComponent<Renderer>().material;
        }

        _fake_Tree.SetActive(false);
    }

    void OnTriggerEnter(Collider collider) //콜라이더로바꾸기
    {
        if (IsCharacter(collider))//if(currentCharacter == character)
        {
            if (check_ani)
                return;

            //- 투명해짐(렌더링모드바꿈)
            _real_Tree.SetActive(false);
            _fake_Tree.SetActive(true);
            //SetMaterialTransparent();

            //- 페이드아웃
            //foreach (GameObject go in _tree_element)
            //{
            //    if (GameObject.ReferenceEquals(go, _fake_Tree)) //(go != _fake_Tree)
            //    {
            //        iTween.FadeTo(go, 0.2f, 1);
            //    }
            //}
            for (int i = 1; i < _tree_element.Length; i++) //1부터
            {
                iTween.FadeTo(_tree_element[i].gameObject, 0.15f, 1f);
            }

            //- 코루틴 확인용
            if (!check_order) // true 일때, false 로 바꾼다
            {
                if (check_cr) // 코루틴이 이미 돌아가고 있으면 멈춘다
                    StopCoroutine(_coroutine);

                _coroutine = StartCoroutine(TreeCoroutine());
                check_order = true;
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (IsCharacter(collider))
        {
            if (check_ani)
                return;

            // - 페이드인
            //iTween.FadeTo(_fake_Tree, 1f, 0.2f);
            for (int i = 1; i < _tree_element.Length; i++) //1부터
            {
                iTween.FadeTo(_tree_element[i].gameObject, 1f, 0.15f);
            }

            if (check_order) // false 일때, true 로 바꾼다
            {
                if (check_cr) // 코루틴이 이미 돌아가고 있으면 멈춘다
                    StopCoroutine(_coroutine);

                Debug.Log("dd");

                _coroutine = StartCoroutine(TreeCoroutine());
                check_order = false;
            }
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
    //    foreach (Material m in _Marts)
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
    //    foreach (Material m in _Marts)
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
        _real_Tree.SetActive(true);
    }

    IEnumerator TreeCoroutine()
    {
        check_cr = true;

        //Material temp = _tree_element[1].gameObject.GetComponent<Renderer>().material;
        Material temp = _tree_mats[1];

        if (check_order) // - 알파값 증가 = 불투명해지기
        {
            while (true)
            {
                float alpha = temp.color.a;

                if (alpha <= 0.99f)
                    yield return new WaitForSeconds(0.1f);
                else
                    break;
            }

            _real_Tree.SetActive(true);
            _fake_Tree.SetActive(false);
        }
        else // - 감소 = 투명해지기
        {
            while (true)
            {
                float alpha = temp.color.a;

                if (alpha >= 0.16f)
                    yield return new WaitForSeconds(0.1f);
                else
                    break;
            }
        }

        // - 코루틴 끝
        check_cr = false;
    }

    public void set_AniState(bool ani) //애니메이션 실행될때는 알파값이
    {
        check_ani = ani;

        if (ani) // - 애니메이션 시작
        {
            if (check_cr) // 코루틴 멈춘다
                StopCoroutine(_coroutine);

            // 알파값 기본으로 : itween 으로 시작지점 지정해줘서 없어도댐
            //for (int i = 0; i < _tree_mats.Length; i++)
            //{
            //    Color fadecolor = _tree_mats[i].color;
            //    fadecolor.a = 1f;
            //    _tree_mats[i].color = fadecolor;
            //}

            _real_Tree.SetActive(false);
            _fake_Tree.SetActive(false);

            check_cr = false;
            check_order = false;
            check_ani = false;
        }
        else // - 애니메이션 끝났을때
        {
            _real_Tree.SetActive(true);
            _fake_Tree.SetActive(false);
        }
    }


}
