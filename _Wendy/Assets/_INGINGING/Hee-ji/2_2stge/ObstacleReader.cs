using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleReader : MonoBehaviour
{
    Camera camera;
    Ray Mouse_ray;
    RaycastHit hitInfo;
    private float range = 2.5f;
    int CamObstacle_layerMask;


    void Start()
    {
        camera = GetComponent<Camera>(); //메인카메라

        CamObstacle_layerMask = 1 << LayerMask.NameToLayer("Obstacle"); //-> X
    }

    void Update()
    {

    }

    public bool LookAtFrame(int layer)
    {
        Mouse_ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(Mouse_ray, out hitInfo, range, layer))
        {
            if (hitInfo.transform.CompareTag("Obstacle"))
            {
                return true;
            }

            //if (hitInfo.transform.CompareTag("Drawer"))
            //{
            //    return true;
            //}
        }
        else
        {
        }

        return false;
    }

}
