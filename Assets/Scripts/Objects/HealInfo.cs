using UnityEngine;
using System.Collections;

public enum HealType
{
    DEFAULT,
    BLUE,
    GREEN,
    RED,
    YELLOW
}


public struct HealInfo
{
    public HealType type;
}
