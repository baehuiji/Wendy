using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockingDollAni : MonoBehaviour
{
    private Coroutine coroutine;
    private bool isPlaying = false;

    public GameObject aniModeling;
    public GameObject realModeling;
    private Animator animator;
    private static string EventAnimationName = "Base Layer.Mock";

    private DollAniManager dollAniManger_script;

    private int dollNum = -1;

    void Start()
    {
        animator = aniModeling.GetComponent<Animator>();

        dollAniManger_script = GameObject.FindObjectOfType<DollAniManager>();

        initialize();
    }

    void Update()
    {
    }

    public void set_dollNum(int number)
    {
        dollNum = number;
    }

    public void initialize()
    {
        aniModeling.SetActive(false);
    }

    public void play_Animation()
    {
        if (isPlaying || !gameObject.activeInHierarchy)
            return;// false;  //StopCoroutine(coroutine);


        coroutine = StartCoroutine(PlayDollAnim());


        return; // true;
    }

    IEnumerator PlayDollAnim()
    {
        aniModeling.SetActive(true);
        realModeling.SetActive(false);
        isPlaying = true;
        dollAniManger_script.set_dollAniState(dollNum, true);

        animator.SetBool("IsMocking", true);

        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(EventAnimationName))
        {
            yield return null;
        }
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.99f)
        {
            yield return null;
        }

        animator.SetBool("IsMocking", false);

        aniModeling.SetActive(false);
        realModeling.SetActive(true);
        isPlaying = false;
        dollAniManger_script.set_dollAniState(dollNum, false);

        dollAniManger_script.CheckDollAnim(dollNum);

        //Debug.Log("end check");
    }

    public bool get_playState()
    {
        return isPlaying;
    }

    public bool get_activeState()
    {
        return gameObject.activeSelf; //내자신의 활성화상태 넘겨주기(습득한상태인지아닌지)
    }
}
