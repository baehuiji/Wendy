using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionCam : MonoBehaviour
{
    public GameObject Criteria; //Criteria 오브젝트 연결

    void Start()
    {
        
    }

    void Update()
    {
        transform.LookAt(Criteria.transform);
    }
}
