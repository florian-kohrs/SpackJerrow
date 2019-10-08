using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovementPredicter : IMovementDirection
{

    Vector3 GetPosition();

}
