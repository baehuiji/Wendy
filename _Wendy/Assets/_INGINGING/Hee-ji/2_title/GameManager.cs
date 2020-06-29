using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance = null;

    public float brightness = 1f;
    public float fx = 0.5f;
    public float music = 0.5f;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // TitleButton 클래스에 임시로 있음

    }

    void Update()
    {
        
    }
}
