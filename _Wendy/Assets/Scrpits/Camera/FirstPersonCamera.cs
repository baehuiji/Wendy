using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public Transform target;
    public float dist = 0.9f;

    public float xSpeed = 100.0f; //220.0f;
    public float ySpeed = 100.0f;

    private float x = 0.0f;
    private float y = 0.0f;

    private float yMinLimit = -40f;
    private float yMaxLimit = 60f;

    public bool jumpScare_state = false; //화면 고정, translateTest에서 값 바뀜

    float m_FieldOfView = 58f; //50f, 30f
    public Camera cameraOption;
    bool once = false;

    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //커서 고정
        Vector3 angles = transform.eulerAngles;

        x = angles.y;
        y = angles.x;

        cameraOption = GetComponent<Camera>();
    }

    void Update()
    {
        if (jumpScare_state)
        {
            return;
        }

        //m_FieldOfView = 58f;
        //cameraOption.fieldOfView = m_FieldOfView;

        x += Input.GetAxis("Mouse X") * xSpeed * 0.015f;
        y -= Input.GetAxis("Mouse Y") * ySpeed * 0.015f;

        y = ClampAngle(y, yMinLimit, yMaxLimit);

        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 position = rotation * new Vector3(dist, dist, dist) + target.position;

        transform.rotation = rotation;
        target.rotation = Quaternion.Euler(0, x, 0);
        transform.position = position;
    }

    public void start_JumpScare()
    {
        // - 화면 확대
        if (!once)
        {
            m_FieldOfView = 50f;
            cameraOption.fieldOfView = m_FieldOfView;
        }

        jumpScare_state = true;
    }

    public void end_JumpScare()
    {
        // - 화면 축소 (기본값)
        if (!once)
        {
            m_FieldOfView = 58f;
            cameraOption.fieldOfView = m_FieldOfView;
            once = true;
        }

        jumpScare_state = false;
    }
}
