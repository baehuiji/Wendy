using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageNote : MonoBehaviour
{
    NoteManger notemager;
    private bool check = false;

    // Start is called before the first frame update
    void Start()
    {
        notemager = FindObjectOfType<NoteManger>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckAddcount(int i)
    {
        if (!check)
        {
            notemager.AddCount(1);
            check = true;
        }
        else
            return;
    }
}
