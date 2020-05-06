using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunToTarget : MonoBehaviour
{
    public Transform Target;

    float MoveSpeed;
    float TurnSpeed = 10f;

    void Start()
    {
        MoveSpeed = Random.Range(0f, 5f);
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, Target.position);

        if(distance > 0.6f)
        {
            Vector3 DirectionVector = (Target.position - transform.position).normalized;
            Vector3 fixedEuler = Quaternion.LookRotation(DirectionVector).eulerAngles;

            transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(fixedEuler), TurnSpeed * Time.deltaTime);
        }
    }
}
