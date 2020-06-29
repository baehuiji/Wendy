using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reFootSound_2S : MonoBehaviour
{

    [SerializeField]
    private string[] floorfootSound;

    [SerializeField]
    private string[] carpetfootSound;

    bool woodfloorState = true;
    bool carpetfloorState = false;


    void FootSound()
    {
        if (woodfloorState == true)
        {
            int i = Random.Range(0, 5);
            SoundManger.instance.PlaySound(floorfootSound[i]);

        }
        //사운드 출력.
        else if (carpetfloorState == true)
        {
            int i = Random.Range(0, 4);
            SoundManger.instance.PlaySound(carpetfootSound[i]);
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
