using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellarDoorCollider : MonoBehaviour
{
    public Text guideCaption;
    ChangeCam_2stage ChangeCam_script;
    bool textstate = true;


    [SerializeField]
    private float range;
    [SerializeField]
    private LayerMask layerMask;
    private RaycastHit hitInfo;

    Camera _mainCam;

    void Start()
    {
        ChangeCam_script = FindObjectOfType<ChangeCam_2stage>();
        guideCaption.gameObject.SetActive(false);

        _mainCam = Camera.main;
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            //Debug.Log("충돌! 페이드 인 아웃 실행가능 범위인가?");
            //  Tinkerbell_ani.Play();
            if (textstate)
            {
                if (LookAtDoor())
                {
                    GuidText();
                }
            }
            else
                NotText();

            if (Input.GetMouseButtonDown(0))
            {
                if (LookAtDoor())
                {
                    ChangeCam_script.change_Camera(1);
                    textstate = false;
                }
            }
        }
    }

    bool LookAtDoor()
    {
        if (Physics.Raycast(_mainCam.transform.position, _mainCam.transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
        {
            return true;
        }
        return false;
    }

    void GuidText()
    {
        guideCaption.gameObject.SetActive(true);
        guideCaption.text = "지하실 살펴보기";
    }

    void NotText()
    {
        //guideCaption.gameObject.SetActive(false);
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            guideCaption.gameObject.SetActive(false);
            textstate = true;
        }
    }
}
