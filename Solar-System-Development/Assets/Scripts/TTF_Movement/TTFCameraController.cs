using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTFCameraController : MonoBehaviour
{

    public Camera mainCamera;
    public Camera weaponCamera;
    public float sensX = 1f;
    public float sensY = 1f;
    public float baseFov = 90f;
    public float maxFov = 140f;
    public float wallRunTilt = 15f;

    public float wishTilt = 0;
    public float curTilt = 0;
    public Vector2 currentLook;
    public Vector2 sway = Vector3.zero;
    public float fov;
    public Rigidbody rb;

    //Mouse Control Camera
    [Header("Mouse settings")]
    public float mouseSensitivityMultiplier = 1;
    public float maxMouseSmoothTime = 0.3f;
    public Vector2 pitchMinMax = new Vector2(-40, 85);
    public InputSettings inputSettings;

    float yaw;
    float pitch;
    float smoothYaw;
    float smoothPitch;

    float yawSmoothV;
    float pitchSmoothV;
    bool debug_playerFrozen;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        curTilt = transform.localEulerAngles.z;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        RotateMainCamera();
    }

    void FixedUpdate()
    {
        float addedFov = rb.velocity.magnitude - 3.44f;
        fov = Mathf.Lerp(fov, baseFov + addedFov, 0.5f);
        fov = Mathf.Clamp(fov, baseFov, maxFov);
        mainCamera.fieldOfView = fov;
        weaponCamera.fieldOfView = fov;

        currentLook = Vector2.Lerp(currentLook, currentLook + sway, 0.8f);
        curTilt = Mathf.LerpAngle(curTilt, wishTilt * wallRunTilt, 0.05f);

        sway = Vector2.Lerp(sway, Vector2.zero, 0.2f);
    }

    void RotateMainCamera()
    {
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        mouseInput.x *= sensX;
        mouseInput.y *= sensY;

        currentLook.x += mouseInput.x;
        currentLook.y = Mathf.Clamp(currentLook.y += mouseInput.y, -90, 90);

        transform.localRotation = Quaternion.AngleAxis(-currentLook.y, Vector3.right);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, curTilt);
        transform.root.transform.localRotation = Quaternion.Euler(transform.root.transform.localRotation.eulerAngles.x, currentLook.x, transform.root.transform.localRotation.eulerAngles.z);
    }

    public void Punch(Vector2 dir)
    {
        sway += dir;
    }

    #region Setters
    public void SetTilt(float newVal)
    {
        wishTilt = newVal;
    }

    public void SetXSens(float newVal)
    {
        sensX = newVal;
    }

    public void SetYSens(float newVal)
    {
        sensY = newVal;
    }

    public void SetFov(float newVal)
    {
        baseFov = newVal;
    }
    #endregion
}
