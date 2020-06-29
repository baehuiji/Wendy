using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_Option : MonoBehaviour
{
    private static GameManager_Option instance;

    public static GameManager_Option Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<GameManager_Option>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newSingleton = new GameObject("Singleton Class").AddComponent<GameManager_Option>();

                    instance = newSingleton;
                }
            }
            return instance;
        }

        private set
        {
            instance = value;
        }
    }

    private void Awake()
    {
        var objs = FindObjectsOfType<GameManager_Option>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
}
