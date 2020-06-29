using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wendy_MoveState : IState
{
    private WendyAI _wendy;
    private int _state_num;

    private bool _contact;
    private bool _start_coroutine;

    void IState.OnEnter(WendyAI wendy)
    {
        // - wendy 프로퍼티 초기화
        this._wendy = wendy;
        _state_num = 3;
        _contact = _wendy.GetContact();
        _start_coroutine = false;

        if (_contact)
        {
            _wendy.SetState(new Wendy_IdleState());
            return;
        }

        // - 초기화
        _wendy.SetWalkAni();

        //이동할 위치
        //_wendy.SetDestination();
        _wendy.StartMovemntCoroutine();
    }

    void IState.Update()
    {
        //if (_start_coroutine)
        //{
        //    _start_coroutine = _wendy.GetCorB();
        //    return;
        //}

        //// - 실행할것 구현
        ////웬디의 이동

        //// - 상태 전이
        //_contact = _wendy.GetContact();
        //if (_contact)
        //{
        //    _wendy.SetState(new Wendy_IdleState());
        //}
        //else
        //{
        //    _wendy.ChangeDestination();
        //    _start_coroutine = true;
        //}
    }

    void IState.OnExit()
    {
        // - 종료되면서 해야할것 

        _wendy.StopMovemntCoroutine(); //StopCDCoroutine();
        _wendy.SetIdleAni();
    }

    int IState.GetStateNum()
    {
        return _state_num;
    }
}
