using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivePuzzle_CP : MonoBehaviour
{
    ClockPuzzle_Manager CPmanager_script;

    public bool puzzleEnd = false;

    void Start()
    {
        CPmanager_script = GameObject.FindObjectOfType<ClockPuzzle_Manager>();
    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (puzzleEnd)
            return;

        if (other.transform.CompareTag("Player"))
        {
            CPmanager_script.set_active(true);
            CPmanager_script.enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (puzzleEnd)
            return;

        if (other.transform.CompareTag("Player"))
        {
            CPmanager_script.release_collider();
            CPmanager_script.set_active(false);
            CPmanager_script.enabled = false;
        }
    }

    public void set_puzzleEnd() //퍼즐이 끝났을때
    {
        puzzleEnd = true;
    }
}
