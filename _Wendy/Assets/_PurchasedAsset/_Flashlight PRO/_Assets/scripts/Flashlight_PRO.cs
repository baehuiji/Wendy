using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Flashlight_PRO : MonoBehaviour
{
    [Space(10)]
    [SerializeField()]
    GameObject Lights; // all light effects and spotlight


    private Light spotlight;
    private Material ambient_light_material;
    private Color ambient_mat_color;
    private bool is_enabled = false;

    bool value;

    // FlashCount on off count
    int FlashCount = 0;

    [SerializeField]
    private Text actionText;

    // Use this for initialization
    void Start()
    {
        value = true;
        Switch(true);
        // cache components
        spotlight = Lights.transform.Find("Spotlight").GetComponent<Light>();
        ambient_light_material = Lights.transform.Find("ambient").GetComponent<Renderer>().material;
        ambient_mat_color = ambient_light_material.GetColor("_TintColor");

        FlashLightAppear();
    }

    void Update()
    {
        if (FlashCount == 0)
        {
            FlashLightAppear();
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            if (value)
            {
                Debug.Log("불켜기실행");
                value = false;
                Switch(false);
                // Enable_Particles(false);
                SoundManger.instance.PlaySound("Switch");
            }
            else
            {
                Debug.Log("불끄기실행");
                value = true;
                Switch(true);
                // Enable_Particles(true);
                SoundManger.instance.PlaySound("Switch");
            }
            Switch(value);
            //  Enable_Particles(value);

            FlashCount++;
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
        actionText.gameObject.SetActive(true);
        actionText.text = "Q를 눌러 손전등 켜기";
    }
    private void FlashLightDisappear()
    {
        actionText.gameObject.SetActive(false);
    }
}
