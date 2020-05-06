using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    float MoveSpeed = 10f;

    void Start()
    {
        
    }

    void Update()
    {
        float V = Input.GetAxis("Vertical");
        float H = Input.GetAxis("Horizontal");

        if (V != 0f)
            transform.Translate(new Vector3(0f, 0f, V * MoveSpeed * Time.deltaTime));
        if (H != 0f)
            transform.Translate(new Vector3(H * MoveSpeed * Time.deltaTime, 0f, 0f));
    }
}
