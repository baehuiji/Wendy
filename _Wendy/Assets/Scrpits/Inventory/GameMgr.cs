using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    public Transform[] points;
    public RectTransform Inventory_Panel;
    public RectTransform scrollRectView;
    private bool check;

    public CanvasGroup inventoryCG;

    private bool isPaused;

    Animator animator;

    // - 선택슬롯
    public SelectSlot selectSlot_script;

    void Start()
    {
        check = false;

        inventoryCG.interactable = false;
        inventoryCG.blocksRaycasts = false;

        //RectTransform scrollRectView = GetComponent<RectTransform>();
        //RectTransform Inventory_Panel = GetComponent<RectTransform>();

        animator = Inventory_Panel.GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (check)
            {
                check = false;
                SlideInventory(false);
            }
            else
            {
                check = true;
                SlideInventory(true);
            }

            OnInventoryOpen(check);
        }

        //Move scroll
        if (check == true)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                selectSlot_script.Set_slotPos_index(-1); //이동보간

                if (selectSlot_script.select_EndSlot)
                    scrollRectView.anchoredPosition += new Vector2(0, -10);
                //Debug.Log("위 실행"); 
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                selectSlot_script.Set_slotPos_index(1);

                if (selectSlot_script.select_EndSlot)
                    scrollRectView.anchoredPosition += new Vector2(0, 10);
                //Debug.Log("아래 실행"); 
            }
        }
    }

    public void SlideInventory(bool is2Opened)
    {
        //Animator animator = Inventory_Panel.GetComponent<Animator>();
        if (Inventory_Panel != null)
        {
            bool isOpen = animator.GetBool("open");

            animator.SetBool("open", !isOpen);
        }
    }

    public void OnInventoryOpen(bool isOpened)
    {
        inventoryCG.interactable = isOpened;
        inventoryCG.blocksRaycasts = isOpened;
        StartCoroutine("WaitForAnimation");
    }
    bool EndAnimationDone()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.CloseInventory") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f; //==true 이면, 애니메이션 끝남
        //false == animator.IsInTransition(0)
    }
    IEnumerator WaitForAnimation()
    {
        while (!EndAnimationDone())
        {
            yield return new WaitForEndOfFrame();
        }

        // - 닫았을때
        scrollRectView.anchoredPosition = new Vector2(0.0f, -144.6f); //인벤토리 애니메이션이 끝날때 호출
    }
}
