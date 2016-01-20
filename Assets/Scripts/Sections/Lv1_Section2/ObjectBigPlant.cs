using UnityEngine;
using System.Collections;
using System;

public class ObjectBigPlant : ObjectData, IHealable
{




    public bool IsHealable()
    {
        return true;
    }

    public void OnHealed(HealInfo heal)
    {
        throw new NotImplementedException();
    }
}
