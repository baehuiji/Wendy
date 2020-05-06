using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreePersonCamera_1stage : MonoBehaviour
{
    private float x = 0.0f;
    private float y = 0.0f;

    public GameObject target_go; //go = gameobject, Player 오브젝트 연결
    public float damping = 1;
    public Vector3 offset;

    public float currentAngle;
    public float desiredAngle;
    public float angle;

    public GameObject Criteria; //Criteria 오브젝트 연결

    void Start()
    {
        Vector3 angles = transform.eulerAngles;

        x = angles.y;
        y = angles.x;

        offset = target_go.transform.position - transform.position;
    }

    void Update()
    {
        currentAngle = transform.eulerAngles.y;
        desiredAngle = target_go.transform.eulerAngles.y;

        angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * damping);

        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        transform.position = target_go.transform.position - (offset);

        transform.LookAt(Criteria.transform);
    }
}
