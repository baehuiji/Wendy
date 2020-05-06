using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowController : MonoBehaviour
{
    public GlobalSnowEffect.GlobalSnow globalSnow_script;

    private bool outset = false;
    void Start()
    {
        globalSnow_script.Coverage_On();
    }

    void Update()
    {
        if (!outset)
        {
            outset = true;
            StartCoroutine("Play_once");
            //globalSnow_script.Coverage_Off();
        }
    }

    public void Set_Off()
    {
        globalSnow_script.Coverage_Off();
    }

    IEnumerator Play_once()
    {
        yield return new WaitForSeconds(2f);
        globalSnow_script.Coverage_Off();
    }
}
