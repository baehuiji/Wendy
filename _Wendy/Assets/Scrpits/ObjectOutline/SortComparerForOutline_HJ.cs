using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortComparerForOutline_HJ : IComparer<EasyOutlineSystem>
{
    public int Compare(EasyOutlineSystem x, EasyOutlineSystem y)
    {
        if (x.index > y.index)
            return 1;

        if (x.index == y.index)
            return 0;

        if (x.index < y.index)
            return -1;

        return 0;
    }
}
