using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAndWallCollide_temp : MonoBehaviour
{
    public FollowCamera_temp followCamera;

    public bool check;

    void Start()
    {
        followCamera = GameObject.Find("MainCamera").GetComponent<FollowCamera_temp>();
        check = false;
    }

    void Update()
    {


    }

    void OnTriggerStay(Collider coll)
    {
        if (coll.gameObject.tag == "Wall")
            check = true;
    }

    void OnTriggerExit(Collider coll)
    {
        check = false;
    }
}
