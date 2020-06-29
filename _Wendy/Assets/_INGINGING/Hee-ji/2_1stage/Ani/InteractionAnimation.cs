using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionAnimation : MonoBehaviour
{
    [SerializeField]
    private string OpenBoxSound = "openBox";

    [SerializeField]
    private string ShakeBoxSound = "ShakeBox";

    [SerializeField]
    private string ShackTreeSound = "ShakeTree";

    public int _type = 0; //1: 박스, 2: 인삼, 3: 일반나무

    public Animator _player_animator;
    public Animator _obj_animator;
    private string EventEndAnimationName = ""; //null
    private static string EventEndAnimationName1 = "Base Layer.box(2)";
    private static string EventEndAnimationName2 = "Base Layer.insami";
    private static string EventEndAnimationName3 = "Base Layer.tree";
    private string EventTriggerName = "";
    private static string EventTriggerName1 = "SeeingBox";
    private static string EventTriggerName2 = "ShakingInsam";
    private static string EventTriggerName3 = "ShakingTree";

    private Player_1stage palyer_script;
    ActionController_01 actionCtrler;

    private Coroutine coroutine;
    private bool isPlaying = false;

    private float transAngle = 0f;
    public bool _isBox = false;
    InteracttAniManager interAniManager_script;
    private int _answer_index = -1;

    public HideTree_ver2 hideTree_script;
    public Collider hideColl;

    void Start()
    {
        palyer_script = GameObject.FindObjectOfType<Player_1stage>();
        actionCtrler = GameObject.FindObjectOfType<ActionController_01>();

        switch (_type)
        {
            case 1:
                EventEndAnimationName = EventEndAnimationName1;
                EventTriggerName = EventTriggerName1;
                //SoundManger.instance.PlaySound(OpenBoxSound);

                break;
            case 2:
                EventEndAnimationName = EventEndAnimationName2;
                EventTriggerName = EventTriggerName2;
               // SoundManger.instance.PlaySound(ShackTreeSound);

                break;
            case 3:
                EventEndAnimationName = EventEndAnimationName3;
                EventTriggerName = EventTriggerName3;
              //  SoundManger.instance.PlaySound(ShackTreeSound);
                break;
            default:
                break;
        }

        interAniManager_script = GameObject.FindObjectOfType<InteracttAniManager>();
    }

    void Update()
    {

    }

    public void PlayAni()
    {
        if (isPlaying == true) //중복재생방지
        {
            return;
        }

        coroutine = StartCoroutine(check_AniState());
    }

    IEnumerator BoxsoundDelay()
    {
        yield return new WaitForSeconds(1.5f);
        SoundManger.instance.PlaySound(OpenBoxSound);
        yield return new WaitForSeconds(4f);
        SoundManger.instance.PlaySound(ShakeBoxSound);

    }
    IEnumerator check_AniState()
    {
        isPlaying = true;

        _player_animator.SetTrigger(EventTriggerName);
        _obj_animator.SetBool("IsPlaying", true);

        switch (_type)
        {
            case 1:
                StartCoroutine(BoxsoundDelay());

                break;
            case 2:
                 SoundManger.instance.PlaySound(ShackTreeSound);

                break;
            case 3:
                  SoundManger.instance.PlaySound(ShackTreeSound);
                break;
            default:
                break;
        }
      //  SoundManger.instance.PlaySound(ShackTreeSound);
        while (!_player_animator.GetCurrentAnimatorStateInfo(0)
                .IsName(EventEndAnimationName))
        {
            // - 전환 중
            yield return null;
        }

        while (_player_animator.GetCurrentAnimatorStateInfo(0)
        .normalizedTime < 0.7f)
        {
            // - 애니메이션 재생 중, 퍼즐조각 애니메이션 중간에 떨어짐
            yield return null;
        }

        // - 퍼즐조각 애니메이션
        if (!_isBox && _answer_index != -1) //정답인 나무일때
        {
            interAniManager_script.Active_Piece(_answer_index);
        }

        while (_player_animator.GetCurrentAnimatorStateInfo(0)
                .normalizedTime < 0.99f)
        {
            // - 애니메이션 재생 중
            yield return null;
        }


        // - 재생 완료, 스크립트 삭제
        _player_animator.SetBool("IsPlaying", false);
        this.enabled = false;

        palyer_script.enabled = true;
        palyer_script.set_Angle(transAngle);
        actionCtrler.enabled = true;

        if (hideTree_script != null)
        {
            hideTree_script.enabled = true; //나무투명화 스크립트
            hideColl.enabled = true;
        }
    }

    public void set_angle(float a)
    {
        transAngle = a;
    }

    public void set_aIndex(int index)
    {
        _answer_index = index;
    }
}
