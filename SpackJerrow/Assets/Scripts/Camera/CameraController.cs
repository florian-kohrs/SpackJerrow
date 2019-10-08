using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float sensitivity = 22.0f;
    public float smoothing = 5.0f;
    public Transform charackter;

    public float maxRotation;
    public float mouseMaxSpeedPerFrame = 10;

    private Vector2 mouseLook;
    private Vector2 smoothedMouseLook;

    public float maxDownRotation = -50;

    public float maxUpRotation = 50;

    public float distanceFromPlayer = 3;

    private float cameraVelocity;

    private float rotateVelocity;

    public Transform maxCameraScrollAnchor;

    public Transform minCameraScrollAnchor;

    public float scrollSpeed = 10;

    public float scrollZoomValue = 0;

    void Start()
    {
        charackter = transform.parent;
        float startZEuler = charackter.localEulerAngles.z;
        if (startZEuler > 180)
        {

            startZEuler -= 360;
        }
        startZEuler *= -1;

        mouseLook = new Vector2(charackter.localEulerAngles.y, startZEuler);
        smoothedMouseLook = mouseLook;
    }

 

    void LateUpdate()
    {
        if (GameManager.CanCameraMove)
        {
            RotateCamera();
            ScrollCamera();
        }
    }
    
    private void ScrollCamera()
    {
        scrollZoomValue = Mathf.Clamp01(scrollZoomValue + Input.mouseScrollDelta.y * Time.deltaTime * scrollSpeed);
        transform.position = Vector3.Lerp(maxCameraScrollAnchor.position, minCameraScrollAnchor.position, scrollZoomValue);
    }

    private void RotateCamera()
    {
        float sensitivity = PersistentGameDataController.Settings.cameraSensitivity;

        Vector2 mouseMovement = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        mouseMovement *= sensitivity * smoothing * Time.deltaTime;

        if (mouseMovement.y > 0)
        {
            mouseMovement.y = Mathf.Min(mouseMaxSpeedPerFrame, mouseMovement.y);
        }
        else
        {
            mouseMovement.y = Mathf.Max(-mouseMaxSpeedPerFrame, mouseMovement.y);
        }

        if (mouseMovement.y > 0)
        {
            mouseMovement.y = Mathf.Min(mouseMaxSpeedPerFrame, mouseMovement.y);
        }
        else
        {
            mouseMovement.y = Mathf.Max(-mouseMaxSpeedPerFrame, mouseMovement.y);
        }

        mouseLook += mouseMovement;
        if (mouseLook.y < 0 && mouseLook.y < maxDownRotation)
        {
            mouseLook.y = maxDownRotation;
        }
        else if (mouseLook.y > 0 && mouseLook.y > maxUpRotation)
        {
            mouseLook.y = maxUpRotation;
        }
        
        smoothedMouseLook.y = Mathf.Lerp(smoothedMouseLook.y, mouseLook.y, 1 / smoothing);
        smoothedMouseLook.x = Mathf.Lerp(smoothedMouseLook.x, mouseLook.x, 1 / smoothing);

        charackter.transform.localEulerAngles = new Vector3(-smoothedMouseLook.y, smoothedMouseLook.x, charackter.transform.localEulerAngles.z);
    }


}
