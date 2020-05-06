//NewVersion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputClockAnswer : MonoBehaviour
{
    //2. 스크립트를 각각의 입력 위치에 붙여주는 방법
    public int PanelNumber = 0; //인스펙터창에 초기화, 인풋위치에 관한 넘버로 없어도됨
    public int InputNum = 1;

    // - 회전타겟
    public Transform CirclePanel;  //인스펙터창에 초기화
    public Transform rotTarget;
    private float rotateValue;
    private bool rotState = false;
    public float speed;

    void Start()
    {
        rotateValue -= 30f;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {

    }

    public void click_button()
    {
        if (rotState) //이미 돌아가고 있는 상태인지
            return;

        Debug.Log("Dddd");

        rotState = true;
        StartCoroutine("Rotate_Dial");
    }

    IEnumerator Rotate_Dial()
    {
        rotTarget.Rotate(Vector3.right * rotateValue);

        while (CirclePanel.rotation.eulerAngles.x != rotTarget.rotation.eulerAngles.x)
        {
            float step = speed * Time.deltaTime;

            CirclePanel.rotation = Quaternion.RotateTowards(CirclePanel.rotation, rotTarget.rotation, step);

            yield return new WaitForSeconds(0.01f);
        }

        rotState = false;

        InputNum++;
        OverNumber();
    }

    private void OverNumber()
    {
        if (InputNum > 12)
            InputNum = 1;
        if (InputNum < 1)
            InputNum = 12;
    }
}
