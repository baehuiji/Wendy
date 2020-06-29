using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckRange : MonoBehaviour
{
    [SerializeField]
    private float range = 2.5f;

    [SerializeField]
    private float length = 2.5f;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private GameObject target;

    [SerializeField]
    private GameObject P_target;

    private RaycastHit hitInfo;

    public Collider sectorRange;

    void Start()
    {
        sectorRange = GetComponent<Collider>();
    }

    void Update()
    {

    }

    public bool checkItem()
    {
        //1
        //if (sectorRange.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, 2.5f, layerMask))
        //{
        //    return true;
        //}
        //return false;

        //2
        Ray ray = new Ray(transform.position, transform.forward);
        if (sectorRange.Raycast(ray, out hitInfo, 2.5f))
        {
            return true;
        }

        return false;
    }

    public RaycastHit Get_hitInfo()
    {
        return hitInfo;
    }


    void OnTriggerStay(Collider other)
    {
        //checkItem();

        //Vector3 thisPos = new Vector3(transform.position.x, 0f, transform.position.z);
        //Vecotr3 otherPos = new Vecotr3(other.)
    }


    void OnTriggerExit(Collider other)
    {

    }

}
