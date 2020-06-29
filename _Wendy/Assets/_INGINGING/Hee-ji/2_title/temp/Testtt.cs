using UnityEngine;
using System.Collections;

public class Testtt : MonoBehaviour
{
    public static Testtt instance = null;              
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}
