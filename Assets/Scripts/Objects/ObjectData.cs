using UnityEngine;
using System.Collections;

public enum ObjectType
{
    NONE,
    PLAYER,
    FOO,
    BAR
}

public class ObjectData : MonoBehaviour
{
    public virtual ObjectType GetObjectType() { return ObjectType.NONE; }
    public virtual Vector2 GetPosition() { return transform.position; }
}
