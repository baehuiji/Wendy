using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wendy_PlayState : IState
{
    private WendyAI _wendy;
    private int _state_num;

    void IState.OnEnter(WendyAI wendy)
    {
        // - wendy 프로퍼티 초기화
        this._wendy = wendy;
        _state_num = 1;

        // - 초기화
        //X (애니메이션은 안해도됨)     
    }

    void IState.Update()
    {
        //if (_wendy.GetClear2Stage())
        //{
        //    Debug.Log("~play");
        //    // - 상태 전이
        //    _wendy.SetState(new Wendy_IdleState());
        //}
    }

    void IState.OnExit()
    {
        // - 종료되면서 해야할것 
        //_wendy.SetClearAni();
        //_wendy.SetContactWithPlayer();
    }

     int IState.GetStateNum()
    {
        return _state_num;
    }

    //// - 지하실노는 상태로부터 상태가 바뀔때 = 2스테이지클리어후, 플레이어가 웬디범위에 들어왔을때
    //public void SetStateFromPlay()
    //{
    //    _wendy.SetState(new Wendy_IdleState());
    //}

    //// - 2스테이지 배치퍼즐 클리어한후, 웬디 애니메이션이 바뀜
    //public void ChangeAniFromPlay()
    //{
    //    _wendy.SetClearAni();
    //}

}
