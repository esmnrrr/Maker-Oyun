using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitcher : MonoBehaviour
{
    public Camera camClose;
    public Camera camMid;
    public Camera camFar;
    public Transform player;

    public Vector3 offsetClose = new Vector3(0, 1.5f, -3f);
    public Vector3 offsetMid = new Vector3(0, 2f, -6f);
    public Vector3 offsetFar = new Vector3(0, 3f, -10f);

    private int currentCamIndex = 1; // 0: close, 1: mid, 2: far

    void Start()
    {
        SetActiveCamera(currentCamIndex); // Başlangıçta orta kamera
    }

    public void OnCamChange(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("CamChange tuşuna basıldı!");
            SwitchCamera();
        }
    }


    private void SwitchCamera()
    {
        currentCamIndex = (currentCamIndex + 1) % 3;
        SetActiveCamera(currentCamIndex);
    }

    private void SetActiveCamera(int index)
    {
        DisableAllCameras();

        switch (index)
        {
            case 0:
                camClose.enabled = true;
                SetCameraFollow(camClose, offsetClose);
                EnableAudioListener(camClose);
                break;
            case 1:
                camMid.enabled = true;
                SetCameraFollow(camMid, offsetMid);
                EnableAudioListener(camMid);
                break;
            case 2:
                camFar.enabled = true;
                SetCameraFollow(camFar, offsetFar);
                EnableAudioListener(camFar);
                break;
        }
    }

    private void SetCameraFollow(Camera cam, Vector3 offset)
    {
        var follow = cam.GetComponent<CameraFollow>();
        if (follow == null)
            follow = cam.gameObject.AddComponent<CameraFollow>();

        follow.target = player;
        follow.offset = offset;
    }

    private void DisableAllCameras()
    {
        camClose.enabled = false;
        camMid.enabled = false;
        camFar.enabled = false;
    }

    private void EnableAudioListener(Camera activeCamera)
    {
        camClose.GetComponent<AudioListener>().enabled = false;
        camMid.GetComponent<AudioListener>().enabled = false;
        camFar.GetComponent<AudioListener>().enabled = false;

        activeCamera.GetComponent<AudioListener>().enabled = true;
    }
}

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
            transform.LookAt(target);
        }
    }
}
