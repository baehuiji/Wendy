using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController_proto : MonoBehaviour
{
    public AudioSource[] _BGM;
    public AudioSource[] _SFX;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void play_btnOn()
    {
        _SFX[0].Play();
    }

    public void play_btnOff()
    {
        _SFX[1].Play();
    }

    public void play_reset()
    {
        _SFX[2].Play();
    }

    public void play_wood1()
    {
        _SFX[3].Play();
    }

    public void play_wood2()
    {
        _SFX[4].Play();
    }

    public void play_wood3()
    {
        _SFX[5].Play();
    }
}
