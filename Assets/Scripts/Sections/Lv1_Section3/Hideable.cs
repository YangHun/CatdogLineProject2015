using UnityEngine;
using System.Collections;

public class Hideable : UnitData {

    private bool hide = false;

    public void Sethide(bool a)
    {
        hide = a;
    }

    public bool IsHide()
    {
        return hide;
    }
}
