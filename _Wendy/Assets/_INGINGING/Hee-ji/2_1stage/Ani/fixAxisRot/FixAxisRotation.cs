using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixAxisRotation : MonoBehaviour
{
    public float _angle = 0.0f;

    void Start()
    {
        //var rot = transform.rotation;
        //transform.rotation = rot * Quaternion.Euler(0, 90, 0);

        //transform.rotation = Quaternion.Euler(0, 90, 0);
        transform.Rotate(0f, _angle, 0f);
    }

    void Update()
    {
        
    }
}
