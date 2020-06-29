using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxOpen : MonoBehaviour
{
    [SerializeField]
    private string boxsound;

    Animator animator;



    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    public void set_aniBool()
    {
        SoundManger.instance.PlaySound(boxsound);


        animator.SetBool("IsOpen", true);
    }
}
