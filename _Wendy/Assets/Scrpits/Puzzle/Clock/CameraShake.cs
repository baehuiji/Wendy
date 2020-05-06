using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeAmount;

    private float shakeTime;
    public float temp;
    Vector3 initialPosition;

    bool state = false;


    public void VibrateForTime(float time)
    {
        shakeTime = time;
    }

    private void Start()
    {
        initialPosition = transform.position;

    }

    private void Update()
    {

    }

    public void set_CameraShake()
    {
        if (state)
            return;

        StartCoroutine(start_camera_shake());
    }

    IEnumerator start_camera_shake()
    {
        state = true;

        VibrateForTime(temp);

        initialPosition = transform.position;

        bool test = false;

        while (shakeTime > 0)
        {
            transform.position = Random.insideUnitSphere * shakeAmount + initialPosition;
            shakeTime -= Time.deltaTime;

            if (!test)
            {
                shakeAmount += 0.0000625f;

                if (shakeAmount >= 0.015f) //0.02
                {
                    test = true;
                }
            }
            else
            {
                if (shakeAmount > 0)
                {
                    shakeAmount -= 0.0001f;//0.0000625f;
                }
                else
                {
                    shakeAmount = 0;
                }
            }

            yield return new WaitForSeconds(0.01f);
        }

        shakeAmount = 0.0f;
        shakeTime = 0.0f;

        transform.position = initialPosition;

        state = false;
    }
}