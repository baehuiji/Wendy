using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController_Ending : MonoBehaviour
{
    public Inventory theInventory;
    private SelectSlot selectSlot_script;

    [SerializeField]
    private float range;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private Text actionText;

    private bool openActivated = false;
    private RaycastHit hitInfo;

    public GameObject Aim;

    private EndingVideo_Loading loadEnding_script;


    void Start()
    {
        selectSlot_script = GameObject.FindObjectOfType<SelectSlot>();

        loadEnding_script = GameObject.FindObjectOfType<EndingVideo_Loading>();
    }

    void Update()
    {
        CheckHit();

        TryAction();
    }

    private void TryAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CanOpen();
        }
    }

    private void CheckHit()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "Door") //compare @
            {
                InfoAppear();
            }
        }
    }

    private void CanOpen()
    {
        if (openActivated)
        {
            if (hitInfo.transform != null)
            {
                // - 선택슬롯에 아무것도 없을때 (열쇠꽂기실패)
                if (theInventory.IsVoid_Slot(selectSlot_script.get_index()))
                {
                    //실패 사운드 @

                    return;
                }

                // - 선택슬롯에 무언가 있을때
                int select_itemCode = theInventory.get_ItemCode(selectSlot_script.get_index());
                Debug.Log(select_itemCode.ToString());

                if (select_itemCode == 40)
                {
                    // - 선택슬롯이 열쇠를 가리키면 (성공)

                    //영상틀기
                    loadEnding_script.InStartFadeAnim();

                    //Aim.SetActive(false);
                }
                else
                {
                    // - (실패)
                    //실패 사운드 @

                }
            }
        }
    }

    private void InfoAppear()
    {
        openActivated = true;
        //actionText.gameObject.SetActive(true);
        //actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득" + " [Click]";
    }
    public void InfoDisappear()
    {
        openActivated = false;
        //actionText.gameObject.SetActive(false);
    }
}
