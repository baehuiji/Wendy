using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearManager : MonoBehaviour
{
    public GameObject gearParent;
    private Gear_Move[] gears;

    public GameObject bgearParent;
    private backGear_Move[] bgears;

    void Start()
    {
        gears = gearParent.GetComponentsInChildren<Gear_Move>();

        for(int i = 0; i < 8; i++)
        {
            gears[i].set_gearNumber(i);
        }

        bgears = bgearParent.GetComponentsInChildren<backGear_Move>();
        for (int i = 0; i < bgears.Length; i++)
        {
            bgears[i].set_gearNumber(i);
        }
    }

    //void Update()
    //{
        
    //}
    
    public void set_unActiveGear(int index)
    {
        gears[index].gear_unActive();
    }

    public void set_delayOnGear(int index)
    {
        gears[index].set_delayOn(false);
    }

    public void set_Active_backGears(bool b, bool f)
    {
        bgears[0].start_Rotation(b, f);
        bgears[1].start_Rotation(b, f);
    }
}
