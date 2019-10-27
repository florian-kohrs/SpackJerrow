using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this script gives a vector 3 for the camera to spectate the object
/// </summary>
public class CameraDirector : MonoBehaviour
{

    public float spectatorHeight;

    public float distance;

    public Vector3 GetCameraSpectatePoint(Vector3 cameraPosition)
    {
        Vector3 direction = transform.position - cameraPosition;
        direction.y = 0;
        direction.Normalize();
        direction *= distance;
        direction.y = spectatorHeight;
        return direction;
    }

    public void CameraSpectateTransform(Vector3 cameraPosition, out Vector3 spectatePosition, out Vector3 spectateRotation)
    {
        Vector3 direction = transform.position - cameraPosition;
        direction.y = 0;
        direction.Normalize();
        direction *= distance;
        direction.y = spectatorHeight;
        spectatePosition = direction;

        //spectateRotation = Quaternion.FromToRotation(spectatePosition, transform.position);
        spectateRotation = Quaternion.LookRotation(direction, Vector3.up).eulerAngles;
    }

    public void AnimateCamera(Transform cam, bool freezePlayer = true)
    {
        if (freezePlayer)
        {
            GameManager.FreezePlayer();
        }
        
        StartCoroutine(SmoothTransformation<Vector3>.SmoothRotateAngleEuler(cam.eulerAngles, observationEuler, 2.5f, v => cam.eulerAngles = v));
        StartCoroutine(
        SmoothTransformation<Vector3>.SmoothRotateEuler(cam.position, observationPosition, 2.5f, v => cam.position = v, () =>
        {
            source.StartCoroutine(SmoothTransformation<Vector3>.SmoothRotateAngleEuler(observationPosition, observationPosition, 1.5f, v => { }, () =>
            {
                cam.localEulerAngles = euler;
                cam.localPosition = pos;
            }));
        })
          );
    }

}
