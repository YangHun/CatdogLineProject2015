using UnityEngine;
using System.Collections;

public interface IHealable
{
    Vector2 GetPosition();
    void OnHealed(HealInfo heal);
    bool IsHealable();
}
