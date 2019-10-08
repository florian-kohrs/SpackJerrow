using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileSimulator
{

    float ProjectileSpeed { get; }

    Vector3 SpawnGlobalPosition { get; }

}
