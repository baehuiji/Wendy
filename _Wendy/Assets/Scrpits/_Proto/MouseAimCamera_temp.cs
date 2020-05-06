//
//2019-11-18
//우클릭으로 카메라 에임 돌리기
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAimCamera_temp : MonoBehaviour
{
    public GameObject target;
    public GameObject player;
    public float rotateSpeed = 1;

    FollowCamera_temp followCamera;
    Vector3 offset;

    float angle;

    public bool gameEnd;

    void Start()
    {
        followCamera = GameObject.Find("MainCamera").GetComponent<FollowCamera_temp>();
        offset = followCamera.offset;

        gameEnd = false;
    }

    void LateUpdate()
    {
        if (gameEnd)
            return;

        if (Input.GetMouseButton(1))
        {
            offset = followCamera.offset;

            float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
            player.transform.Rotate(0, horizontal, 0);

            float currentAngle = transform.eulerAngles.y;
            float desiredAngleY = target.transform.eulerAngles.y;

            angle = Mathf.LerpAngle(currentAngle, desiredAngleY, Time.deltaTime * followCamera.damping);

            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            transform.position = target.transform.position - (rotation * offset);

            transform.LookAt(target.transform);
        }
    }
}
