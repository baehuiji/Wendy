using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FadeManager : MonoBehaviour
{

    public Image Panel;
    float time = 0f;
    float F_time = 1f;
    private Color aplha;
    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);




    public void FadeOut()
    {
        StartCoroutine(FadeOutFlow());
    }
    IEnumerator FadeOutFlow()
    {
        Color alpha = Panel.color;
        time = 0f;

        Panel.gameObject.SetActive(true);

        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            Panel.color = alpha;
            yield return waitTime;
        }
    }



    public void FadeIn()
    {
        StartCoroutine(FadeInFlow());
    }
    IEnumerator FadeInFlow()
    {
        Color alpha = Panel.color;
        time = 0f;

        while (alpha.a > 0f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            Panel.color = alpha;
            yield return waitTime;
        }
        Panel.gameObject.SetActive(false);

    }






}
