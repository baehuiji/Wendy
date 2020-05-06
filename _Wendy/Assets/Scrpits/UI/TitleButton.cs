using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleButton : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene("02_3_Stage copy");
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    void Start()
    {
        Screen.SetResolution(1920, 1080, false);
    }

    void Update()
    {
        
    }
}
