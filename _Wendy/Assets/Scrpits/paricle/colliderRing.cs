using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colliderRing : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject Ring_Particle;




    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            //정지

            Ring_Particle.gameObject.SetActive(false);

            // this.gameObject(false);
            this.enabled = false;


        }


    }
}

