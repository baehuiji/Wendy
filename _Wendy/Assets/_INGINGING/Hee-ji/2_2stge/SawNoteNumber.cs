using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawNoteNumber : MonoBehaviour
{
    public static SawNoteNumber _note_num_instance = null;
    NoteManger notemager;

    public int noteCount = 0;

    void Awake()
    {
        if (_note_num_instance == null)
        {
            _note_num_instance = this;
        }
        else if (_note_num_instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        notemager = FindObjectOfType<NoteManger>();
    }

    void Update()
    {

    }

    public void SetNoteCount()
    {
        noteCount = notemager.CheckCount;
    }

    public void init_2stage()
    {
        notemager = FindObjectOfType<NoteManger>();
        for (int i = 0; i < noteCount; i++)
        {
            notemager.AddCount(1);
        }        
    }
}
