using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera_temp : MonoBehaviour
{
    public GameObject target;
    public float damping = 1;
    public Vector3 offset;

    public float currentAngle;
    public float desiredAngle;
    public float angle;

    public float zPosMin = 1.2f; //0.2가 최대
    public float zPosMax = 2.4f;
    float zOffset;

    //RaycastHit hit;
    float MaxDistance = 8f;
    public LayerMask layerMask;
    int layermask;

    public GameObject footObj; //foot
    Vector3 footPos;

    public GameObject viewObj; //view_point
    Vector3 viewPos;

    private CameraAndWallCollide_temp cameraColl;

    void Start()
    {
        offset = target.transform.position - transform.position;
        footPos = footObj.transform.position;
        viewPos = viewObj.transform.position;
        layermask = 1 << LayerMask.NameToLayer("CameraObstacle");
        cameraColl = GameObject.Find("camera_wall_coll").GetComponent<CameraAndWallCollide_temp>();
    }

    void LateUpdate()
    {
        //각도
        currentAngle = transform.eulerAngles.y;
        desiredAngle = target.transform.eulerAngles.y;

        angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * damping);

        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        transform.position = target.transform.position - (rotation * offset);

        transform.LookAt(target.transform);

        //시야
        Ray ray = new Ray(viewPos, footPos - viewPos);
        //Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow, 0.3f);

        RaycastHit hit;
        footPos = footObj.transform.position;
        viewPos = transform.position;

        if (Physics.Raycast(ray, out hit, MaxDistance, layermask))
        {
            zOffset = offset.z;

            if (hit.transform.tag.Equals("Wall"))//(hit.collider.tag == "Wall")
            {
                if (!cameraColl.check)
                    return;

                if (zOffset >= zPosMin)
                {
                    Vector3 vec3 = new Vector3(0f, 0f, Time.deltaTime * 0.5f);

                    //offset -= vec3;
                    offset = ClampOffset(offset, vec3, zPosMin, zPosMax, -1);
                }
            }
            else if (hit.transform.tag.Equals("Floor"))//(hit.collider.tag == "Floor")
            {
                if (cameraColl.check)
                    return;

                if (zOffset <= zPosMax)
                {
                    Vector3 vec3 = new Vector3(0f, 0f, Time.deltaTime * 0.5f);

                    offset = ClampOffset(offset, vec3, zPosMin, zPosMax, 1);

                }
            }
        }
    }

    Vector3 ClampOffset(Vector3 ofs, Vector3 vec, float zMin, float ZMax, int a) //a는 min max 중에 어떤 것을 검사할 건지
    {
        float _z = ofs.z;

        if (a == -1)
            _z = Mathf.Clamp(ofs.z - vec.z, zMin, ZMax);
        else if (a == 1)
            _z = Mathf.Clamp(ofs.z + vec.z, zMin, ZMax);

        ofs = new Vector3(ofs.x, ofs.y, _z);

        return ofs;
    }
}
