using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface GunInterface
{
    public void Shoot(Vector2 coordinates , ulong clientID , Vector3 origin);
}
