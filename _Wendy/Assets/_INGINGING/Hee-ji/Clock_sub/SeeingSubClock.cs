using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeingSubClock : MonoBehaviour
{
    ChangeCam_SubClock ChangeCam_script;

    void Start()
    {
        ChangeCam_script = GameObject.FindObjectOfType<ChangeCam_SubClock>();
    }

        void Update()
    {
        TryAction();
    }

    void TryAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ChangeCam_script.change_Camera(false);
        }
    }
}
