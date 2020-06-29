using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRange_1stage : MonoBehaviour
{
    Sight_1stage _sight_script;

    void Start()
    {
        _sight_script = GetComponent<Sight_1stage>();
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        //Destroy(other.gameObject);

        if(!(_sight_script.getCheck()))
        {
            _sight_script.setCheck(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        //Destroy(other.gameObject);

    }


}
