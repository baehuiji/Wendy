using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBlock : MonoBehaviour
{
    public NextStage_01 nextStage;

    void start()
    {
        //nextStage = GameObject.FindObjectOfType<NextStage_01>();
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("Destination"))
        {
            nextStage.InStartFadeAnim();
        }
    }
}
