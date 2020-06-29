//3스테이지 배치퍼즐 enter 버튼

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollAniManager : MonoBehaviour
{
    DisplayManager_3stage displayManager_script;

    public MockingDollAni[] doll_anims = new MockingDollAni[8];
    public bool[] doll_states = new bool[8]; //애니메이션 상태, 인덱스는 doll_anims와 같고 아이템코드 순서순
    public bool[] doll_acitve_state = new bool[8]; //오브젝트의 활성화 상태, 인덱스는 장식장배치

    private bool clickable = true; //종 클릭이 가능한지 (애니메이션이 다 끝난상태면 가능함)


    void Start()
    {
        displayManager_script = GameObject.FindObjectOfType<DisplayManager_3stage>();

        for (int i = 0; i < doll_anims.Length; i++)
        {
            doll_anims[i] = displayManager_script.doll_obj[i].GetComponent<MockingDollAni>();
            doll_anims[i].set_dollNum(i);
        }

        for (int i = 0; i < doll_states.Length; i++)
        {
            doll_states[i] = false; // true;
        }

        for (int i = 0; i < doll_acitve_state.Length; i++)
        {
            doll_acitve_state[i] = false;
        }
    }

    void Update()
    {

    }

    public void Active_AnimScript() //이거 각자의 MockingDollAni에서 활성화관련 코드에 잇는걸로 바꿈
    {
        for (int i = 0; i < doll_anims.Length; i++)
        {
            doll_anims[i].gameObject.SetActive(true);
        }
    }

    public bool ClickButton()
    {
        if (clickable == false) //애니메이션이 실행할 수 있는 상태인지 검사
            return false;

        //clickable = false;

        return true;
    }
    public void set_clickable(bool b)
    {
        clickable = b;
    }
        
    public void MisplaceDolls()
    {
        //이함수에 반환형을 bool 로 해준다면 함수의 실행성공실패를 알수있다

        //인형 애니메이션 실행
        for (int i = 0; i < doll_anims.Length; i++)
        {
            doll_acitve_state[i] = doll_anims[i].get_activeState();
            if (doll_acitve_state[i] == true) 
                doll_anims[i].play_Animation();
        }
    }

    public void CheckDollAnim(int index) //각각 인형의 애니메이션 끝날떄마다 호출, 인형들 애니메이션이 끝이 났는지 검사함, 
    {
        doll_states[index] = doll_anims[index].get_playState();
        if (doll_states[index] == true)
        {
            return;
        }

        for (int i = 0; i < doll_states.Length; i++)
        {
            if (doll_states[i] == true) // || !doll_anims[i].gameObject.activeInHierarchy)
            {
                return;
            }
        }

        // 모두 애니메이션이 완료된 상태여야 클릭할수있다
        clickable = true; //클릭할수있는 상태로 전환
    }

    // - 활성화 상태
    public void set_dollAcitveState(int index, bool state)
    {
        doll_acitve_state[index] = state;
    }

    // - 애니메이션 상태
    public void set_dollAniState(int index, bool state)
    {
        doll_states[index] = state;
    }
}
