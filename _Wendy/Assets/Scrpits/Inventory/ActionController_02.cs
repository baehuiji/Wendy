/////////////////////Inventory action/////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class ActionController_02 : MonoBehaviour
{
    public Inventory theInventory;

    [SerializeField]
    private float range;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private Text actionText;

    /// acquire true - false 
    private bool pickupActivated = false;
    private RaycastHit hitInfo;

    /// Note item
    public GameObject Note1;
    public GameObject Note2;
    public GameObject Note3;
    public GameObject Note4;

    // item event
    public GameObject FlashlightItem;
    public GameObject Wendy_animation;
    public GameObject Stagecollider;
    public GameObject StageLight;

    // item event count
    private int DollCount = 0;


    void Start()
    {
        Note1.SetActive(false);
        Note2.SetActive(false);
        Note3.SetActive(false);
        Note4.SetActive(false);
        FlashlightItem.SetActive(false);
        StageLight.SetActive(false);
    }


    void Update()
    {
        CheckItem();
        TryAction();
        if (DollCount == 8)
        {
            SoundManger.instance.PlaySound("Click_Loud");
            WendyAnimation();
            DollCount++;
        }

    }


    private void TryAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckItem();
            CanPickUp();
        }
    }

    private void CanPickUp()
    {
        if (pickupActivated)
        {
            if (hitInfo.transform != null)
            {
                if (hitInfo.transform.tag == "ItemSet") 
                {
                  theInventory.RemoveSlot(hitInfo.transform.GetComponent<ItemSetUp>().item);

                    if (theInventory.Remove_Count)
                    {
                        hitInfo.transform.GetComponent<ItemSetUp>().SetItem.SetActive(true);

                        if (hitInfo.transform.GetComponent<ItemSetUp>().item.itemName == "Doll")
                        {
                            DollCount++;
                            Debug.Log(DollCount);
                        }

                    }
                }

                if (hitInfo.transform.tag == "Item")
                {
                    Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + "획득했습니다");
                    theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item);
                    Destroy(hitInfo.transform.gameObject);
                    InfoDisappear();



                    if (hitInfo.transform.GetComponent<ItemPickUp>().item.itemName == "Note1")
                    {
                        Note1.SetActive(true);
                    }

                    if (hitInfo.transform.GetComponent<ItemPickUp>().item.itemName == "Note2")
                    {
                        Note2.SetActive(true);
                    }

                    if (hitInfo.transform.GetComponent<ItemPickUp>().item.itemName == "Note3")
                    {
                        Note3.SetActive(true);
                    }

                    if (hitInfo.transform.GetComponent<ItemPickUp>().item.itemName == "Note4")
                    {
                        Note4.SetActive(true);
                    }

                    if (hitInfo.transform.GetComponent<ItemPickUp>().item.itemName == "FlashlightItem")
                    {
                        FlashlightItem.SetActive(true);
                    }

                    if (hitInfo.transform.GetComponent<ItemPickUp>().item.itemName == "Wendy")
                    {
                        Debug.Log("웬디 입수완료 실행");
                        SoundManger.instance.PlaySound("light");
                        StageLight.SetActive(true);
                        Stagecollider.SetActive(false); // 뭔가 안됨. 이상함
                    }
                }
            }
        }
    }

    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }

            if (hitInfo.transform.tag == "ItemSet")
            {
                Dollcabinet();
            }
        }
        else
            InfoDisappear();
    }




    // Need to modify

    private void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + "획득" + "[Click]";
    }

    public void InfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }


    public void Dollcabinet()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = "장식장에 인형두기";


    }



    public void WendyAnimation()
    {      
        Animator animator = Wendy_animation.GetComponent<Animator>();
        animator.SetBool("Doll_Set", true);
    }

}
