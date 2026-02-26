using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [Header("Look Settings")]
    [SerializeField] private float minSensitivity = 50f;
    [SerializeField] private float maxSensitivity = 400f;
    [SerializeField] private float sensitivity = 180f;
    [SerializeField] private float lookSmoothTime = 0.03f;
    [SerializeField] private bool lockCursor = true;
    public Transform player;
    private float xRotation = 0f;
    private Vector2 currentLookDelta;
    private Vector2 lookDeltaVelocity;

    // Start is called before the first frame update
    void Start()
    {
        sensitivity = Mathf.Clamp(sensitivity, minSensitivity, maxSensitivity);
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CameraChange();
    }

   void CameraChange(){
        if (player == null) return;

        Vector2 targetLookDelta = new Vector2(
            Input.GetAxisRaw("Mouse X"),
            Input.GetAxisRaw("Mouse Y")
        );

        currentLookDelta = Vector2.SmoothDamp(
            currentLookDelta,
            targetLookDelta,
            ref lookDeltaVelocity,
            lookSmoothTime
        );

        float mouseX = currentLookDelta.x * sensitivity * Time.deltaTime;
        float mouseY = currentLookDelta.y * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        player.Rotate(Vector3.up * mouseX);

    }

    // 可供 UI Slider 绑定：动态调整视角灵敏度
    public void SetSensitivity(float value)
    {
        sensitivity = Mathf.Clamp(value, minSensitivity, maxSensitivity);
    }

    public float GetSensitivity()
    {
        return sensitivity;
    }

}
