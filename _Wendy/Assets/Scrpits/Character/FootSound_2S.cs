using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSound_2S : MonoBehaviour
{

    [SerializeField]
    private string[] floorfootSound;


    void Start()
    {

    }

    void OnTriggerEnter(Collider _col)
    {
        if (_col.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {

                int i = Random.Range(0, 5);
                SoundManger.instance.PlaySound(floorfootSound[i]);
            
        }


        //여기서 바닥이 흠.................
        // (고민중)


    }
}
