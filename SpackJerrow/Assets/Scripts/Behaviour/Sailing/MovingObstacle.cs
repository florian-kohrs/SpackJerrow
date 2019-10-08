using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : Obstacle
{

    private IMovementPredicter direction;

    private IMovementPredicter Direction
    {
        get
        {
            if (direction == null)
            {
                direction = gameObject.GetInterface<IMovementPredicter>();
            }
            return direction;
        }
    }

    protected override Vector3 NewVelocity(WaterRigidBody w)
    {
        Vector3 vel = base.NewVelocity(w);

        Vector3 crashDirection = (w.GetPosition() - Direction.GetPosition()).normalized * Direction.GetDirection().magnitude;

        vel += crashDirection;

        return vel;
    }

}
