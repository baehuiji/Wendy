using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubClockInfo : MonoBehaviour
{
    public int _index;
    //public SeeingSubClock seeingSubClock_script;
    public Transform _camPos;

    void Start()
    {
        //seeingSubClock_script = GetComponent<SeeingSubClock>();
    }

    public int get_Index()
    {
        return _index;
    }
}
