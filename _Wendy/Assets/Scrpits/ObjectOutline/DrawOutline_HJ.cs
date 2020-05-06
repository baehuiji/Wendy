using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawOutline_HJ : MonoBehaviour
{
    public List<EasyOutlineSystem> systems;

    void Start()
    {
        systems = new List<EasyOutlineSystem>();
        EasyOutlineSystem[] tempSystems = FindObjectsOfType<EasyOutlineSystem>();
        for (int i = 0; i < tempSystems.Length; i++)
        {
            systems.Add(tempSystems[i]);
        }

        //systems.Sort(new SortComparerForOutline_HJ());
        systems.Sort(
            delegate (EasyOutlineSystem a, EasyOutlineSystem b) { return a.index.CompareTo(b.index); });

    }

    void Update()
    {
    }

    public void set_enabled(int index, bool enabled)
    {
       systems[index].enabled = enabled;
    }

    public void set_destroy(int index)
    {
        EasyOutlineSystem tempOutline = systems[index];
        systems.RemoveAt(index);

        Destroy(tempOutline);
    }

    public void Set()
    {

    }
}
