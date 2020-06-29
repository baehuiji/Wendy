using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wendy_IdleState : IState
{
    private WendyAI _wendy;
    private int _state_num;

    private bool _contact;

    //private bool _start_coroutine;

    void IState.OnEnter(WendyAI wendy)
    {
        // - wendy 프로퍼티 초기화
        this._wendy = wendy;
        _state_num = 2;
        //_start_coroutine = false;
        //_contact = _wendy.GetContact();

        // - 초기화
        _wendy.SetIdleAni();

        if (!_contact)
            _wendy.ChangeState();
    }

    void IState.Update()
    {
        //if (_start_coroutine)
        //{
        //    _start_coroutine = _wendy.GetCorB222();
        //    return;
        //}

        ////StartCoroutine(ChangeState());
        _contact = _wendy.GetContact();
        if (!_contact)
            _wendy.ChangeState();

        //if (_contact) // 지하실에서 플레이어와 접촉하였을때
        //{
        //    //    // - 상태 전이
        //    //    //if (플레이어가 멀어졌을때)
        //    //    {
        //    //        _wendy.SetState(new Wendy_MoveState());
        //    //    }
        //}
        //else // 플레이어와 접촉하지 않았을때 (처음 Play애니에서 넘어왔을때)
        //{
        //    //    // - 상태 전이
        //    //    //if (플레이어가 범위 안에 들어왔을때)
        //    //    {
        //    //        _wendy.SetState(new Wendy_MoveState());
        //    //    }
        //    _wendy.ChangeState();
        //    _start_coroutine = true;
        //}
    }

    void IState.OnExit()
    {
        // - 종료되면서 해야할것 
        //_wendy.SetContactWithPlayer(false);

        _wendy.StopCWPCoroutine();

    }

    //IEnumerator ChangeState()
    //{
    //    _start_coroutine = true;

    //    yield return new WaitForSeconds(2f);

    //    Debug.Log("~idle");
    //    _wendy.SetState(new Wendy_MoveState());
    //}

     int IState.GetStateNum()
    {
        return _state_num;
    }

    //public void SetContact()
    //{
    //    _contact = _wendy.GetContact();
    //}
}