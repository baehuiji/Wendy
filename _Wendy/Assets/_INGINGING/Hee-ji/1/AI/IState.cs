using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void OnEnter(WendyAI wendy);

    void Update();

    void OnExit();

    int GetStateNum();
}
