using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOn_3stage : MonoBehaviour
{
    public GameObject[] _lights3stage;
    public GameObject[] _lights2stage;
    public GameObject[] _lightsfront;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void LightOn()
    {
        for(int i = 0; i < _lights3stage.Length; i++)
        {
            _lights3stage[i].SetActive(true);
        }

        for (int i = 0; i < _lights2stage.Length; i++)
        {
            _lights2stage[i].SetActive(false);
        }
    }

    public void LightOnFront()
    {
        for (int i = 0; i < _lightsfront.Length; i++)
        {
            _lightsfront[i].SetActive(true);
        }
    }
}
