using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wendy_FootSound : MonoBehaviour
{

    //웬디 발자국 사운드 
    [SerializeField]
    private string[] floorfootSound;

    [SerializeField]
    private string[] carpetfootSound;

    bool woodfloorState = true;
    bool carpetfloorState = false;


    void WendyFootSound()
    {
        if (woodfloorState == true)
        {
            int i = Random.Range(0, 4);
            SoundManger.instance.PlaySound(floorfootSound[i]);

        }
        //사운드 출력.
        else if (carpetfloorState == true)
        {
            int i = Random.Range(0, 3);
            SoundManger.instance.PlaySound(carpetfootSound[i]);
            //Debug.Log("carpetSound");

        }
        else
        {
            return;
        }

    }


    void OnTriggerEnter(Collider _col)
    {

        if (_col.CompareTag("Carpet"))
        {
            carpetfloorState = true;
            woodfloorState = false;
        }


    }

    void OnTriggerExit(Collider _col)
    {

        if (_col.CompareTag("Carpet"))
        {
            carpetfloorState = false;
            woodfloorState = true;

        }

    }
}
