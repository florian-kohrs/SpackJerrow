using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this script gives a vector 3 for the camera to spectate the object
/// </summary>
public class CameraDirector : MonoBehaviour
{

    public float spectatorHeight = 8;

    public float distance = 10;

    public float duration = 2;

    private IEnumerator rotator;

    public IEnumerator Rotator => rotator;

    private IEnumerator transformer;

    public IEnumerator Transformer => transformer;

    private System.Action onStop; 
    
    public void StopCameraAnimation()
    {
        if(rotator != null)
        {
            StopCoroutine(rotator);
        }
        if(transformer != null)
        {
            StopCoroutine(transformer);
        }
        onStop?.Invoke();
    }

    private void Finish()
    {
        StopCameraAnimation();
        rotator = null;
        transformer = null;
        onStop = null;
    }

    public Vector3 GetCameraSpectatePoint(Vector3 cameraPosition)
    {
        Vector3 direction = transform.position - cameraPosition;
        direction.y = 0;
        direction.Normalize();
        direction *= distance;
        direction.y = spectatorHeight;
        return direction;
    }

    public void CameraSpectateTransform(Vector3 cameraPosition, out Vector3 spectatePosition, out Vector3 spectateEuler)
    {
        Vector3 direction = transform.position - cameraPosition;
        direction.y = 0;
        direction.Normalize();
        direction *= distance;
        spectatePosition = transform.position - direction;
        spectatePosition.y = spectatorHeight + transform.position.y;
        //spectatePosition = transform.position;
        //spectatePosition.y += 40;

        Vector3 cameraDirection = transform.position - spectatePosition;

        Transform t = new GameObject().transform;
        t.position = spectatePosition;
        t.LookAt(transform.position);

        spectateEuler = t.eulerAngles;

        Destroy(t.gameObject);

        //spectateRotation = Quaternion.FromToRotation(spectatePosition, transform.position);
        //spectateRotation = Quaternion.FromToRotation(spectatePosition, transform.position).eulerAngles;
        //spectateRotation.z = 0;
    }

    public void AnimateCamera(Transform cam, bool freezePlayer = true, System.Action onStop = null)
    {
        if (freezePlayer)
        {
            GameManager.FreezePlayer();
        }

        this.onStop = onStop;

        Vector3 observationEuler;
        Vector3 observationPosition;
        CameraSpectateTransform(cam.position, out observationPosition, out observationEuler);

        rotator = SmoothTransformation<Vector3>.SmoothRotateAngleEuler(cam.eulerAngles, observationEuler, 2.5f, v => cam.eulerAngles = v);
        StartCoroutine(Rotator);
        transformer  =
        SmoothTransformation<Vector3>.SmoothRotateEuler(cam.position, observationPosition, 2.5f, v => cam.position = v, () =>
        {
            transformer = this.DoDelayed(1.5f, Finish);
        });
        StartCoroutine(transformer);
    }

}
