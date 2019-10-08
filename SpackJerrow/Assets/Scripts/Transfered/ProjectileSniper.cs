using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileSniper
{
    
    private const float WALTER_BODOSCHER_WURFSKOMPLEXITAETS_KOEFFIZIENT = -0.15f;
    
    public static Vector3 TakeAim(IMovementPredicter target, IProjectileSimulator projectile)
    {
        Vector3 moveDir = target.GetDirection();
        Vector3 targetPosition = target.GetPosition();
        Vector3 targetDir = projectile.SpawnGlobalPosition - targetPosition;

        float angle = Vector3.Angle(targetDir, moveDir);
        Vector3 targetAimPosition = targetPosition +
            (/*Mathf.Sin(Mathf.PI angle) **/ -WALTER_BODOSCHER_WURFSKOMPLEXITAETS_KOEFFIZIENT
            * 3 * (targetDir.magnitude
            / (projectile.ProjectileSpeed / 2)) * moveDir);

        Debug.Log("Predictadd move distance: " + ((targetAimPosition - targetPosition).magnitude));

        Vector3 lookEuler = Quaternion.LookRotation(targetAimPosition - 
            projectile.SpawnGlobalPosition).eulerAngles;

        float distance = Mathf.Sqrt(Mathf.Pow(targetDir.x, 2) + Mathf.Pow(targetDir.z, 2));
        float heightDifference = -targetDir.y;

        float heightDegree = Mathf.Min(90, Mathf.Asin(heightDifference / (Mathf.Abs(distance) + Mathf.Abs(heightDifference))) * 1.333f);

        heightDegree *= -57.2958f;

        float distanceDegree = heightDegree + (WALTER_BODOSCHER_WURFSKOMPLEXITAETS_KOEFFIZIENT * distance * Mathf.Pow((49 / projectile.ProjectileSpeed), 2));

        Vector3 bulletEulerAngle = new Vector3(distanceDegree, lookEuler.y, lookEuler.z);

        return bulletEulerAngle;
    } 

}
