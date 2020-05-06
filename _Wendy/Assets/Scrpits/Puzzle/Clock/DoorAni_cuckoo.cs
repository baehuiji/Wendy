using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAni_cuckoo : MonoBehaviour
{
    private Animator aniController;
    private static string EventEndAnimationName = "Base Layer.cuckoo_door_close";
    Move_Cuckoo moveCuckoo_script;

    void Start()
    {
        aniController = GetComponent<Animator>();
        moveCuckoo_script = GameObject.FindObjectOfType<Move_Cuckoo>();
    }

    void Update()
    {

    }

    public void set_Ani_param(bool param)
    {
        aniController.SetBool("IsWrongAnswer", param);
    }

    public void check_state()
    {
        StartCoroutine(check_doorAniState());
    }

    IEnumerator check_doorAniState()
    {
        while (!aniController.GetCurrentAnimatorStateInfo(0)
                .IsName(EventEndAnimationName))
        {
            // - 전환 중
            yield return null;
        }

        while (aniController.GetCurrentAnimatorStateInfo(0)
                .normalizedTime < 0.99f)
        {
            // - 애니메이션 재생 중
            yield return null;
        }

        moveCuckoo_script.set_active();
    }
}
