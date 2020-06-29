using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Flashlight_PRO : MonoBehaviour
{
    [Space(10)]
    [SerializeField()]
    GameObject Lights; // all light effects and spotlight

    [SerializeField]
    private string Swith_Sound;


    private Light spotlight;
    private Material ambient_light_material;
    private Color ambient_mat_color;
    private bool is_enabled = false;

    bool value;

    // FlashCount on off count
    int FlashCount = 0;

    [SerializeField]
    private Text actionText;
    [SerializeField]
    private Image actionImg;

    float FlickerT = 0f;
    bool delay = false;
    private WaitForSeconds waitTime = new WaitForSeconds(0.025f);



    // Use this for initialization
    void Start()
    {
        value = true;
        Switch(true);
        // cache components
        spotlight = Lights.transform.Find("Spotlight").GetComponent<Light>();
        ambient_light_material = Lights.transform.Find("ambient").GetComponent<Renderer>().material;
        ambient_mat_color = ambient_light_material.GetColor("_TintColor");

        spotlight.intensity = 2;
       

        FlashRandom();

        FlashLightAppear();
    }

    void FlashRandom()
    {
        StartCoroutine(Randomlight());
    }

    void Update()
    {
        if (FlashCount == 0)
        {
            FlashLightAppear();
        } 
        else
        {
            FlashLightDisappear();
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            if (value)
            {
                value = false;
                Switch(false);
                // Enable_Particles(false);
                SoundManger.instance.PlaySound(Swith_Sound);
            }
            else
            {
                value = true;
                Switch(true);
                // Enable_Particles(true);
                SoundManger.instance.PlaySound(Swith_Sound);
            }
            Switch(value);
            //  Enable_Particles(value);

            FlashCount++;
        }
    }


    IEnumerator Randomlight()
    {
        while (true)
        {
            FlickerT = 0;
            int a = Random.Range(2, 6);


            while (FlickerT <= 3.0f) // 선형보간이 진행됩니다. 선형보간의 이동이 끝날때까지! 
            {
                FlickerT += Time.deltaTime;

                while (a > 0)
                {
                    float w = Random.Range(0f, 1f);
                    float in_Light = Random.Range(0f, 1f);

                    spotlight.intensity = in_Light;
                    ambient_light_material.SetColor("_TintColor", new Color(ambient_mat_color.r, ambient_mat_color.g, ambient_mat_color.b, in_Light / 20));

                    yield return new WaitForSeconds(w);


                    a--;
                }

                //   Wall_E.transform.position = Vector3.Lerp(Wall_E.transform.position, E_WallStop, t);

            }

            spotlight.intensity = 2;
            ambient_light_material.SetColor("_TintColor", new Color(ambient_mat_color.r, ambient_mat_color.g, ambient_mat_color.b, 1f / 20));

            yield return new WaitForSeconds(20);

        }


    }

    /// <summary>
    /// changes the intensivity of lights from 0 to 100.
    /// call this from other scripts.
    /// </summary>
    public void Change_Intensivity(float percentage)
    {
        percentage = Mathf.Clamp(percentage, 0, 100);
        spotlight.intensity = (8 * percentage) / 100;
        ambient_light_material.SetColor("_TintColor", new Color(ambient_mat_color.r, ambient_mat_color.g, ambient_mat_color.b, percentage / 2000));
    }

    /// <summary>
    /// switch current state  ON / OFF.
    /// call this from other scripts.
    /// </summary>
    public void Switch(bool is_enabled)
    {
        is_enabled = !is_enabled;
        Lights.SetActive(is_enabled);

        //if (switch_sound != null)
        //	switch_sound.Play ();
    }

    private void FlashLightAppear()
    {
        //actionText.gameObject.SetActive(true);
        //actionText.text = "Q를 눌러 손전등 켜기";
        actionImg.gameObject.SetActive(true);
    }
    private void FlashLightDisappear()
    {
        //actionText.gameObject.SetActive(false);
        actionImg.gameObject.SetActive(false);
    }
}
